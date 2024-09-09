using Autofac.Extensions.DependencyInjection;
using Serilog;
using Serilog.Exceptions;

namespace MqDemo.Api;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("Application", "mqdemo")
            .Enrich.WithCorrelationIdHeader()
            .Enrich.WithClientIp()
            .WriteTo.Console()
            .WriteTo.Seq("http://localhost:5341", apiKey: "HcDNs6L55bHrIsBJ3SgB")
            .CreateLogger();
        
        try
        {
            Log.Information("Configuring api host ({ApplicationContext})... ", "mqdemo");

            var webHost = Host.CreateDefaultBuilder(args)
                .ConfigureLogging(l => l.AddSerilog(Log.Logger))
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseStartup<Startup>();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog()
                .Build();

            Log.Information("Starting api host ({ApplicationContext})...", "mqdemo");

            webHost.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", "mqdemo");
        }
        finally
        {
            Log.CloseAndFlush();
        }
        
        
    }
}