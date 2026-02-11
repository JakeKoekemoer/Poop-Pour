using FluentAssertions;
using PoopNPour.Domain.Common.Identity;
using Xunit;

namespace PoopNPour.Domain.UnitTests.Common;

public class ApplicationUserTests
{
    [Fact]
    public void ApplicationUser_HasExpectedProperties()
    {
        var user = new ApplicationUser
        {
            Id = "user-1",
            UserName = "testuser",
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            CreatedAt = new DateTime(2025, 1, 1),
            UpdatedAt = new DateTime(2025, 1, 2)
        };

        user.Id.Should().Be("user-1");
        user.UserName.Should().Be("testuser");
        user.Email.Should().Be("test@example.com");
        user.FirstName.Should().Be("Test");
        user.LastName.Should().Be("User");
        user.CreatedAt.Should().Be(new DateTime(2025, 1, 1));
        user.UpdatedAt.Should().Be(new DateTime(2025, 1, 2));
    }
}
