using Lab4FoodDelivery.domain;
using Lab4FoodDelivery.pricing;
using Xunit;

namespace Lab4FoodDelivery.test;

public class CompositePricingStrategyTest
{
    private Order CreateOrderWithItems(decimal itemPrice, int quantity, bool isExpress, IPricingStrategy strategy)
    {
        var item = new MenuItem(Guid.NewGuid(), "Test item", itemPrice);
        var orderItem = new OrderItem(item, quantity);

        var order = new Order(
            id: Guid.NewGuid(),
            customerName: "Test",
            deliveryAddress: "Test address",
            phoneNumber: "1234567890",
            items: [orderItem],
            pricingStrategy: strategy,
            isExpress: isExpress);

        return order;
    }
    
    [Fact]
    public void CompositePricingStrategy_StandardScenario()
    {
        // Базовый сценарий:
        // - без скидки
        // - налог 10%
        // - доставка 200

        var discountPolicy = new NoDiscountPolicy();
        var taxPolicy = new TaxRatePolicy(0.10m); // 10%
        var deliveryPolicy = new FixedDeliveryFeePolicy(200m);

        var strategy = new CompositePricingStrategy(discountPolicy, taxPolicy, deliveryPolicy);

        // Товар: 1000 * 1 = 1000
        var order = CreateOrderWithItems(itemPrice: 1000m, quantity: 1, isExpress: false, strategy: strategy);

        var total = order.CalculateTotalPrice();

        const decimal itemsTotal = 1000m;
        const decimal afterDiscount = itemsTotal;
        const decimal afterTax = afterDiscount + afterDiscount * 0.10m; // 1100
        const decimal expectedTotal = afterTax + 200m;                  // 1300

        Assert.Equal(expectedTotal, total);
    }

    [Fact]
    public void CompositePricingStrategy_WithPercentageDiscount_AndFreeDeliveryOverThreshold()
    {
        // Сценарий:
        // - скидка 10% на товары
        // - налог 10%
        // - доставка бесплатна, если сумма товаров >= 1500, иначе 200
        // Берём сумму товаров 2000, чтобы доставка стала бесплатной.

        var discountPolicy = new PercentDiscountPolicy(0.10m); // 10%
        var taxPolicy = new TaxRatePolicy(0.10m);               // 10%
        var deliveryPolicy = new DeliveryFeeOverThresholdPolicy(
            threshold: 1500m,
            fee: 200m);

        var strategy = new CompositePricingStrategy(discountPolicy, taxPolicy, deliveryPolicy);

        // Товар: 1000 * 2 = 2000
        var order = CreateOrderWithItems(itemPrice: 1000m, quantity: 2, isExpress: false, strategy: strategy);

        var total = order.CalculateTotalPrice();

        const decimal itemsTotal = 2000m;
        const decimal afterDiscount = itemsTotal - itemsTotal * 0.10m; // 1800
        const decimal afterTax = afterDiscount + afterDiscount * 0.10m; // 1980
        const decimal deliveryFee = 0m;                                 // free delivery
        const decimal expectedTotal = afterTax + deliveryFee;

        Assert.Equal(expectedTotal, total);
    }
    
    [Fact]
    public void CompositePricingStrategy_WithFixedDiscount_ExpressDeliverySurcharge()
    {
        // Сценарий:
        // - фиксированная скидка 300
        // - налог 10%
        // - базовая доставка 200
        // - экспресс-наценка +150, если заказ express

        var discountPolicy = new FixedSumDiscountPolicy(300m);
        var taxPolicy = new TaxRatePolicy(0.10m);
        var baseDeliveryPolicy = new FixedDeliveryFeePolicy(200m);
        var expressDeliveryPolicy = new ExpressDeliveryFeePolicy(
            expressExtraFee: 150m,
            baseDeliveryPolicy);

        var strategy = new CompositePricingStrategy(discountPolicy, taxPolicy, expressDeliveryPolicy);

        // Товар: 800 * 2 = 1600
        var order = CreateOrderWithItems(itemPrice: 800m, quantity: 2, isExpress: true, strategy: strategy);

        var total = order.CalculateTotalPrice();

        const decimal itemsTotal = 1600m;
        const decimal afterDiscount = itemsTotal - 300m;          // 1300
        const decimal afterTax = afterDiscount + afterDiscount * 0.10m; // 1430
        const decimal deliveryFee = 200m + 150m;                  // 350
        const decimal expectedTotal = afterTax + deliveryFee;     // 1780

        Assert.Equal(expectedTotal, total);
    }
    
    [Fact]
    public void CompositePricingStrategy_NeverReturnsNegativeTotal_WhenDiscountTooHigh()
    {
        // Сценарий:
        // - скидка сильно больше суммы товаров → не должно уйти в минус
        // - нет налогов
        // - доставки нет

        var discountPolicy = new FixedSumDiscountPolicy(1000m);
        var taxPolicy = new NoTaxPolicy();
        var deliveryPolicy = new FixedDeliveryFeePolicy(0m);

        var strategy = new CompositePricingStrategy(discountPolicy, taxPolicy, deliveryPolicy);

        // Товар: 300 * 1 = 300
        var order = CreateOrderWithItems(itemPrice: 300m, quantity: 1, isExpress: false, strategy: strategy);

        var total = order.CalculateTotalPrice();

        Assert.True(total >= 0m);
    }

}