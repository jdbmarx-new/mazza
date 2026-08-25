using OrderManagement.Application.Abstractions;

namespace OrderManagement.UnitTests;

internal sealed class FakeClock(DateTime now) : IClock
{
    public DateTime UtcNow => now;
}