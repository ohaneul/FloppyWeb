using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FloppyWeb.AI.Automation.Actions;

public class TaskAutomator
{
    private readonly Dictionary<string, IAutomationAction> actions;

    public TaskAutomator()
    {
        actions = new Dictionary<string, IAutomationAction>
        {
            { "fill_form", new FormFillAction() },
            { "extract_data", new DataExtractionAction() },
            { "screenshot", new ScreenshotAction() },
            { "translate", new TranslationAction() }
        };
    }

    public async Task ExecuteAction(string actionName, Dictionary<string, object> parameters)
    {
        if (actions.TryGetValue(actionName, out var action))
        {
            await action.Execute(parameters);
        }
    }
} 