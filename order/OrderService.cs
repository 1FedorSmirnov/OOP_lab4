using Lab4FoodDelivery.domain;
using Lab4FoodDelivery.pricing;

namespace Lab4FoodDelivery.order;

/// <summary>
/// Фасад над внутренней логикой заказов
/// Паттерны Builder, Strategy, State, Factory скрыты внутри
/// Внешнему коду доступен простой API
/// </summary>
public class OrderService(IMenu menu)
{
    private readonly IMenu _menu = menu ?? throw new ArgumentNullException(nameof(menu));
    private readonly Dictionary<Guid, Order> _orders = new();

    public List<MenuItem> GetMenu()
    {
        return _menu.GetAllItems().ToList();
    }

    public Order CreateStandardOrder(
        string customerName,
        string deliveryAddress,
        string phoneNumber,
        List<(Guid menuItemId, int quantity)>? items,
        string comment = null)
    {
        return CreateOrderInternal(OrderType.Standard, customerName, deliveryAddress, phoneNumber, items, comment);
    }

    public Order CreateExpressOrder(
        string customerName,
        string deliveryAddress,
        string phoneNumber,
        List<(Guid menuItemId, int quantity)>? items,
        string comment = null
    )
    {
        return CreateOrderInternal(OrderType.Express, customerName, deliveryAddress, phoneNumber, items, comment);
    }

    public Order CreateCustomOrderWithDiscount(
        string customerName,
        string deliveryAddress,
        string phoneNumber,
        List<(Guid menuItemId, int quantity)> items,
        string comment = null)
    {
        return CreateOrderInternal(OrderType.Custom, customerName, deliveryAddress, phoneNumber, items, comment);
    }

    public Order GetOrderById(Guid orderId)
    {
        if (!_orders.TryGetValue(orderId, out var order)) 
            throw new KeyNotFoundException($"Order with id {orderId} not found");
        return order;
    }

    public string GetOrderStatus(Guid orderId)
    {
        var order = GetOrderById(orderId);
        return order.Status;
    }

    public decimal GetTotalSum(Guid orderId)
    {
        var order = GetOrderById(orderId);
        return order.CalculateTotalPrice();
    }

    public void StartOrderPreparation(Guid orderId)
    {
        var order = GetOrderById(orderId);
        order.StartPreparation();
    }
    
    public void StartOrderDelivery(Guid orderId)
    {
        var order = GetOrderById(orderId);
        order.StartDelivery();
    }
    
    public void CompleteOrder(Guid orderId)
    {
        var order = GetOrderById(orderId);
        order.Complete();
    }
    
    public void CancelOrder(Guid orderId)
    {
        var order = GetOrderById(orderId);
        order.Cancel();
    }
    
    private Order CreateOrderInternal(
        OrderType orderType,
        string customerName,
        string deliveryAddress,
        string phoneNumber,
        List<(Guid menuItemId, int quantity)> items,
        string comment)
    {
        ArgumentNullException.ThrowIfNull(items);
        
        if (items.Count == 0)
            throw new ArgumentException("Order item list is empty", nameof(items));
        var builder = OrderFactory.Create(orderType, _menu)
            .WithCustomerName(customerName)
            .WithDeliveryAddress(deliveryAddress)
            .WithPhoneNumber(phoneNumber)
            .WithComment(comment);
        foreach (var (menuItemId, quantity) in items)
        {
            builder.AddItem(menuItemId, quantity);
        }
        var order = builder.Build();
        _orders.Add(order.Id, order);
        return order;
    }
}