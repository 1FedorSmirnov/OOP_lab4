using Lab4FoodDelivery.domain;

namespace Lab4FoodDelivery.pricing;

public interface ITaxPolicy
{
    ///
    /// Применяет налоги к сумме после скидки
    ///
    decimal ApplyTax(decimal sumAfterDiscount, Order order);
}