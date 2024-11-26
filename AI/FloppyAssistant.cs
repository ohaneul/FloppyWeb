using System;
using System.Threading.Tasks;
using FloppyWeb.AI.Models;

public class FloppyAssistant
{
    private readonly ILanguageModel languageModel;
    private readonly IContextManager contextManager;
    private readonly WebPageAnalyzer pageAnalyzer;

    public FloppyAssistant()
    {
        languageModel = new GPTModel(); // Or any other AI model
        contextManager = new ContextManager();
        pageAnalyzer = new WebPageAnalyzer();
    }

    public async Task<AssistantResponse> ProcessQuery(string query, WebPageContext currentPage)
    {
        var context = contextManager.GetCurrentContext();
        context.AddWebPage(currentPage);

        var response = await languageModel.GenerateResponse(query, context);
        return new AssistantResponse
        {
            TextResponse = response.Text,
            SuggestedActions = response.Actions,
            RelatedContent = response.References
        };
    }

    public async Task<PageSummary> AnalyzeCurrentPage(string pageContent)
    {
        return await pageAnalyzer.AnalyzePage(pageContent);
    }
} 