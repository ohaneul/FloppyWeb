use std::process::{Command, Child};
use std::sync::Arc;
use tokio::sync::Mutex;

pub struct ProcessIsolator {
    processes: Arc<Mutex<Vec<Child>>>,
    memory_limit: usize,
}

impl ProcessIsolator {
    pub fn new(memory_limit: usize) -> Self {
        ProcessIsolator {
            processes: Arc::new(Mutex::new(Vec::new())),
            memory_limit,
        }
    }

    pub async fn isolate_process(&self, executable: &str) -> Result<u32, String> {
        let mut child = Command::new(executable)
            .arg("--sandbox")
            .arg(format!("--memory-limit={}", self.memory_limit))
            .spawn()
            .map_err(|e| e.to_string())?;

        let pid = child.id();
        self.processes.lock().await.push(child);
        Ok(pid)
    }

    pub async fn terminate_process(&self, pid: u32) -> Result<(), String> {
        let mut processes = self.processes.lock().await;
        if let Some(index) = processes.iter().position(|p| p.id() == pid) {
            let mut process = processes.remove(index);
            process.kill().map_err(|e| e.to_string())?;
        }
        Ok(())
    }
} 