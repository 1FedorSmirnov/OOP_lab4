using Lab4FoodDelivery.domain;

namespace Lab4FoodDelivery.pricing;

public interface IDeliveryFeePolicy
{
    /// <summary>
    /// Возвращает стоимость доставки для заказа.
    /// </summary>
    decimal CalculateDeliveryFee(Order order);
}