namespace QuickPulse.Show.Tests.Flat;

public class PrimitiveTests
{
    [Fact]
    public void Introduce_Null() =>
        Assert.Equal("null", Introduce.This(null!), false);

    [Fact]
    public void Introduce_Int() =>
        Assert.Equal("42", Introduce.This(42), false);

    [Fact]
    public void Introduce_Double() =>
        Assert.Equal("3.14", Introduce.This(3.14), false);

    [Fact]
    public void Introduce_String() =>
        Assert.Equal("\"hello\"", Introduce.This("hello"), false);

    [Fact]
    public void Introduce_Bool_True() =>
        Assert.Equal("true", Introduce.This(true), false);

    [Fact]
    public void Introduce_Bool_False() =>
        Assert.Equal("false", Introduce.This(false), false);

    [Fact]
    public void Introduce_Char() =>
        Assert.Equal("'A'", Introduce.This('A'), false);

    [Fact]
    public void Introduce_Decimal() =>
        Assert.Equal("123.45", Introduce.This(123.45m), false);

    [Fact]
    public void Introduce_Float() =>
        Assert.Equal("2.5", Introduce.This(2.5f), false);

    [Fact]
    public void Introduce_Byte() =>
        Assert.Equal("255", Introduce.This((byte)255), false);

    [Fact]
    public void Introduce_SByte() =>
        Assert.Equal("-5", Introduce.This((sbyte)-5), false);

    [Fact]
    public void Introduce_Short() =>
        Assert.Equal("-1234", Introduce.This((short)-1234), false);

    [Fact]
    public void Introduce_UShort() =>
        Assert.Equal("1234", Introduce.This((ushort)1234), false);

    [Fact]
    public void Introduce_UInt() =>
        Assert.Equal("1234567890", Introduce.This(1234567890u), false);

    [Fact]
    public void Introduce_Long() =>
        Assert.Equal("9223372036854775807", Introduce.This(long.MaxValue), false);

    [Fact]
    public void Introduce_ULong() =>
        Assert.Equal("18446744073709551615", Introduce.This(ulong.MaxValue), false);

    [Fact]
    public void Introduce_DateTime() =>
        Assert.Equal("2023-01-01T12:00:00.0000000Z", Introduce.This(DateTime.Parse("2023-01-01T12:00:00Z").ToUniversalTime()), false);

    [Fact]
    public void Introduce_Guid() =>
        Assert.Equal("01234567-89ab-cdef-0123-456789abcdef", Introduce.This(Guid.Parse("01234567-89ab-cdef-0123-456789abcdef")), false);

    [Fact]
    public void Introduce_DateOnly() =>
        Assert.Equal("2023-07-16", Introduce.This(DateOnly.Parse("2023-07-16")), false);

    [Fact]
    public void Introduce_TimeOnly() =>
        Assert.Equal("13:45:30.0000000", Introduce.This(TimeOnly.Parse("13:45:30")), false);

    [Fact]
    public void Introduce_DayOfWeek() =>
        Assert.Equal("Monday", Introduce.This(DayOfWeek.Monday), false);
}
