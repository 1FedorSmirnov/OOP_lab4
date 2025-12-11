using Lab4FoodDelivery.domain;
using Lab4FoodDelivery.pricing;
using Xunit;

namespace Lab4FoodDelivery.test;

public class PricingPoliciesTests
{
    private Order CreateOrderWithCertainTotalSum(decimal totalSum, bool isExpress = false)
    {
        // Для тестов FreeDeliveryOverThreshold / ExpressSurcharge нужен Order,
        // у которого GetTotalSum() возвращает нужную сумму.
        var item = new MenuItem(Guid.NewGuid(), "Item with certain price", totalSum);
        var orderItem = new OrderItem(item, 1);

        var discount = new NoDiscountPolicy();
        var tax = new NoTaxPolicy();
        var delivery = new FixedDeliveryFeePolicy(0);
        var strategy = new CompositePricingStrategy(discount, tax, delivery);

        var order = new Order(Guid.NewGuid(),
            "Test customer",
            "Test address",
            "1234567890",
            [orderItem],
            isExpress,
            strategy);
        return order;
    }
    
    [Fact]
    public void NoDiscountPolicy_ShouldReturnSameSum()
    {
        //Arrange
        var policy = new NoDiscountPolicy();
        const int totalSum = 100;
        
        //Act
        var result = policy.ApplyDiscount(totalSum, null);
       
        //Assert
        Assert.Equal(totalSum, result);
    }

    [Fact]
    public void FixedSumDiscountPolicy_ShouldSubstructDiscount_NotBelowZero()
    {
        //Arrange
        var policy = new FixedSumDiscountPolicy(300);
        const int totalSum = 200;
        
        //Act
        var result = policy.ApplyDiscount(totalSum, null);
        
        //Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void PercentDiscountPolicy_ShouldApplyCorrectDiscount()
    {
        //Arrange
        var policy = new PercentDiscountPolicy(0.1m);
        const int totalSum = 1000;
        var expected = 1000m - 1000m * 0.10m; 
        
        //Act
        var result = policy.ApplyDiscount(totalSum, null);
        
        //Assert
        Assert.Equal(expected, result);
    }
}