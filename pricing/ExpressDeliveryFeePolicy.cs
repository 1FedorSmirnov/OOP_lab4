using Lab4FoodDelivery.domain;

namespace Lab4FoodDelivery.pricing;

/// <summary>
/// Надбавка за экспресс-доставку
/// Комбинируется с базовой политикой доставки
/// </summary>

public class ExpressDeliveryFeePolicy : IDeliveryFeePolicy
{
    private readonly decimal _expressExtraFee;
    private readonly IDeliveryFeePolicy _basePolicy;
    public ExpressDeliveryFeePolicy(decimal expressExtraFee, IDeliveryFeePolicy basePolicy)
    {
        _basePolicy = basePolicy ?? throw new ArgumentNullException(nameof(basePolicy));
        if (expressExtraFee <= 0) 
            throw new ArgumentException("Express extra fee must be greater than zero", nameof(expressExtraFee));
        _expressExtraFee = expressExtraFee;
        
    }
    public decimal CalculateDeliveryFee(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        var baseFee = _basePolicy.CalculateDeliveryFee(order);
        if (order.IsExpress)
        {
            return baseFee + _expressExtraFee;
        }
        return baseFee;
    }
}