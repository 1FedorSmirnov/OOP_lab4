using Lab4FoodDelivery.domain;

namespace Lab4FoodDelivery.pricing;

/// <summary>
/// Общая стратегия расчёта:
/// 1) суммирует товары;
/// 2) применяет скидку;
/// 3) применяет налоги;
/// 4) добавляет стоимость доставки.
/// </summary>
public class CompositePricingStrategy : IPricingStrategy
{
    private readonly IDiscountPolicy _discountPolicy;
    private readonly ITaxPolicy _taxPolicy;
    private readonly IDeliveryFeePolicy _deliveryFeePolicy;

    public CompositePricingStrategy(IDiscountPolicy discountPolicy,
        ITaxPolicy taxPolicy,
        IDeliveryFeePolicy deliveryFeePolicy)
    {
        _discountPolicy = discountPolicy ?? throw new ArgumentNullException(nameof(discountPolicy));
        _taxPolicy = taxPolicy ?? throw new ArgumentNullException(nameof(taxPolicy));
        _deliveryFeePolicy = deliveryFeePolicy ?? throw new ArgumentNullException(nameof(deliveryFeePolicy));
    }
    public decimal CalculateTotal(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        var totalSum = order.GetTotalSum();
        var afterDiscount = _discountPolicy.ApplyDiscount(totalSum, order);
        var afterTax = _taxPolicy.ApplyTax(afterDiscount, order);
        var deliveryFee = _deliveryFeePolicy.CalculateDeliveryFee(order);
        return afterTax + deliveryFee;
        
    }
}