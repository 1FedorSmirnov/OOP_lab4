using Lab4FoodDelivery.order;
using Xunit;

namespace Lab4FoodDelivery.test;

public class OrderServiceFacadeTests
{
    [Fact]
    public void OrderService_ShouldCreateStartedOrder_AndCalculateTotalSum()
    {
        //Arrange
        var menu = new TestMenu();
        var service = new OrderService(menu);

        var order = service.CreateStandardOrder(
            "Test Customer",
            "Test Address",
            "1234567890",
            [(menu.PizzaId, 1), (menu.BurgerId, 2)]
        );
        //Act
        var total = service.GetTotalSum(order.Id);
        
        //Assert
        Assert.NotEqual(Guid.Empty, order.Id);
        Assert.Equal("New", order.Status);
        Assert.Equal(3, order.Items.Sum(i => i.Quantity));
        Assert.True(total > 0m);
    }

    [Fact]
    public void OrderService_ShouldChangeOrderStatusThroughFacade()
    {
        var menu = new TestMenu();
        var service = new OrderService(menu);

        var order = service.CreateExpressOrder(
            "Test Customer",
            "Test Address",
            "1234567890",
            [(menu.PizzaId, 1)]);
        
        Assert.Equal("New", service.GetOrderStatus(order.Id));
        
        service.StartOrderPreparation(order.Id);
        Assert.Equal("Preparing", service.GetOrderStatus(order.Id));
        
        service.StartOrderDelivery(order.Id);
        Assert.Equal("Delivering", service.GetOrderStatus(order.Id));
        
        service.CompleteOrder(order.Id);
        Assert.Equal("Completed", service.GetOrderStatus(order.Id));
    }
}