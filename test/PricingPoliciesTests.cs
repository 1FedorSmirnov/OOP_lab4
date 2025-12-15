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

    [Fact]
    public void TaxRatePolicy_ShouldAddTaxOnSum()
    {
        //Arrange
        var policy = new TaxRatePolicy(0.2m);
        var baseSum = 1000m;
        var expected = baseSum + baseSum * 0.20m;

        //Act
        var result = policy.ApplyTax(baseSum, null);

        //Assert
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void NoTaxPolicy_ShouldNotChangeSum()
    {
        //Arrange
        var policy = new NoTaxPolicy();
        var baseSum = 1000m;
  
        //Act
        var result = policy.ApplyTax(baseSum, null);

        //Assert
        Assert.Equal(baseSum, result);
    }
    
    [Fact]
    public void FixedDeliveryFeePolicy_ShouldReturnFixedFee()
    {
        //Arrange
        var policy = new FixedDeliveryFeePolicy(250m);

        //Act
        var fee = policy.CalculateDeliveryFee( null);

        //Assert
        Assert.Equal(250m, fee);
    }
    
    [Fact]
    public void DeliveryFeeOverThresholdPolicy_ShouldBeFree()
    {
        //Arrange
        const decimal threshold = 1000m;
        const decimal feeBelow = 250m;
        var policy = new DeliveryFeeOverThresholdPolicy(threshold, feeBelow);
        //Заказ с суммой, превышающей порог
        var order = CreateOrderWithCertainTotalSum(1200m);

        //Act
        var fee = policy.CalculateDeliveryFee(order);

        //Assert
        Assert.Equal(0m, fee);
    }
    
    [Fact]
    public void ExpressDeliveryFeePolicy_ShouldAddExtraFree()
    {
        //Arrange
        var basePolicy = new FixedDeliveryFeePolicy(200m);
        var policy = new ExpressDeliveryFeePolicy(150m, basePolicy);
        //Заказ с экспресс доставкой
        var order = CreateOrderWithCertainTotalSum(500m, true );
        var expectedFee = basePolicy.CalculateDeliveryFee(order) + 150m;
        //Act
        var fee = policy.CalculateDeliveryFee(order);

        //Assert
        Assert.Equal(expectedFee, fee);
    }
    
    [Fact]
    public void ExpressDeliveryFeePolicy_ShouldNotAddExtraFee_ForNonExpressOrder()
    {
        //Arrange
        var basePolicy = new FixedDeliveryFeePolicy(fee: 200m);
        var policy = new ExpressDeliveryFeePolicy(expressExtraFee: 150m, basePolicy);

        // Создаём заказ с isExpress = false
        var order = CreateOrderWithCertainTotalSum(500m, isExpress: false);
        //Act
        var fee = policy.CalculateDeliveryFee(order);
        //Assert
        Assert.Equal(200m, fee);
    }
}