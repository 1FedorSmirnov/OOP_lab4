using Lab4FoodDelivery.domain;

namespace Lab4FoodDelivery.order.states;

public class CancelledOrderState : IOrderState
{
    public string Name => "Cancelled";
    
    public void StartPreparation(Order order)
    {
        throw new InvalidOperationException("Cancelled order can't be changed");
    }

    public void StartDelivery(Order order)
    {
        throw new InvalidOperationException("Cancelled order can't be changed");
    }

    public void Complete(Order order)
    {
        throw new InvalidOperationException("Cancelled order can't be completed");
    }

    public void Cancel(Order order)
    {
        throw new InvalidOperationException("Order is already cancelled");
    }
}