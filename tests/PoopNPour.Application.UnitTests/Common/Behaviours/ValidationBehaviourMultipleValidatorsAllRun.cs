using FluentAssertions;
using FluentValidation;
using MediatR;
using PoopNPour.Application.Common.Behaviours;
using Xunit;
using ValidationException = PoopNPour.Application.Common.Exceptions.ValidationException;

namespace PoopNPour.Application.UnitTests.Common.Behaviours;

public class ValidationBehaviourMultipleValidatorsAllRun
{
    private static RequestHandlerDelegate<string> Next => (ct) => Task.FromResult("result");

    [Fact]
    public async Task Handle_MultipleValidators_AllRun()
    {
        // Arrange
        var validator1 = new TestRequestValidator();
        var validator2 = new TestRequestValidator(); // Same validator twice for testing
        var validators = new IValidator<TestRequest>[] { validator1, validator2 };
        var behaviour = new ValidationBehaviour<TestRequest, string>(validators);
        var request = new TestRequest("", -1);

        // Act
        var act = () => behaviour.Handle(request, Next, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}
