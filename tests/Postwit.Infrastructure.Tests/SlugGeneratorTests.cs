using Postwit.Infrastructure.Services;

namespace Postwit.Infrastructure.Tests;

public class SlugGeneratorTests
{
    private readonly SlugGenerator _sut = new();
    
    [Fact]
    public void GenerateSlug_InputIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _sut.GenerateSlug(null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void GenerateSlug_InputIsEmptyOrWhitespace_ReturnsEmptyString(string input)
    {
        var result = _sut.GenerateSlug(input);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GenerateSlug_InputWithoutReplaceableChars_ReturnsSameString()
    {
        const string input = "HelloWorld";
        var result = _sut.GenerateSlug(input);

        Assert.Equal("HelloWorld", result);
    }

    [Fact]
    public void GenerateSlug_InputWithSpaces_ReplacesWithDash()
    {
        const string input = "Hello World Example";
        var result = _sut.GenerateSlug(input);

        Assert.Equal("Hello-World-Example", result);
    }

    [Fact]
    public void GenerateSlug_InputWithSpecialChars_ReplacesWithDash()
    {
        const string input = "Hello,World!Example";
        var result = _sut.GenerateSlug(input);

        Assert.Equal("Hello-World-Example", result);
    }

    [Fact]
    public void GenerateSlug_InputStartsAndEndsWithSpecialChars_TrimsCorrectly()
    {
        const string input = "@Hello World!";
        var result = _sut.GenerateSlug(input);

        Assert.Equal("Hello-World", result);
    }

    [Fact]
    public void GenerateSlug_InputOnlySpecialChars_ReturnsEmptyString()
    {
        const string input = "!@#$%^&*()";
        var result = _sut.GenerateSlug(input);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GenerateSlug_InputWithMultipleSequentialSpecialChars_TreatsAsOneSeparator()
    {
        const string input = "Hello---World";
        var result = _sut.GenerateSlug(input);

        Assert.Equal("Hello-World", result);
    }

    [Fact]
    public void GenerateSlug_InputWithTabsAndNewLines_ReplacesWithDash()
    {
        const string input = "Hello\tWorld\nTest";
        var result = _sut.GenerateSlug(input);

        Assert.Equal("Hello-World-Test", result);
    }
}
