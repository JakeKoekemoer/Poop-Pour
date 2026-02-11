using Xunit.Abstractions;
using Xunit.Sdk;

namespace PoopNPour.Api.FunctionalTests.Infrastructure;

/// <summary>
/// xUnit test case orderer that orders tests by their <see cref="TestPriorityAttribute"/> priority.
/// Tests without the attribute run last (priority = int.MaxValue).
/// </summary>
public class PriorityOrderer : ITestCaseOrderer
{
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(
        IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
    {
        return testCases.OrderBy(tc =>
        {
            var priorityAttribute = tc.TestMethod.Method
                .GetCustomAttributes(typeof(TestPriorityAttribute).AssemblyQualifiedName!)
                .FirstOrDefault();

            return priorityAttribute?.GetNamedArgument<int>(nameof(TestPriorityAttribute.Priority))
                   ?? int.MaxValue;
        });
    }
}
