using Lab4FoodDelivery.domain;
using Lab4FoodDelivery.pricing;

namespace Lab4FoodDelivery.order;

public class OrderFactory
{
     public static OrderBuilder Create(OrderType orderType, IMenu menu)
        {
        ArgumentNullException.ThrowIfNull(menu);

        var builder = new OrderBuilder(menu);

            // Базовые политики по умолчанию:
            IDiscountPolicy discountPolicy = new NoDiscountPolicy();
            ITaxPolicy taxPolicy = new TaxRatePolicy(0.10m);                 // 10% НДС
            IDeliveryFeePolicy deliveryFeePolicy = new FixedDeliveryFeePolicy(200m); // обычная доставка 200

            switch (orderType)
            {
                case OrderType.Standard:
                {
                    // Стандарт: без скидок, базовый налог, базовая доставка
                    var strategy = new CompositePricingStrategy(discountPolicy, taxPolicy, deliveryFeePolicy);
                    return builder.WithPricingStrategy(strategy);
                }

                case OrderType.Express:
                {
                    // Экспресс: тот же налог, но надбавка за скорость
                    var expressFeePolicy = new ExpressDeliveryFeePolicy(
                        expressExtraFee: 150m, deliveryFeePolicy); // +150 за скорость

                    var strategy = new CompositePricingStrategy(discountPolicy, taxPolicy, expressFeePolicy);

                    return builder
                        .WithPricingStrategy(strategy)
                        .WithExpressDelivery();
                }

                case OrderType.Custom:
                {
                    // Пример: кастомный заказ с фиксированной скидкой 100
                    discountPolicy = new FixedSumDiscountPolicy(100m);

                    var strategy = new CompositePricingStrategy(discountPolicy, taxPolicy, deliveryFeePolicy);
                    return builder.WithPricingStrategy(strategy);
                }

                default:
                    throw new ArgumentOutOfRangeException(nameof(orderType), orderType, "Unknown order type.");
            }
        }
}