using QuickPulse.Show.Tests._tools;


namespace QuickPulse.Show.Tests.Flat;

public class PrimitiveTests : AbstractFlowTests
{
    [Fact]
    public void Pulse_Null() =>
        Assert.Equal("null", Pulse(null!));

    [Fact]
    public void Pulse_Int() =>
        Assert.Equal("42", Pulse(42));

    [Fact]
    public void Pulse_Double() =>
        Assert.Equal("3.14", Pulse(3.14));

    [Fact]
    public void Pulse_String() =>
        Assert.Equal("\"hello\"", Pulse("hello"));

    [Fact]
    public void Pulse_Bool_True() =>
        Assert.Equal("true", Pulse(true));

    [Fact]
    public void Pulse_Bool_False() =>
        Assert.Equal("false", Pulse(false));

    [Fact]
    public void Pulse_Char() =>
        Assert.Equal("'A'", Pulse('A'));

    [Fact]
    public void Pulse_Decimal() =>
        Assert.Equal("123.45", Pulse(123.45m));

    [Fact]
    public void Pulse_Float() =>
        Assert.Equal("2.5", Pulse(2.5f));

    [Fact]
    public void Pulse_Byte() =>
        Assert.Equal("255", Pulse((byte)255));

    [Fact]
    public void Pulse_SByte() =>
        Assert.Equal("-5", Pulse((sbyte)-5));

    [Fact]
    public void Pulse_Short() =>
        Assert.Equal("-1234", Pulse((short)-1234));

    [Fact]
    public void Pulse_UShort() =>
        Assert.Equal("1234", Pulse((ushort)1234));

    [Fact]
    public void Pulse_UInt() =>
        Assert.Equal("1234567890", Pulse(1234567890u));

    [Fact]
    public void Pulse_Long() =>
        Assert.Equal("9223372036854775807", Pulse(long.MaxValue));

    [Fact]
    public void Pulse_ULong() =>
        Assert.Equal("18446744073709551615", Pulse(ulong.MaxValue));

    [Fact]
    public void Pulse_DateTime() =>
        Assert.Equal("2023-01-01T12:00:00.0000000Z", Pulse(DateTime.Parse("2023-01-01T12:00:00Z").ToUniversalTime()));

    [Fact]
    public void Pulse_Guid() =>
        Assert.Equal("01234567-89ab-cdef-0123-456789abcdef", Pulse(Guid.Parse("01234567-89ab-cdef-0123-456789abcdef")));

    [Fact]
    public void Pulse_DateOnly() =>
        Assert.Equal("2023-07-16", Pulse(DateOnly.Parse("2023-07-16")));

    [Fact]
    public void Pulse_TimeOnly() =>
        Assert.Equal("13:45:30.0000000", Pulse(TimeOnly.Parse("13:45:30")));

    [Fact]
    public void Pulse_DayOfWeek() =>
        Assert.Equal("Monday", Pulse(DayOfWeek.Monday));
}
