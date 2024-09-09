using Autofac;
using Serilog;

namespace MqDemo.Api;

public class Startup
{
    private IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        // services.AddMassTransit(x =>
        // {
        //     x.AddConsumer<QueryProductConsumer>();
        //
        //     x.UsingRabbitMq((context, cfg) =>
        //     {
        //         cfg.Host("localhost", "/", h => { });
        //
        //         // 配置FileReceived的接收端点
        //         cfg.ReceiveEndpoint("query_product_queue", e =>
        //         {
        //             e.ConfigureConsumer<QueryProductConsumer>(context);
        //         });
        //     });
        // });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
    
    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterModule(new MqDemoModule(Configuration, typeof(MqDemoModule).Assembly));
        builder.RegisterModule(new LogModule(Log.Logger));
    }
}