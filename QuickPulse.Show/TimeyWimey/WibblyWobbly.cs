namespace QuickPulse.Show.TimeyWimey;

public static class WibblyWobbly
{
    // , TimeOnly? time = null
    public static DateTime January(this int day, int year) { return new DateTime(year, 1, day); }
    public static DateTime February(this int day, int year) { return new DateTime(year, 2, day); }
    public static DateTime March(this int day, int year) { return new DateTime(year, 3, day); }
    public static DateTime April(this int day, int year) { return new DateTime(year, 4, day); }
    public static DateTime May(this int day, int year) { return new DateTime(year, 5, day); }
    public static DateTime June(this int day, int year) { return new DateTime(year, 6, day); }
    public static DateTime July(this int day, int year) { return new DateTime(year, 7, day); }
    public static DateTime August(this int day, int year) { return new DateTime(year, 8, day); }
    public static DateTime September(this int day, int year) { return new DateTime(year, 9, day); }
    public static DateTime October(this int day, int year) { return new DateTime(year, 10, day); }
    public static DateTime November(this int day, int year) { return new DateTime(year, 11, day); }
    public static DateTime December(this int day, int year) { return new DateTime(year, 12, day); }


    public static string ToHumanDateTime(this DateTime datetime) =>
        $"{datetime.Day}.{months[datetime.Month]}({datetime.Year})"; // TimeOnly.FromDateTime(date)

    public static string ToHumanDate(this DateOnly date) =>
        $"{date.Day}.{months[date.Month]}({date.Year})";

    public static string ToHumanTime(this TimeOnly time)
    {
        var h = time.Hour;
        var m = time.Minute;
        var s = time.Second;
        var ms = time.Millisecond;

        if (s == 0 && ms == 0)
            return $"{h}.Hours({m:D2})";

        if (ms == 0)
            return $"{h}.Hours({m:D2}.Minutes({s:D2}))";

        return $"{h}.Hours({m:D2}.Minutes({s:D2}.Seconds({ms:D3})))";
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

    // public static TimeOnly Hours(this int hours)
    // {
    //     return new TimeOnly(hours, 0);
    // }
}