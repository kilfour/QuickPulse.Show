using System.Collections;

namespace QuickPulse.Show.Bolts;

public static class EnumerableExtensions
{
    public static int Count(this IEnumerable source)
    {
        if (source is ICollection collection)
            return collection.Count;

        int count = 0;
        var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
            count++;
        return count;
    }
}