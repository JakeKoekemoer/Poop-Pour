using FluentAssertions;
using FluentValidation;
using MediatR;
using PoopNPour.Application.Common.Behaviours;
using Xunit;
using ValidationException = PoopNPour.Application.Common.Exceptions.ValidationException;

namespace PoopNPour.Application.UnitTests.Common.Behaviours;

public class ValidationBehaviourPartiallyInvalidRequestThrowsForInvalidFields
{
    private static RequestHandlerDelegate<string> Next => (ct) => Task.FromResult("result");

    [Fact]
    public async Task Handle_PartiallyInvalidRequest_ThrowsForInvalidFields()
    {
        // Arrange
        var validators = new IValidator<TestRequest>[] { new TestRequestValidator() };
        var behaviour = new ValidationBehaviour<TestRequest, string>(validators);
        var request = new TestRequest("John", -1); // Name valid, Age invalid

        // Act
        var act = () => behaviour.Handle(request, Next, CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<ValidationException>();
        exception.Which.Errors.Should().ContainKey("Age");
        exception.Which.Errors.Should().NotContainKey("Name");
    }
}
