using Lab4FoodDelivery.domain;

namespace Lab4FoodDelivery.order.states;

public class CompletedOrderState : IOrderState
{
    public string Name => "Completed";
    
    public void StartPreparation(Order order)
    {
        throw new InvalidOperationException("Completed order can't be changed");
    }

    public void StartDelivery(Order order)
    {
        throw new InvalidOperationException("Completed order can't be changed");
    }

    public void Complete(Order order)
    {
        throw new InvalidOperationException("Order is already completed");
    }

    public void Cancel(Order order)
    {
        throw new InvalidOperationException("Completed order can't be cancelled");
    }
}