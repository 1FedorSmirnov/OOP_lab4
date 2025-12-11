using Lab4FoodDelivery.domain;

namespace Lab4FoodDelivery.order.states;

public class NewOrderState : IOrderState
{
    public string Name => "New";
    public void StartPreparation(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        order.SetState(new PreparingOrderState());
    }

    public void StartDelivery(Order order)
    {
        throw new InvalidOperationException("Order is not prepared yet");
    }

    public void Complete(Order order)
    {
        throw new InvalidOperationException("Order is not prepared yet");
    }

    public void Cancel(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        order.SetState(new CancelledOrderState());
    }
}