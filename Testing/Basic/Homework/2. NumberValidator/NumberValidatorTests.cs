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

    [TestCase(5, 10, true)]
    [TestCase(5, 10, false)]
    [TestCase(5, 5, true)]
    [TestCase(5, 5, false)]
    public void Constructor_ScaleGreaterOrEqualsPrecision_ThrowsArgumentException(
        int precision, int scale, bool onlyPositive)
    {
        var action = () => 
        {
            _ = new NumberValidator(precision, scale, onlyPositive);
        };

        action.Should().Throw<ArgumentException>();
    }


    // тесты работы метода IsValidNumber

    [TestCase(3, 2, true, "a.sd")]
    [TestCase(3, 2, true, "aaa")]
    [TestCase(3, 2, true, "1/2")]
    [TestCase(3, 2, true, "5.")]
    [TestCase(3, 2, true, ".1")]
    public void IsValidNumber_InvalidFormat_ReturnsFalse(
        int precision, int scale, bool onlyPositive, string testValue)
    {
        var numberValidator = new NumberValidator(precision, scale, onlyPositive);
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

    [TestCase(10, 0, true, "100")]
    [TestCase(10, 0, true, "4321")]
    public void IsValidNumber_IntegerWithZeroScale_ReturnsTrue(
        int precision, int scale, bool onlyPositive, string testValue)
    {
        var numberValidator = new NumberValidator(precision, scale, onlyPositive);
        var actual = numberValidator.IsValidNumber(testValue);
        actual.Should().BeTrue();
    }

    [TestCase(10, 0, false, "123.45")]
    [TestCase(10, 0, false, "0.45")]
    [TestCase(10, 0, false, "-10.45")]
    public void IsValidNumber_FractionalNumberWithZeroScale_ReturnsFalse(
        int precision, int scale, bool onlyPositive, string testValue)
    {
        var numberValidator = new NumberValidator(precision, scale, onlyPositive);
        var actual = numberValidator.IsValidNumber(testValue);
        actual.Should().BeFalse();
    }

    [TestCase(3, 2, false, "00.00")]
    [TestCase(3, 2, false, "-0.00")]
    [TestCase(3, 2, false, "+0.00")]
    [TestCase(3, 2, false, "-1.23")]
    [TestCase(3, 2, false, "+1.23")]
    public void IsValidNumber_ExceedingPrecision_ReturnsFalse(
        int precision, int scale, bool onlyPositive, string testValue)
    {
        var numberValidator = new NumberValidator(precision, scale, onlyPositive);
        var actual = numberValidator.IsValidNumber(testValue);
        actual.Should().BeFalse();
    }

    [TestCase(17, 2, true, "0.000")]
    public void IsValidNumber_ExceedingScale_ReturnsFalse(
        int precision, int scale, bool onlyPositive, string testValue)
    {
        var numberValidator = new NumberValidator(precision, scale, onlyPositive);
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

    [TestCase(10, 8, true, "")]
    [TestCase(10, 8, false, "")]
    [TestCase(10, 8, true, "     ")]
    [TestCase(10, 8, false, "     ")]
    public void IsValidNumber_EmptyOrWhitespaceString_ReturnsFalse(
        int precision, int scale, bool onlyPositive, string testValue)
    {
        var numberValidator = new NumberValidator(precision, scale, onlyPositive);
        var actual = numberValidator.IsValidNumber(testValue);
        actual.Should().BeFalse();
    }

    [TestCase(10, 8, true)]
    [TestCase(10, 8, false)]
    public void IsValidNumber_NullString_ReturnsFalse(
        int precision, int scale, bool onlyPositive)
    {
        var numberValidator = new NumberValidator(precision, scale, onlyPositive);
        var actual = numberValidator.IsValidNumber(null);
        actual.Should().BeFalse();
    }

    [TestCase(10, 5, false, "123,45")]
    [TestCase(10, 5, false, "0,45")]
    [TestCase(10, 5, false, "-10,45")]
    public void IsValidNumber_CommaDelimiter_ReturnsTrue(
        int precision, int scale, bool onlyPositive, string testValue)
    {
        var numberValidator = new NumberValidator(precision, scale, onlyPositive);
        var actual = numberValidator.IsValidNumber(testValue);
        actual.Should().BeTrue();
    }
}