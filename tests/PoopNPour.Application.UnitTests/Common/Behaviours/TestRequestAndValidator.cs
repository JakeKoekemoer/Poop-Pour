using FluentValidation;
using MediatR;

namespace PoopNPour.Application.UnitTests.Common.Behaviours;

// Shared test request and validator for ValidationBehaviour testing
public record TestRequest(string Name, int Age) : IRequest<string>;

public class TestRequestValidator : AbstractValidator<TestRequest>
{
    public TestRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Age).GreaterThan(0);
    }
}
