using Lab4FoodDelivery.domain;

namespace Lab4FoodDelivery.pricing;

public class FixedDeliveryFeePolicy : IDeliveryFeePolicy
{
    private readonly decimal _fee;
    
    public FixedDeliveryFeePolicy(decimal fee)
    {
        if (fee <= 0) 
            throw new ArgumentException("Delivery fee must be greater than zero", nameof(fee));
        _fee = fee;
    }
    public decimal CalculateDeliveryFee(Order order)
    {
        return _fee;
    }
}