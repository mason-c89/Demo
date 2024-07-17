using System.Reflection;

namespace IOCDemoByConstructor.IOCS;

public class IOCFactory
{
    // 储存类型信息
    private readonly Dictionary<string, Type> _registeredTypes = new Dictionary<string, Type>();

    // 储存对象实例
    private readonly Dictionary<string, object> _objects = new Dictionary<string, object>();

    public IOCFactory()
    {
        // 加载程序集并获取所有类型
        var assembly = Assembly.Load("IOCDemoByConstructor");
        Type[] types = assembly.GetTypes();

        foreach (Type type in types)
        {
            // 检查类型是否有 IOCService 特性
            if (type.GetCustomAttribute(typeof(IOCService)) != null)
            {
                RegisterType(type);
            }
        }
    }

    // 注册类型信息
    private void RegisterType(Type type)
    {
        var key = type.FullName;
        if (key != null)
        {
            _registeredTypes.TryAdd(key, type);
        }
    }

    // 创建实例
    private object CreateInstance(Type type)
    {
        var constructorInfos = type.GetConstructors();

        // 按参数数量降序排序
        constructorInfos = constructorInfos.OrderByDescending(c => c.GetParameters().Length).ToArray();

        foreach (var constructorInfo in constructorInfos)
        {
            var parameters = constructorInfo.GetParameters();
            var parameterValues = new object[parameters.Length];

            bool canInstantiate = true;

            for (int i = 0; i < parameters.Length; i++)
            {
                var parameterType = parameters[i].ParameterType;
                var parameterName = parameterType.FullName;

                if (_objects.TryGetValue(parameterName, out var parameterValue))
                {
                    parameterValues[i] = parameterValue;
                }
                else
                {
                    try
                    {
                        // 尝试创建依赖项实例并注册
                        var dependencyInstance = CreateInstance(parameterType);
                        _objects[parameterName] = dependencyInstance;
                        parameterValues[i] = dependencyInstance;
                    }
                    catch
                    {
                        canInstantiate = false;
                        break;
                    }
                }
            }

            if (canInstantiate)
            {
                return constructorInfo.Invoke(parameterValues);
            }
        }

        throw new InvalidOperationException($"Cannot instantiate type {type.FullName}. No suitable constructor found.");
    }

    public object GetObject(string key)
    {
        if (!_objects.TryGetValue(key, out var obj))
        {
            if (_registeredTypes.TryGetValue(key, out var type))
            {
                obj = CreateInstance(type);
                _objects[key] = obj;
            }
            else
            {
                throw new KeyNotFoundException($"Type '{key}' not found in IOC container.");
            }
        }

        return obj;
    }

    public T GetObject<T>()
    {
        var key = typeof(T).FullName;
        if (key == null)
        {
            throw new InvalidOperationException($"'{typeof(T).FullName}' is null.");
        }

        return (T)GetObject(key);
    }
}