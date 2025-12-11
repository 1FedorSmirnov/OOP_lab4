using Lab4FoodDelivery.domain;

namespace Lab4FoodDelivery.order.states;

public interface IOrderState
{
    string Name { get; }
    void StartPreparation(Order order);
    void StartDelivery(Order order);
    void Complete(Order order);
    void Cancel(Order order);

}