using System.Reflection;
using QuickPulse.Show.Bolts.State;

namespace QuickPulse.Show.Bolts;

public class Puzzles
{
    public bool PrettyPrint { get; set; } = false;
    public bool WithClass { get; set; } = false;

    public PrimitivesRegistry Registry { get; } = new PrimitivesRegistry();

    public SystemTypeRegistry SystemTypeRegistry { get; init; } = new SystemTypeRegistry();
    public void RegisterSystemTypeFormatter<T>(Func<T, string> formatter)
        => SystemTypeRegistry.Register(formatter);

    public Dictionary<Type, Func<object, string>> TypeRegistry { get; } = [];
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

    public Dictionary<Type, Func<object, string>> SelfReferencingRegistry { get; } = [];

    public void SelfReferencingFormatter<T>(Func<T, string> formatter)
    {
        SelfReferencingRegistry[typeof(T)] = a => formatter((T)a);
    }

    public HashSet<Type> InlinedTypes { get; } = [];

    public void InlineType(Type type)
    {
        InlinedTypes.Add(type);
    }

    public Dictionary<Func<Type, bool>, Func<object, object>> Formatters { get; } = [];
    public void RegisterFormatter(Func<Type, bool> predicate, Func<object, object> formatter)
    {
        Formatters[predicate] = formatter;
    }
}