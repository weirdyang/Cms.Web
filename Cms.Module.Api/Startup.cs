using Cms.Module.Api.TestObject;
using Cms.Module.Api.Workflows.Scripting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Workflows.Drivers;
using OrchardCore.Workflows.Options;
using OrchardCore.Workflows.Timers;
using OriginalScriptTask = OrchardCore.Workflows.Activities.ScriptTask;
namespace Cms.Module.Api
{
    [Feature("Cms.Module.Api")]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
        }



        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
        }
    }
    [Feature("Cms.Module.Data")]
    public class DataStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddTestObject();
        }



        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
        }
    }
    [Feature("Cms.Module.Scripting")]
    public class TimerStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<WorkflowOptions>(options =>
            {
                if (options.IsActivityRegistered<OriginalScriptTask>())
                {
                    options.UnregisterActivityType<OriginalScriptTask>();
                }
                options.RegisterActivity<DebugScript, Cms.Module.Api.Workflows.Scripting.DebugScriptDisplayDriver>();
            });

        }
    }
}