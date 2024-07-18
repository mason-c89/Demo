using IOCDemoLifetime.Enum;

namespace IOCDemoLifetime.IOCS;

public class IOCFactory
{
    private readonly Dictionary<Type, Type> _implementationMappings = new();
    private readonly Dictionary<Type, object> _scopedServices = new();
    private readonly Dictionary<Type, object> _singletonServices = new();
    private readonly Dictionary<Type, ServiceLifetime> _typeMappings = new();

    public void RegisterTransient<TService, TImplementation>()
        where TService : class where TImplementation : TService, new()
    {
        SetLifetimeType(typeof(TService), ServiceLifetime.Transient);
        _implementationMappings[typeof(TService)] = typeof(TImplementation);
    }

    public void RegisterScoped<TService, TImplementation>()
        where TService : class where TImplementation : TService, new()
    {
        SetLifetimeType(typeof(TService), ServiceLifetime.Scoped);
        _implementationMappings[typeof(TService)] = typeof(TImplementation);
    }

    public void RegisterSingleton<TService, TImplementation>()
        where TService : class where TImplementation : TService, new()
    {
        SetLifetimeType(typeof(TService), ServiceLifetime.Singleton);
        _implementationMappings[typeof(TService)] = typeof(TImplementation);
    }

    private void SetLifetimeType(Type type, ServiceLifetime lifetime)
    {
        if (!_typeMappings.TryAdd(type, lifetime)) _typeMappings[type] = lifetime;
    }

    private ServiceLifetime GetLifetimeType(Type type)
    {
        if (!_typeMappings.TryGetValue(type, out var lifetime)) throw new Exception("Value not found");

        return lifetime;
    }

    private object CreateInstance(Type type, Dictionary<Type, object> services)
    {
        if (_implementationMappings.TryGetValue(type, out var implementationType))
        {
            var constructorInfos = implementationType.GetConstructors();
            constructorInfos = constructorInfos.OrderByDescending(c => c.GetParameters().Length).ToArray();

            foreach (var constructorInfo in constructorInfos)
            {
                var parameters = constructorInfo.GetParameters();
                var parameterValues = new object[parameters.Length];

                var canInstantiate = true;

                for (var i = 0; i < parameters.Length; i++)
                {
                    var parameterType = parameters[i].ParameterType;

                    if (services.TryGetValue(parameterType, out var parameterValue))
                        parameterValues[i] = parameterValue;
                    else
                        try
                        {
                            var dependencyInstance = CreateInstance(parameterType, services);
                            services[parameterType] = dependencyInstance;
                            parameterValues[i] = dependencyInstance;
                        }
                        catch
                        {
                            canInstantiate = false;
                            break;
                        }
                }

                if (canInstantiate) return constructorInfo.Invoke(parameterValues);
            }

            throw new InvalidOperationException(
                $"Cannot instantiate type {implementationType.FullName}. No suitable constructor found.");
        }

        throw new InvalidOperationException($"No implementation registered for type {type.FullName}");
    }

    public TService Resolve<TService>() where TService : class
    {
        var key = typeof(TService);
        var lifetime = GetLifetimeType(key);
        object instance;

        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                if (!_singletonServices.TryGetValue(key, out instance))
                {
                    instance = CreateInstance(key, _singletonServices);
                    _singletonServices[key] = instance;
                }

                break;

            case ServiceLifetime.Scoped:
                if (!_scopedServices.TryGetValue(key, out instance))
                {
                    instance = CreateInstance(key, _scopedServices);
                    _scopedServices[key] = instance;
                }

                break;

            case ServiceLifetime.Transient:
                instance = CreateInstance(key, new Dictionary<Type, object>());
                break;

            default:
                throw new Exception("error");
        }

        return instance as TService ?? throw new InvalidOperationException();
    }

    public void BeginScope()
    {
        _scopedServices.Clear();
    }
}