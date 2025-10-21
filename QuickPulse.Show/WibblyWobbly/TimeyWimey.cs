using System.ComponentModel;
using QuickPulse.Instruments;

namespace WibblyWobbly;

[EditorBrowsable(EditorBrowsableState.Advanced)]
public static class TimeyWimey
{
    public static DateOnly January(this int day, int year)
        => new(year, 1, day);
    public static DateTime January(this int day, int year, TimeOnly timeOnly)
        => new(year, 1, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);

    public static DateOnly February(this int day, int year)
        => new(year, 2, day);
    public static DateTime February(this int day, int year, TimeOnly timeOnly)
        => new(year, 2, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);

    public static DateOnly March(this int day, int year)
        => new(year, 3, day);
    public static DateTime March(this int day, int year, TimeOnly timeOnly)
        => new(year, 3, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);

    public static DateOnly April(this int day, int year)
        => new(year, 4, day);
    public static DateTime April(this int day, int year, TimeOnly timeOnly)
        => new(year, 4, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);

    public static DateOnly May(this int day, int year)
        => new(year, 5, day);
    public static DateTime May(this int day, int year, TimeOnly timeOnly)
        => new(year, 5, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);

    public static DateOnly June(this int day, int year)
        => new(year, 6, day);
    public static DateTime June(this int day, int year, TimeOnly timeOnly)
        => new(year, 6, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);

    public static DateOnly July(this int day, int year)
        => new(year, 7, day);
    public static DateTime July(this int day, int year, TimeOnly timeOnly)
        => new(year, 7, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);

    public static DateOnly August(this int day, int year)
        => new(year, 8, day);
    public static DateTime August(this int day, int year, TimeOnly timeOnly)
        => new(year, 8, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);

    public static DateOnly September(this int day, int year)
        => new(year, 9, day);
    public static DateTime September(this int day, int year, TimeOnly timeOnly)
        => new(year, 9, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);

    public static DateOnly October(this int day, int year)
        => new(year, 10, day);
    public static DateTime October(this int day, int year, TimeOnly timeOnly)
        => new(year, 10, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);

    public static DateOnly November(this int day, int year)
        => new(year, 11, day);
    public static DateTime November(this int day, int year, TimeOnly timeOnly)
        => new(year, 11, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);

    public static DateOnly December(this int day, int year)
        => new(year, 12, day);
    public static DateTime December(this int day, int year, TimeOnly timeOnly)
        => new(year, 12, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);


    public static string ToHumanDateOnly(this DateTime datetime) =>
        $"{datetime.Day}.{months[datetime.Month]}({datetime.Year})";

    public static string ToHumanDateOnly(this DateOnly datetime) =>
        $"{datetime.Day}.{months[datetime.Month]}({datetime.Year})";

    public static string ToHumanDate(this DateTime date, bool noSeconds = false) =>
        $"{date.Day}.{months[date.Month]}({date.Year}).At({ToHumanTime(TimeOnly.FromDateTime(date), noSeconds)})";

    public static string ToHumanTime(this TimeOnly time, bool noSeconds = false)
    {
        var h = time.Hour;
        var m = time.Minute;
        var s = time.Second;
        var ms = time.Millisecond;
        if (m == 0 && s == 0 && ms == 0)
            return $"{h}.OClock()";
        string? result;
        if (m > 30)
            result = $"{60 - m}.To({FormatHour(h + 1)})";
        else
            result = $"{m}.Past({FormatHour(h)})";
        if (noSeconds || (s == 0 && ms == 0))
            return result;
        if (ms == 0)
            return $"{result}.Seconds({s})";
        return $"{result}.Seconds({s}, {ms})";
    }

    private static string FormatHour(int hour)
    {
        if (hour == 12)
            return "Mid.Day";
        if (hour == 00 || hour == 24)
            return "Mid.Night";
        return hour.ToString();
    }

    private static readonly Dictionary<int, string> months =
        new Dictionary<int, string>
        {
            {1, "January"},
            {2, "February"},
            {3, "March"},
            {4, "April"},
            {5, "May"},
            {6, "June"},
            {7, "July"},
            {8, "August"},
            {9, "September"},
            {10, "October"},
            {11, "November"},
            {12, "December"},
        };

    public static DateTime At(this DateOnly dateOnly, TimeOnly timeOnly)
    {
        return dateOnly.ToDateTime(timeOnly);
    }

    public static TimeOnly OClock(this int hours)
        => new(CheckHour(hours), 0);

    public static TimeOnly PM(this TimeOnly timeOnly)
    {
        var hour = timeOnly.Hour;
        if (hour == 12) return timeOnly;
        if (hour < 1) ComputerSays.No("Hour must be bigger than zero.");
        if (hour > 12) ComputerSays.No("Hour must be smaller than 13.");
        return timeOnly.Add(new TimeSpan(12, 0, 0));
    }

    public static TimeOnly To(this int minutes, int hours)
        => new TimeOnly(CheckHour(hours), 0).Add(new TimeSpan(0, 0 - CheckMinutes(minutes), 0));

    public static TimeOnly Past(this int minutes, int hours)
        => new(CheckHour(hours), CheckMinutes(minutes));

    public static TimeOnly Seconds(this TimeOnly timeOnly, int seconds, int milliseconds = 0)
        => timeOnly.Add(new TimeSpan(0, 0, 0, seconds, milliseconds));

    private static int CheckHour(int hour)
    {
        if (hour == 24) return 0;
        if (hour < 0) ComputerSays.No("Hour must be bigger or equal to zero.");
        if (hour > 24) ComputerSays.No("Hour must be smaller than 25.");
        return hour;
    }

    private static int CheckMinutes(int hour)
    {
        if (hour < 1) ComputerSays.No("Minutes must be bigger than zero.");
        if (hour > 59) ComputerSays.No("Hour must be smaller than 60.");
        return hour;
    }
}

public static class Mid
{
    public const int Day = 12;
    public const int Night = 0;
}
