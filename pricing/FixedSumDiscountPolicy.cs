using Lab4FoodDelivery.domain;

namespace Lab4FoodDelivery.pricing;

public class FixedSumDiscountPolicy : IDiscountPolicy
{
    private readonly decimal _discountSum;
    
    public FixedSumDiscountPolicy(decimal discountSum)
    {
        if (discountSum <= 0) 
            throw new ArgumentException("Discount sum must be greater than zero", nameof(discountSum));
        _discountSum = discountSum;
    }
    public decimal ApplyDiscount(decimal totalSum, Order order)
    {
        var result = totalSum - _discountSum;
        return result > 0 ? result : 0;
    }
}