using System.Globalization;
using System.Text;
using WibblyWobbly;

namespace QuickPulse.Show.Bolts.State;

public class PrimitivesRegistry
{
    private readonly Dictionary<Type, Func<object, string>> registered = new();

    private static string FormatString(string value)
    {
        var result = new StringBuilder(value.Length + 2);
        result.Append('"');

        foreach (var character in value)
        {
            switch (character)
            {
                case '"': result.Append("\\\""); break;
                case '\\': result.Append("\\\\"); break;
                case '\0': result.Append("\\0"); break;
                case '\a': result.Append("\\a"); break;
                case '\b': result.Append("\\b"); break;
                case '\f': result.Append("\\f"); break;
                case '\n': result.Append("\\n"); break;
                case '\r': result.Append("\\r"); break;
                case '\t': result.Append("\\t"); break;
                case '\v': result.Append("\\v"); break;

                default:
                    if (char.IsControl(character) ||
                        character != ' ' && char.IsWhiteSpace(character))
                    {
                        result
                            .Append("\\u")
                            .Append(((int)character).ToString(
                                "X4",
                                CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        result.Append(character);
                    }

                    break;
            }
        }

        return result.Append('"').ToString();
    }

    public PrimitivesRegistry()
    {
        Register<double>(x => x.ToString("G", CultureInfo.InvariantCulture));
        Register<string>(FormatString);
        Register<bool>(x => x ? "true" : "false");
        Register<char>(x => $"'{x}'");
        Register<decimal>(x => x.ToString("G", CultureInfo.InvariantCulture));
        Register<float>(x => ((float)x).ToString("G", CultureInfo.InvariantCulture));

        Register<int>(x => x.ToString());

        Register<byte>(x => x.ToString());
        Register<sbyte>(x => x.ToString());
        Register<short>(x => x.ToString());
        Register<ushort>(x => x.ToString());
        Register<uint>(x => x.ToString());
        Register<long>(x => x.ToString());
        Register<ulong>(x => x.ToString());
        Register<Half>(x => x.ToString());

        Register<Guid>(x => x.ToString());

        Register<DateTime>(x => x.ToString("O"));
        Register<DateOnly>(x => x.ToString("O"));
        Register<TimeOnly>(x => x.ToString("O"));
    }

    public void UsingWibblyWobbly(bool noSeconds)
    {
        Register<DateTime>(x => x.ToHumanDate(noSeconds));
        Register<DateOnly>(x => x.ToHumanDateOnly());
        Register<TimeOnly>(x => x.ToHumanTime(noSeconds));
    }

    public void Register<T>(Func<T, string> show)
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
