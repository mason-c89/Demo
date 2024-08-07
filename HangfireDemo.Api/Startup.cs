using Autofac;
using Hangfire;
using Hangfire.Redis.StackExchange;
using HangfireDemo.Core;
using HangfireDemo.Core.Data;
using Microsoft.EntityFrameworkCore;
using PractiseForMason.Core.Domain;

namespace HangfireDemo.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        // 配置 Hangfire 使用 Redis
        services.AddHangfire(config => config.UseRedisStorage());
        
        // 添加 Hangfire 服务器
        services.AddHangfireServer();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // 启用 Hangfire 仪表盘
        app.UseHangfireDashboard();
        // 注册一个示例作业
        // backgroundJobs.Enqueue(() => Console.WriteLine("Hello, Hangfire with Redis!"));
        
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterModule(new HangfireDemoModule());
    }
}