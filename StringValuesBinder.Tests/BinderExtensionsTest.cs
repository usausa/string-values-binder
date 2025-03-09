namespace StringValuesBinder.Tests;

using Microsoft.Extensions.Primitives;

public sealed class BinderExtensionsTest
{
    // String

    [Fact]
    public void TestStringProperty()
    {
        var dictionary = new Dictionary<string, StringValues>
        {
            ["Value"] = "test"
        };

        var target = dictionary.Bind<StringPropertyTarget>();

        Assert.Equal("test", target.Value);
    }

    private sealed class StringPropertyTarget
    {
        public string? Value { get; set; }
    }

    // Convert

    [Fact]
    public void TestConvertProperty()
    {
        var dictionary = new Dictionary<string, StringValues>
        {
            ["Value"] = "123"
        };

        var target = dictionary.Bind<ConvertPropertyTarget>();

        Assert.Equal(123, target.Value);
    }

    private sealed class ConvertPropertyTarget
    {
        public int Value { get; set; }
    }

    // Convert nullable

    [Fact]
    public void TestConvertNullableProperty()
    {
        var dictionary = new Dictionary<string, StringValues>
        {
            ["Value"] = "123"
        };

        var target = dictionary.Bind<ConvertNullablePropertyTarget>();

        Assert.Equal(123, target.Value);
    }

    private sealed class ConvertNullablePropertyTarget
    {
        public int? Value { get; set; }
    }

    // String array

    [Fact]
    public void TestStringArrayProperty()
    {
        var dictionary = new Dictionary<string, StringValues>
        {
            ["Value"] = new(["abc", "cde"])
        };

        var target = dictionary.Bind<StringArrayPropertyTarget>();

        Assert.NotNull(target.Value);
        Assert.Equal(2, target.Value.Length);
        Assert.Equal("abc", target.Value[0]);
        Assert.Equal("cde", target.Value[1]);
    }

#pragma warning disable CA1819
    private sealed class StringArrayPropertyTarget
    {
        public string?[]? Value { get; set; }
    }
#pragma warning restore CA1819

    // Convert array

    [Fact]
    public void TestConvertArrayProperty()
    {
        var dictionary = new Dictionary<string, StringValues>
        {
            ["Value"] = new(["123", "456"])
        };

        var target = dictionary.Bind<ConvertArrayPropertyTarget>();

        Assert.NotNull(target.Value);
        Assert.Equal(2, target.Value.Length);
        Assert.Equal(123, target.Value[0]);
        Assert.Equal(456, target.Value[1]);
    }

#pragma warning disable CA1819
    private sealed class ConvertArrayPropertyTarget
    {
        public int[]? Value { get; set; }
    }
#pragma warning restore CA1819

    // Convert nullable array

    [Fact]
    public void TestConvertNullableArrayProperty()
    {
        var dictionary = new Dictionary<string, StringValues>
        {
            ["Value"] = new(["123", "456"])
        };

        var target = dictionary.Bind<ConvertNullableArrayPropertyTarget>();

        Assert.NotNull(target.Value);
        Assert.Equal(2, target.Value.Length);
        Assert.Equal(123, target.Value[0]);
        Assert.Equal(456, target.Value[1]);
    }

#pragma warning disable CA1819
    private sealed class ConvertNullableArrayPropertyTarget
    {
        public int?[]? Value { get; set; }
    }
#pragma warning restore CA1819
}
