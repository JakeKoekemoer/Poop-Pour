using FluentAssertions;
using FluentValidation;
using MediatR;
using PoopNPour.Application.Common.Behaviours;
using Xunit;
using ValidationException = PoopNPour.Application.Common.Exceptions.ValidationException;

namespace PoopNPour.Application.UnitTests.Common.Behaviours;

public class ValidationBehaviourInvalidRequestThrowsValidationException
{
    private static RequestHandlerDelegate<string> Next => (ct) => Task.FromResult("result");

    [Fact]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Arrange
        var validators = new IValidator<TestRequest>[] { new TestRequestValidator() };
        var behaviour = new ValidationBehaviour<TestRequest, string>(validators);
        var request = new TestRequest("", -1);

        // Act
        var act = () => behaviour.Handle(request, Next, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}
