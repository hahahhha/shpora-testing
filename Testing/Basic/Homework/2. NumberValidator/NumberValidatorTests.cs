using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    // тесты конструктора

    [TestCase(-1)]
    [TestCase(0)]
    public void Constructor_NotPositivePrecision_ThrowsArgumentException(int precision)
    {
        var action = () => new NumberValidator(precision, 0);

        action.Should().Throw<ArgumentException>();
    }

    [TestCase(-2)]
    [TestCase(-1)]
    public void Constructor_NegativeScale_ThrowsArgumentException(int scale)
    {
        var action = () => new NumberValidator(1, scale);

        action.Should().Throw<ArgumentException>();
    }

    [TestCase(1, 0)] // граничный precision
    [TestCase(10, 1)] // граничный scale
    [TestCase(10, 9)] // граничные значения scale при указанном precision (precision должен быть больше scale)
    public void Constructor_CorrectArguments_ShouldCreateInstance(
        int precision, int scale)
    {
        var action = () => new NumberValidator(precision, scale);

        action.Should().NotThrow();
    }

    [TestCase(5, 10)]
    [TestCase(5, 5)]
    public void Constructor_ScaleGreaterOrEqualsPrecision_ThrowsArgumentException(
        int precision, int scale)
    {
        var action = () => new NumberValidator(precision, scale);
        action.Should().Throw<ArgumentException>();
    }


    // тесты работы метода IsValidNumber

    [TestCase("a.sd")]
    [TestCase("aaa")]
    [TestCase("1/2")]
    [TestCase("5.")]
    [TestCase(".1")]
    public void IsValidNumber_InvalidFormat_ReturnsFalse(string testValue)
    {
        var numberValidator = new NumberValidator(10, 5);
        var actual = numberValidator.IsValidNumber(testValue);
        actual.Should().BeFalse();
    }
    
    [Test]
    public void IsValidNumber_NegativeNumberWhenOnlyPositive_ReturnsFalse()
    {
        var testValue = "-12.34";
        var numberValidator = new NumberValidator(5, 2, true);
        var actual = numberValidator.IsValidNumber(testValue);
        actual.Should().BeFalse();
    }

    [Test]
    public void IsValidNumber_ZeroWhenOnlyPositive_ReturnsTrue()
    {
        var numberValidator = new NumberValidator(1, 0, true);
        var actual = numberValidator.IsValidNumber("0");
        actual.Should().BeTrue();
    }

    [Test]
    public void IsValidNumber_ZeroWithFractionalPart_ReturnsTrue()
    {
        var numberValidator = new NumberValidator(17, 2, true);
        var actual = numberValidator.IsValidNumber("0.0");
        actual.Should().BeTrue();
    }

    [Test]
    public void IsValidNumber_IntegerWithZeroScale_ReturnsTrue()
    {
        var testValue = "123";
        var numberValidator = new NumberValidator(testValue.Length, 0);
        var actual = numberValidator.IsValidNumber(testValue);
        actual.Should().BeTrue();
    }

    [TestCase("123.45")]
    [TestCase("12.3")]
    public void IsValidNumber_FractionalNumberWithZeroScale_ReturnsFalse(string testValue)
    {
        var numberValidator = new NumberValidator(testValue.Length, 0);
        var actual = numberValidator.IsValidNumber(testValue);
        actual.Should().BeFalse();
    }

    [TestCase(3, 2, "00.00")]
    [TestCase(3, 2, "-0.00")]
    [TestCase(3, 2, "+0.00")]
    [TestCase(3, 2, "-1.23")]
    [TestCase(3, 2, "+1.23")]
    public void IsValidNumber_ExceedingPrecision_ReturnsFalse(
        int precision, int scale, string testValue)
    {
        var numberValidator = new NumberValidator(precision, scale);
        var actual = numberValidator.IsValidNumber(testValue);
        actual.Should().BeFalse();
    }

    [Test]
    public void IsValidNumber_ExceedingScale_ReturnsFalse()
    {
        var testValue = "1.23";
        var numberValidator = new NumberValidator(4, 1);
        var actual = numberValidator.IsValidNumber(testValue);
        actual.Should().BeFalse();
    }

    [Test]
    public void IsValidNumber_WithLeadingPlus_ReturnsTrue()
    {
        var numberValidator = new NumberValidator(4, 2, true);
        var actual = numberValidator.IsValidNumber("+1.23");
        actual.Should().BeTrue();
    }

    [TestCase("")]
    [TestCase("   ")]
    public void IsValidNumber_EmptyOrWhitespaceString_ReturnsFalse(string testValue)
    {
        var numberValidator = new NumberValidator(1);
        var actual = numberValidator.IsValidNumber(testValue);
        actual.Should().BeFalse();
    }

    [Test]
    public void IsValidNumber_NullString_ReturnsFalse()
    {
        var numberValidator = new NumberValidator(1, 0);
        var actual = numberValidator.IsValidNumber(null);
        actual.Should().BeFalse();
    }

    [TestCase("123,45")]
    [TestCase("0,45")]
    [TestCase("-10,45")]
    public void IsValidNumber_CommaDelimiter_ReturnsTrue(string testValue)
    {
        var numberValidator = new NumberValidator(10, 5);
        var actual = numberValidator.IsValidNumber(testValue);
        actual.Should().BeTrue();
    }
}