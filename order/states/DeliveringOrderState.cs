using Lab4FoodDelivery.domain;

namespace Lab4FoodDelivery.order.states;

public class DeliveringOrderState : IOrderState
{
    public string Name { get; }
    public void StartPreparation(Order order)
    {
        throw new NotImplementedException();
    }

    public void StartDelivery(Order order)
    {
        throw new NotImplementedException();
    }

    public void Complete(Order order)
    {
        throw new NotImplementedException();
    }

    public void Cancel(Order order)
    {
        throw new NotImplementedException();
    }
}