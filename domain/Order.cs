using Lab4FoodDelivery.order.states;
using Lab4FoodDelivery.pricing;

namespace Lab4FoodDelivery.domain;

public class Order
{
    private readonly List<OrderItem> _items = [];
    
    public Guid Id { get;  }
    public string CustomerName { get;  }
    public string DeliveryAddress { get;  }
    public string PhoneNumber { get;  }
    public string Comment { get;  }
    public bool IsExpress { get;  }
    public DateTime CreatedAt { get;  }
    public IPricingStrategy PricingStrategy { get;  }
    public IOrderState State {get; private set;}
    
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
    public string Status => State.Name;
    
    public Order(Guid id, 
        string customerName, 
        string deliveryAddress, 
        string phoneNumber, 
        List<OrderItem> items,
        bool isExpress, 
        IPricingStrategy? pricingStrategy,
        string comment = null)
    {
        if (string.IsNullOrWhiteSpace(customerName)) 
            throw new ArgumentException("Customer name cannot be empty", nameof(customerName));
        if (string.IsNullOrWhiteSpace(deliveryAddress)) 
            throw new ArgumentException("Delivery address cannot be empty", nameof(deliveryAddress));
        if (string.IsNullOrWhiteSpace(phoneNumber)) 
            throw new ArgumentException("Phone number cannot be empty", nameof(phoneNumber));
        ArgumentNullException.ThrowIfNull(items);
        if (items.Count == 0) 
            throw new ArgumentException("Order must contain at least one item", nameof(items));
        
        PricingStrategy = pricingStrategy ?? throw new ArgumentNullException(nameof(pricingStrategy));
        Id = id;
        CustomerName = customerName;
        DeliveryAddress = deliveryAddress;
        PhoneNumber = phoneNumber;
        Comment = comment;
        IsExpress = isExpress;
        CreatedAt = DateTime.UtcNow;
        _items.AddRange(items);
        State = new NewOrderState();
    }
    
    internal void SetState(IOrderState state)
    {
        ArgumentNullException.ThrowIfNull(state);
        State = state;
    }

    public void StartPreparation()
    {
        State.StartPreparation(this);
    }

    public void StartDelivery()
    {
        State.StartDelivery(this);
    }

    public void Complete()
    {
        State.Complete(this);
    }

    public void Cancel()
    {
        State.Cancel(this);
    }

    public decimal GetTotalSum()
    {
        return _items.Sum(i => i.GetTotalPrice());
    }

    public decimal CalculateTotalPrice()
    {
        return PricingStrategy.CalculateTotal(this);
    }
    
}