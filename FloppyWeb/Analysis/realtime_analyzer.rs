use std::sync::Arc;
use tokio::sync::{mpsc, Mutex};
use serde::{Serialize, Deserialize};
use futures::stream::{self, StreamExt};

#[derive(Debug, Serialize, Deserialize)]
pub struct AnalysisResult {
    pub page_load_time: f64,
    pub memory_usage: u64,
    pub cpu_usage: f64,
    pub network_stats: NetworkStats,
    pub performance_score: f64,
}

pub struct RealtimeAnalyzer {
    data_channel: mpsc::Sender<AnalysisData>,
    results: Arc<Mutex<Vec<AnalysisResult>>>,
    config: AnalyzerConfig,
}

impl RealtimeAnalyzer {
    pub fn new(config: AnalyzerConfig) -> Self {
        let (tx, rx) = mpsc::channel(1000);
        let results = Arc::new(Mutex::new(Vec::new()));
        let analyzer = Self {
            data_channel: tx,
            results: results.clone(),
            config,
        };

        // Start background processing
        tokio::spawn(Self::process_data(rx, results, config));
        analyzer
    }

    pub async fn analyze_page(&self, url: &str) -> Result<AnalysisResult, AnalysisError> {
        let data = self.collect_metrics(url).await?;
        self.data_channel.send(data).await?;

        let result = self.process_metrics(data).await?;
        self.results.lock().await.push(result.clone());

        Ok(result)
    }

    async fn process_metrics(&self, data: AnalysisData) -> Result<AnalysisResult, AnalysisError> {
        let mut processors = vec![
            Box::new(PerformanceAnalyzer::new(&self.config)),
            Box::new(MemoryAnalyzer::new(&self.config)),
            Box::new(NetworkAnalyzer::new(&self.config)),
        ];

        let results = stream::iter(processors)
            .map(|processor| processor.analyze(&data))
            .buffer_unordered(3)
            .collect::<Vec<_>>()
            .await;

        self.combine_results(results)
    }

    fn combine_results(&self, results: Vec<ProcessorResult>) -> Result<AnalysisResult, AnalysisError> {
        let mut combined = AnalysisResult::default();
        
        for result in results {
            match result {
                ProcessorResult::Performance(score) => {
                    combined.performance_score = score;
                }
                ProcessorResult::Memory(usage) => {
                    combined.memory_usage = usage;
                }
                ProcessorResult::Network(stats) => {
                    combined.network_stats = stats;
                }
            }
        }

        Ok(combined)
    }

    async fn collect_metrics(&self, url: &str) -> Result<AnalysisData, AnalysisError> {
        let mut collectors = vec![
            Box::new(PerformanceCollector::new()),
            Box::new(MemoryCollector::new()),
            Box::new(NetworkCollector::new()),
        ];

        stream::iter(collectors)
            .map(|collector| collector.collect(url))
            .buffer_unordered(3)
            .collect::<Vec<_>>()
            .await
            .into_iter()
            .collect()
    }
} 