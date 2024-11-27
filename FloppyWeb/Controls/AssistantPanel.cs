using System;
using System.Windows.Forms;
using FloppyWeb.AI.Models;
using FloppyWeb.AI.Automation;
using FloppyWeb.AI.Context;
using FloppyWeb.AI.WebPageAnalyzer;

public class AssistantPanel : UserControl
{
    private readonly FloppyAssistant assistant;
    private TextBox queryInput;
    private RichTextBox responseArea;
    private FlowLayoutPanel actionPanel;

    public AssistantPanel()
    {
        assistant = new FloppyAssistant();
        InitializeComponents();
        SetupEventHandlers();
    }

    private async void OnQuerySubmitted(object sender, EventArgs e)
    {
        string query = queryInput.Text;
        var currentPage = GetCurrentPageContext();
        
        var response = await assistant.ProcessQuery(query, currentPage);
        DisplayResponse(response);
    }

    private void DisplayResponse(AssistantResponse response)
    {
        responseArea.Text = response.TextResponse;
        UpdateActionButtons(response.SuggestedActions);
    }
} 