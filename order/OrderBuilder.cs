using Lab4FoodDelivery.domain;
using Lab4FoodDelivery.pricing;

namespace Lab4FoodDelivery.order;

public class OrderBuilder
{
    private readonly IMenu _menu;
    private readonly List<OrderItem> _items = [];
    
    private string _customerName;
    private string _deliveryAddress;
    private string _phoneNumber;
    private bool _isExpress;
    private IPricingStrategy? _pricingStrategy;
    private string? _comment;
    
    public OrderBuilder(IMenu menu)
    {
        _menu = menu ?? throw new ArgumentNullException(nameof(menu));
    }

    public OrderBuilder WithCustomerName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name cannot be empty", nameof(name));
        _customerName = name;
        return this;
    }

    public OrderBuilder WithDeliveryAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Delivery address cannot be empty", nameof(address));
        _deliveryAddress = address;
        return this;
    }

    public OrderBuilder WithPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number cannot be empty", nameof(phoneNumber));
        _phoneNumber = phoneNumber;
        return this;
    }

    public OrderBuilder WithComment(string comment)
    {
        _comment = comment;
        return this;
    }
    
    public OrderBuilder WithExpressDelivery()
    {
        _isExpress = true;
        return this;
    }

    public OrderBuilder WithPricingStrategy(IPricingStrategy pricingStrategy)
    {
        _pricingStrategy = pricingStrategy ?? throw new ArgumentNullException(nameof(pricingStrategy));
        return this;
    }

    public OrderBuilder AddItem(Guid menuItemId, int quantity)
    {
        var menuItem = _menu.GetItemById(menuItemId);
        if (menuItem == null)
            throw new ArgumentException($"Menu item with id {menuItemId} not found", nameof(menuItemId));
        
        if (quantity <= 0) 
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        
        _items.Add(new OrderItem(menuItem, quantity));
        return this;
    }

    public Order Build()
    {
        if (string.IsNullOrWhiteSpace(_customerName))
            throw new InvalidOperationException("Customer name not specified");
        
        if (string.IsNullOrWhiteSpace(_deliveryAddress))
            throw new InvalidOperationException("Delivery address not specified");

        if (string.IsNullOrWhiteSpace(_phoneNumber))
            throw new InvalidOperationException("Phone number not specified");
        
        if (_items.Count == 0) 
            throw new InvalidOperationException("Order contains no items");
        
        if (_pricingStrategy == null) 
            throw new InvalidOperationException("Pricing strategy not specified");
        return new Order(
            Guid.NewGuid(),
            _customerName,
            _deliveryAddress,
            _phoneNumber,
            _items,
            _isExpress,
            _pricingStrategy,
            _comment);
    }
}