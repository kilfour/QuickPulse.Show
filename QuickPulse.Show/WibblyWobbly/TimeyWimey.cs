using QuickPulse.Instruments;

namespace WibblyWobbly;

public static class TimeyWimey
{
    public static DateTime January(this int day, int year)
        => new(year, 1, day);
    public static DateTime January(this int day, int year, TimeOnly timeOnly)
        => new DateTime(year, 1, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);

    public static DateTime February(this int day, int year)
        => new DateTime(year, 2, day);
    public static DateTime February(this int day, int year, TimeOnly timeOnly)
        => new DateTime(year, 2, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);

    public static DateTime March(this int day, int year)
        => new DateTime(year, 3, day);
    public static DateTime March(this int day, int year, TimeOnly timeOnly)
        => new DateTime(year, 3, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);

    public static DateTime April(this int day, int year)
        => new DateTime(year, 4, day);
    public static DateTime April(this int day, int year, TimeOnly timeOnly)
        => new DateTime(year, 4, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);

    public static DateTime May(this int day, int year)
        => new DateTime(year, 5, day);
    public static DateTime May(this int day, int year, TimeOnly timeOnly)
        => new DateTime(year, 5, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);

    public static DateTime June(this int day, int year)
        => new DateTime(year, 6, day);
    public static DateTime June(this int day, int year, TimeOnly timeOnly)
        => new DateTime(year, 6, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);

    public static DateTime July(this int day, int year)
        => new DateTime(year, 7, day);
    public static DateTime July(this int day, int year, TimeOnly timeOnly)
        => new DateTime(year, 7, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);

    public static DateTime August(this int day, int year)
        => new DateTime(year, 8, day);
    public static DateTime August(this int day, int year, TimeOnly timeOnly)
        => new DateTime(year, 8, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);

    public static DateTime September(this int day, int year)
        => new DateTime(year, 9, day);
    public static DateTime September(this int day, int year, TimeOnly timeOnly)
        => new DateTime(year, 9, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);

    public static DateTime October(this int day, int year)
        => new DateTime(year, 10, day);
    public static DateTime October(this int day, int year, TimeOnly timeOnly)
        => new DateTime(year, 10, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);

    public static DateTime November(this int day, int year)
        => new DateTime(year, 11, day);
    public static DateTime November(this int day, int year, TimeOnly timeOnly)
        => new DateTime(year, 11, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);

    public static DateTime December(this int day, int year)
        => new DateTime(year, 12, day);
    public static DateTime December(this int day, int year, TimeOnly timeOnly)
        => new DateTime(year, 12, day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second);


    // public static string ToHumanDateOnly(this DateOnly datetime) =>
    //     $"{datetime.Day}.{months[datetime.Month]}({datetime.Year})";

    // public static string ToHumanDate(this DateOnly date) =>
    //     $"{date.Day}.{months[date.Month]}({date.Year})";

    // public static string ToHumanTime(this TimeOnly time)
    // {
    //     var h = time.Hour;
    //     var m = time.Minute;
    //     var s = time.Second;
    //     var ms = time.Millisecond;

    //     if (s == 0 && ms == 0)
    //         return $"{h}.OClock({m:D2})";

    //     if (ms == 0)
    //         return $"{h}.OClock({m:D2}.Minutes({s:D2}))";

    //     return $"{h}.OClock({m:D2}.Minutes({s:D2}.Seconds({ms:D3})))";
    // }

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
        => new TimeOnly(CheckHour(hours), 0);

    public static TimeOnly PM(this TimeOnly timeOnly)
    {
        var hour = timeOnly.Hour;
        if (hour < 1) ComputerSays.No("Hour must be bigger than zero.");
        if (hour > 12) ComputerSays.No("Hour must be smaller than 13.");
        return timeOnly.Add(new TimeSpan(12, 0, 0));
    }

    public static TimeOnly To(this int minutes, int hours)
        => new TimeOnly(CheckHour(hours), 0).Add(new TimeSpan(0, 0 - CheckMinutes(minutes), 0));

    public static TimeOnly Past(this int minutes, int hours)
        => new TimeOnly(CheckHour(hours), CheckMinutes(minutes));

    private static int CheckHour(int hour)
    {
        if (hour < 1) ComputerSays.No("Hour must be bigger than zero.");
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
