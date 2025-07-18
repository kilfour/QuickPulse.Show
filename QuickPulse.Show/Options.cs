using System.Reflection;

namespace QuickPulse.Show;

public class Options
{
    public Dictionary<Type, List<FieldInfo>> FieldsToIgnore { get; } = [];
    public Dictionary<Type, List<PropertyInfo>> PropertiesToIgnore { get; } = [];

    public void RegisterFieldToIgnore<T>(FieldInfo field)
    {
        if (!FieldsToIgnore.ContainsKey(typeof(T)))
            FieldsToIgnore[typeof(T)] = [];
        FieldsToIgnore[typeof(T)].Add(field);
    }

    public void RegisterPropertyToIgnore<T>(PropertyInfo prop)
    {
        if (!PropertiesToIgnore.ContainsKey(typeof(T)))
            PropertiesToIgnore[typeof(T)] = [];
        PropertiesToIgnore[typeof(T)].Add(prop);
    }
}