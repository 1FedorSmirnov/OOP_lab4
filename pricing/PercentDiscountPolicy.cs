using Lab4FoodDelivery.domain;

namespace Lab4FoodDelivery.pricing;

public class PercentDiscountPolicy : IDiscountPolicy
{
    private readonly decimal _discountPercent;
    
    public PercentDiscountPolicy(decimal discountPercent)
    {
        if (discountPercent is <= 0 or > 1) 
            throw new ArgumentException("Discount percent must be greater than zero and less than 100", nameof(discountPercent));
        
        _discountPercent = discountPercent;
    }
    public decimal ApplyDiscount(decimal totalSum, Order order)
    {
        var discount = totalSum * _discountPercent;
        var result = totalSum - discount;
        return result > 0 ? result : 0;
    }
}