using Autofac.Extensions.DependencyInjection;

namespace MqDemo.Api;

public class Program
{
    public static void Main(string[] args)
    {
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder =>
            {
                builder.UseStartup<Startup>();
            })
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .Build()
            .Run();
    }
}