namespace Cms.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddOrchardCms()
                .AddMvc();
            
            var app = builder.Build();
          
            app.UseRouting();
            app.UseStaticFiles();
            app.UseOrchardCore();

            app.Run();
        }
    }
}
