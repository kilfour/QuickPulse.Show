using System.Collections;

namespace QuickPulse.Show.Bolts;

public static class Is
{
    public static bool Primitive(object obj, PrimitivesRegistry registry)
    {
        return registry.HasType(obj.GetType());
    }

    public static bool Collection(object obj)
    {
        return obj is IEnumerable && obj.GetType() != typeof(string);
    }

    public static bool Dictionary(object obj)
    {
        return obj.GetType().GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>));
    }

    public static bool KeyValuePair(object obj)
    {
        var type = obj.GetType();
        return type.IsGenericType &&
               type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>);
    }

    public static bool Object(object obj)
    {
        if (obj is null) return false;
        var t = obj.GetType();

        // Exclude:
        if (t.IsPrimitive) return false;
        if (t.IsEnum) return false;
        if (t == typeof(string)) return false;

        // // Exclude tuple & KVP (already routed)
        // if (Is.Tuple(x)) return false;
        // if (Is.KeyValuePair(x)) return false;

        // // Dictionaries/collections already routed
        // if (Is.Dictionary(x)) return false;
        // if (Is.Collection(x)) return false;

        // Everything else (including custom structs / record structs) is “object”
        return true;

        // var type = obj.GetType();
        // if (type == typeof(string)) return false;
        // return type.IsClass;
    }

    public static bool ObjectProperty(object obj)
    {
        return obj.GetType() == typeof(ObjectProperty);
    }

    public static bool Tuple(object obj)
    {
        return obj.GetType().FullName!.StartsWith("System.ValueTuple");
    }
}