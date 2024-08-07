using System.Reflection;
using Autofac;
using MqDemo.Api.Extension;
using Module = Autofac.Module;

namespace MqDemo.Api;

public class MqDemoModule : Module
{
    private readonly IConfiguration _configuration;
    private readonly Assembly[] _assemblies;

    public MqDemoModule(IConfiguration configuration, params Assembly[] assemblies)
    {
        _configuration = configuration;
        _assemblies = assemblies;

        if (_assemblies == null || !_assemblies.Any())
        {
            throw new ArgumentException("No assemblies found to scan. Supply at least one assembly to scan.");
        }
    }
    
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterMultiBus(_configuration, _assemblies);

        base.Load(builder);
    }
}