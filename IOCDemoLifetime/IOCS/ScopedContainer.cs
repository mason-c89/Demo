namespace IOCDemoLifetime.IOCS;

public class ScopedContainer
{
    private readonly Dictionary<string, object> _scopedObjects = new();

    public object GetOrCreateInstance(string key, Func<object> instanceCreator)
    {
        if (!_scopedObjects.TryGetValue(key, out var instance))
        {
            instance = instanceCreator();
            _scopedObjects[key] = instance;
        }

        return instance;
    }
}