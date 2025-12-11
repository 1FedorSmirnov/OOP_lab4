using Lab4FoodDelivery.domain;

namespace Lab4FoodDelivery.pricing;

public interface IPricingStrategy
{
    decimal CalculateTotal(Order order);
}