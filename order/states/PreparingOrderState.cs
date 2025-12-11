using Lab4FoodDelivery.domain;

namespace Lab4FoodDelivery.order.states;

public class PreparingOrderState : IOrderState
{
    public string Name => "Preparing";
    public void StartPreparation(Order order)
    {
        throw new InvalidOperationException("Order is already preparing");
    }

    public void StartDelivery(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        order.SetState(new DeliveringOrderState());
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