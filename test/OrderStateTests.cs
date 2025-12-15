using Lab4FoodDelivery.domain;
using Lab4FoodDelivery.pricing;
using Xunit;

namespace Lab4FoodDelivery.test;

public class OrderStateTests
{
    private Order CreateSimpleOrder()
    {
        var menu = new TestMenu();
        var items = new List<OrderItem>()
        {
            new OrderItem(menu.GetItemById(menu.PizzaId), 1)
        };

        var strategy = new CompositePricingStrategy(new NoDiscountPolicy(),
            new TaxRatePolicy(0.18m), 
            new FixedDeliveryFeePolicy(0));
        var order = new Order(
            id: Guid.NewGuid(),
            customerName: "Petr",
            deliveryAddress: "Lenina 10",
            phoneNumber: "1234567890",
            items: items,
            pricingStrategy: strategy,
            isExpress: false);

        return order;
    }

    [Fact]
    public void NewOrder_CanBePrepared_AndDelivered_AndCompleted()
    {
        var order = CreateSimpleOrder();

        Assert.Equal("New", order.Status);

        order.StartPreparation();
        Assert.Equal("Preparing", order.Status);

        order.StartDelivery();
        Assert.Equal("Delivering", order.Status);

        order.Complete();
        Assert.Equal("Completed", order.Status);
    }

    [Fact]
    public void CompletedOrder_CannotBeCancelled()
    {
        var order = CreateSimpleOrder();
        order.StartPreparation();
        order.StartDelivery();
        order.Complete();

        Assert.Equal("Completed", order.Status);

        Assert.Throws<InvalidOperationException>(() => order.Cancel());
    }

    [Fact]
    public void NewOrder_CanBeCancelled()
    {
        var order = CreateSimpleOrder();

        order.Cancel();

        Assert.Equal("Cancelled", order.Status);
    }
}