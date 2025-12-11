using Lab4FoodDelivery.domain;

namespace Lab4FoodDelivery.pricing;

public class NoDiscountPolicy : IDiscountPolicy
{
    public decimal ApplyDiscount(decimal totalSum, Order order) => totalSum;
}