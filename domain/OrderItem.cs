namespace Lab4FoodDelivery.domain;

public class OrderItem
{
    public MenuItem MenuItem { get;  }
    public int Quantity { get;  }

    public OrderItem(MenuItem menuItem, int quantity)
    {
        MenuItem = menuItem ?? throw new ArgumentNullException(nameof(menuItem));
        if (quantity <= 0) 
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        Quantity = quantity;
    }

    public decimal GetTotalPrice()
    {
       return MenuItem.Price * Quantity; 
    }
}