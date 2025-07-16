using System.Globalization;

namespace QuickPulse.Show;

public static class Registry
{
    private static readonly Dictionary<Type, Func<object, string>> _instances = new();

    static Registry()
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

    public static void Register<T>(Func<T, string> show)
    {
        _instances[typeof(T)] = x => show((T)x!);
    }

    public static Func<object?, string>? Get(Type type)
    {
        if (_instances.TryGetValue(type, out var val)) return val!;
        return null;
    }
}
