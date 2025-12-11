using Lab4FoodDelivery.domain;

namespace Lab4FoodDelivery.pricing;

public class DeliveryFeeOverThresholdPolicy : IDeliveryFeePolicy
{
    /// <summary>
    /// Доставка бесплатна, если сумма товаров превышает threshold
    /// </summary>
    private readonly decimal _threshold;
    private readonly decimal _fee;
  
    public DeliveryFeeOverThresholdPolicy(decimal threshold, decimal fee)
    {
        if (threshold <= 0) 
            throw new ArgumentException("Threshold must be greater than zero", nameof(threshold));
        if (fee <= 0) 
            throw new ArgumentException("Delivery fee must be greater than zero", nameof(fee));
        _threshold = threshold;
        _fee = fee;
    }
    public decimal CalculateDeliveryFee(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        var totalSum = order.GetTotalSum();
        return totalSum >= _threshold ? 0 : _fee;
    }
}