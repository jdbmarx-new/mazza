using OrderManagement.Application.Common;
using OrderManagement.Domain.Orders;
using Xunit;

namespace OrderManagement.UnitTests;

public sealed class HandlersTests
{
    [Fact]
    public async Task Given_ValidOrderData_When_CreatingOrder_Then_ShouldCreatePendingOrderWithCalculatedTotal()
    {
        // Arrange
        var repo = new FakeOrderRepository();
        var now = new DateTime(2026, 8, 24, 12, 0, 0, DateTimeKind.Utc);
        var sut = new Application.Orders.Create.CreateOrderHandler(repo, new FakeClock(now));

        // Act
        OrderDto result = await sut.Handle(new(Guid.NewGuid(), [new("Keyboard", 2, 150m)]), TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(OrderStatus.Pending, result.Status);
        Assert.Equal(300m, result.TotalAmount);
        Assert.Single(repo.Orders);
    }

    [Fact]
    public async Task Given_NonExistingOrder_When_GettingOrderById_Then_ShouldThrowNotFoundException()
    {
        // Arrange
        var sut = new Application.Orders.GetById.GetOrderByIdHandler(new FakeOrderRepository());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(new(Guid.NewGuid()), TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Given_ExistingOrders_When_GettingPagedOrders_Then_ShouldReturnTotalCount()
    {
        // Arrange
        var repo = new FakeOrderRepository();
        repo.Orders.Add(Order.Create(Guid.NewGuid(), [("A", 1, 10m)], DateTime.UtcNow));
        var sut = new Application.Orders.GetPage.GetOrdersHandler(repo);

        // Act
        PagedResult<OrderDto> result = await sut.Handle(new(1, 10), TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(1, result.TotalCount);
    }

    [Fact]
    public async Task Given_PendingOrder_When_CancellingOrder_Then_ShouldChangeStatusToCancelled()
    {
        // Arrange
        var repo = new FakeOrderRepository();
        var order = Order.Create(Guid.NewGuid(), [("A", 1, 10m)], DateTime.UtcNow);
        repo.Orders.Add(order);
        var sut = new Application.Orders.Cancel.CancelOrderHandler(repo);

        // Act
        OrderDto result = await sut.Handle(new(order.Id), TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(OrderStatus.Cancelled, result.Status);
    }
}