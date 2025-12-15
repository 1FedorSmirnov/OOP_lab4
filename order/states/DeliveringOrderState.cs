using Lab4FoodDelivery.domain;

namespace Lab4FoodDelivery.order.states;

public class DeliveringOrderState : IOrderState
{
    public string Name => "Delivering";
    
    public void StartPreparation(Order order)
    {
        throw new InvalidOperationException("Delivering order can't be return to preparing");
    }

    public void StartDelivery(Order order)
    {
        throw new InvalidOperationException("Order is already delivered");
    }

    public void Complete(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        order.SetState(new CompletedOrderState());
    }

    public void Cancel(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        order.SetState(new CancelledOrderState());
    }
}