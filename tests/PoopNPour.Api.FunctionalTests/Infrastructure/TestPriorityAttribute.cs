namespace PoopNPour.Api.FunctionalTests.Infrastructure;

/// <summary>
/// Attribute to specify the execution priority of a test method.
/// Lower numbers run first. Used with <see cref="PriorityOrderer"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class TestPriorityAttribute : Attribute
{
    public int Priority { get; }

    public TestPriorityAttribute(int priority)
    {
        Priority = priority;
    }
}
