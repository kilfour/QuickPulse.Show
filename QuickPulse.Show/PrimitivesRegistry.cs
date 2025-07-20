using System.Globalization;

namespace QuickPulse.Show;

public class PrimitivesRegistry
{
    private readonly Dictionary<Type, Func<object, string>> registered = new();

    public PrimitivesRegistry()
    {
        Register<double>(x => ((double)x).ToString("G", CultureInfo.InvariantCulture));
        Register<string>(x => x == null ? "null" : $"\"{x}\"");
        Register<bool>(x => (bool)x ? "true" : "false");
        Register<char>(x => $"'{x}'");
        Register<decimal>(x => ((decimal)x).ToString("G", CultureInfo.InvariantCulture));
        Register<float>(x => ((float)x).ToString("G", CultureInfo.InvariantCulture));

        Register<DateTime>(x => ((DateTime)x).ToString("O"));
        Register<DateOnly>(x => ((DateOnly)x).ToString("O"));
        Register<TimeOnly>(x => ((TimeOnly)x).ToString("O"));

        Register<int>(x => x.ToString());

        Register<byte>(x => x.ToString());
        Register<sbyte>(x => x.ToString());
        Register<short>(x => x.ToString());
        Register<ushort>(x => x.ToString());
        Register<uint>(x => x.ToString());
        Register<long>(x => x.ToString());
        Register<ulong>(x => x.ToString());

        Register<Guid>(x => x.ToString());
        Register<DayOfWeek>(x => x.ToString());
    }

    private void Register<T>(Func<T, string> show)
    {
        registered[typeof(T)] = x => show((T)x!);
    }

    public bool HasType(Type type)
    {
        return registered.ContainsKey(type);
    }

    public Func<object?, string>? Get(Type type)
    {
        if (registered.TryGetValue(type, out var val)) return val!;
        return null;
    }
}
