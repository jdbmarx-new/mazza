using OrderManagement.Application.Abstractions;

namespace OrderManagement.Infrastructure;

public sealed class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}