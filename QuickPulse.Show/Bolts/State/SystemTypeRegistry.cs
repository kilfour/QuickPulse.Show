namespace QuickPulse.Show.Bolts.State;

public class SystemTypeRegistry
{
    private readonly Dictionary<Type, Func<object, string>> registered = new();

    public SystemTypeRegistry()
    {
        Register<Type>(x => x.Name);
    }

    public void Register<T>(Func<T, string> show)
    {
        registered[typeof(T)] = x => show((T)x!);
    }

    public bool HasType(Type type)
    {
        return registered.ContainsKey(type);
    }

    public Func<object, string>? Get(Type type)
    {
        if (registered.TryGetValue(type, out Func<object, string>? formatter))
            return formatter;

        foreach (var (targetType, f) in registered)
        {
            if (targetType.IsAssignableFrom(type))
            {
                return f;
            }
        }
        return default!;
    }
}
