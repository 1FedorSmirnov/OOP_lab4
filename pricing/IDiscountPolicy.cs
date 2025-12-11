using Lab4FoodDelivery.domain;

namespace Lab4FoodDelivery.pricing;

public interface IDiscountPolicy
{
    ///
    ///Применяет скидку к базовой сумме
    ///
    
    decimal ApplyDiscount(decimal totalSum, Order order);
}