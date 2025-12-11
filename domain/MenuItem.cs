namespace Lab4FoodDelivery.domain;

public class MenuItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public MenuItem(Guid id, string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or whitespace", nameof(name));
        if (price <= 0)
            throw new ArgumentException("Price must be greater than zero", nameof(price));
        Id = id;
        Name = name;
        Price = price;
    }
}