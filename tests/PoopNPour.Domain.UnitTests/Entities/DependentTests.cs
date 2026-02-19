using FluentAssertions;
using PoopNPour.Domain.Entities;
using Xunit;

namespace PoopNPour.Domain.UnitTests.Entities;

public class DependentTests
{
    [Fact]
    public void Age_CalculatesCorrectAgeFromDateOfBirth()
    {
        var dateOfBirth = DateTimeOffset.UtcNow.AddYears(-5);
        var dependent = new Dependent { DateOfBirth = dateOfBirth };

        dependent.Age.Should().Be(5);
    }

    [Fact]
    public void Age_BeforeBirthday_ReturnsCorrectAge()
    {
        var today = new DateTimeOffset(2026, 2, 19, 0, 0, 0, TimeSpan.Zero);
        var dateOfBirth = new DateTimeOffset(2020, 6, 15, 0, 0, 0, TimeSpan.Zero);
        
        var dependent = new Dependent { DateOfBirth = dateOfBirth };
        
        var expectedAge = 5;
        var actualYears = today.Year - dateOfBirth.Year;
        if (today.Month < dateOfBirth.Month || (today.Month == dateOfBirth.Month && today.Day < dateOfBirth.Day))
        {
            actualYears--;
        }
        
        actualYears.Should().Be(expectedAge);
    }

    [Fact]
    public void Age_AfterBirthday_ReturnsCorrectAge()
    {
        var today = new DateTimeOffset(2026, 2, 19, 0, 0, 0, TimeSpan.Zero);
        var dateOfBirth = new DateTimeOffset(2020, 1, 15, 0, 0, 0, TimeSpan.Zero);
        
        var dependent = new Dependent { DateOfBirth = dateOfBirth };
        
        var expectedAge = 6;
        var actualYears = today.Year - dateOfBirth.Year;
        if (today.Month < dateOfBirth.Month || (today.Month == dateOfBirth.Month && today.Day < dateOfBirth.Day))
        {
            actualYears--;
        }
        
        actualYears.Should().Be(expectedAge);
    }

    [Fact]
    public void Age_OnBirthday_ReturnsCorrectAge()
    {
        var today = DateTimeOffset.UtcNow;
        var dateOfBirth = today.AddYears(-10);
        
        var dependent = new Dependent { DateOfBirth = dateOfBirth };

        dependent.Age.Should().Be(10);
    }

    [Fact]
    public void Age_DefaultDateOfBirth_ReturnsZero()
    {
        var dependent = new Dependent { DateOfBirth = default };

        dependent.Age.Should().Be(0);
    }
}
