using Jint.Runtime;
using Microsoft.Extensions.Localization;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Activities;
using OrchardCore.Workflows.Display;
using OrchardCore.Workflows.Models;
using OrchardCore.Workflows.Scripting;
using OrchardCore.Workflows.Services;
using OrchardCore.Workflows.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Cms.Module.Api.Workflows.Scripting
{
    public class DebugScript : TaskActivity<DebugScript>
    {
        private readonly IWorkflowScriptEvaluator _scriptEvaluator;
        protected readonly IStringLocalizer S;

        public DebugScript(IWorkflowScriptEvaluator scriptEvaluator, IStringLocalizer<DebugScript> localizer)
        {
            _scriptEvaluator = scriptEvaluator;
            S = localizer;
        }

        public override LocalizedString DisplayText => S["Script Task"];

        public override LocalizedString Category => S["Control Flow"];

        public IList<string> AvailableOutcomes
        {
            get => GetProperty(() => new List<string> { "Done" });
            set => SetProperty(value);
        }

        /// <summary>
        /// The script can call any available functions, including setOutcome().
        /// </summary>
        public WorkflowExpression<object> Script
        {
            get => GetProperty(() => new WorkflowExpression<object>("setOutcome('Done');"));
            set => SetProperty(value);
        }

        public override IEnumerable<Outcome> GetPossibleOutcomes(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            return Outcomes(AvailableOutcomes.Select(x => S[x]).ToArray());
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            var outcomes = new List<string>();
            try
            {
                workflowContext.LastResult = await _scriptEvaluator.EvaluateAsync(Script, workflowContext, new OutcomeMethodProvider(outcomes));

            }
            catch (JavaScriptException ex)
            {
                // throw custom exception
                throw new InvalidOperationException(string.Format("Error at Line:{0} Column: {1} | {2}",
                    ex.Location.Start.Line,
                    ex.Location.Start.Column,
                    ex.Message), ex);

            }

            return Outcomes(outcomes);
        }
    }
    public class DebugScriptViewModel
    {
        [Required]
        public string AvailableOutcomes { get; set; }

        [Required]
        public string Script { get; set; }
    }
    public class DebugScriptDisplayDriver : ActivityDisplayDriver<DebugScript, DebugScriptViewModel>
    {
        protected override void EditActivity(DebugScript source, DebugScriptViewModel model)
        {
            model.AvailableOutcomes = string.Join(", ", source.AvailableOutcomes);
            model.Script = source.Script.Expression;
        }

        protected override void UpdateActivity(DebugScriptViewModel model, DebugScript activity)
        {
            activity.AvailableOutcomes = model.AvailableOutcomes.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
            activity.Script = new WorkflowExpression<object>(model.Script);
        }
    }
}