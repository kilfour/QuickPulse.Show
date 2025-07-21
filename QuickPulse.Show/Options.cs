using System.Reflection;

namespace QuickPulse.Show;

public class Options
{
    public PrimitivesRegistry Registry { get; } = new PrimitivesRegistry();

    public Dictionary<Type, Func<object, string>> TypeRegistry { get; } = new();

    public void RegisterTypeFormatter<T>(Func<T, string> formatter)
    {
        TypeRegistry[typeof(T)] = a => formatter((T)a);
    }

    public Dictionary<Type, List<FieldInfo>> FieldsToIgnore { get; } = [];
    public void RegisterFieldToIgnore<T>(FieldInfo field)
    {
        if (!FieldsToIgnore.ContainsKey(typeof(T)))
            FieldsToIgnore[typeof(T)] = [];
        FieldsToIgnore[typeof(T)].Add(field);
    }


    public Dictionary<Type, List<PropertyInfo>> PropertiesToIgnore { get; } = [];
    public void RegisterPropertyToIgnore<T>(PropertyInfo prop)
    {
        if (!PropertiesToIgnore.ContainsKey(typeof(T)))
            PropertiesToIgnore[typeof(T)] = [];
        PropertiesToIgnore[typeof(T)].Add(prop);
    }
}