using FluentAssertions;
using FluentValidation;
using MediatR;
using PoopNPour.Application.Common.Behaviours;
using Xunit;

namespace PoopNPour.Application.UnitTests.Common.Behaviours;

public class ValidationBehaviourValidRequestProceeds
{
    private static RequestHandlerDelegate<string> Next => (ct) => Task.FromResult("result");

    [Fact]
    public async Task Handle_ValidRequest_Proceeds()
    {
        // Arrange
        var validators = new IValidator<TestRequest>[] { new TestRequestValidator() };
        var behaviour = new ValidationBehaviour<TestRequest, string>(validators);
        var request = new TestRequest("John", 25);

        // Act
        var result = await behaviour.Handle(request, Next, CancellationToken.None);

        // Assert
        result.Should().Be("result");
    }
}
