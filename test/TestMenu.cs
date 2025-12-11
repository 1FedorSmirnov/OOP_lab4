using System.Collections.ObjectModel;
using Lab4FoodDelivery.domain;

namespace Lab4FoodDelivery.test;
/// <summary>
/// Тестовая реализация меню, чтобы не зависеть от Singleton Menu.Instance
/// </summary>
public class TestMenu : IMenu
{
    private readonly List<MenuItem> _items = [];
    
    public TestMenu()
    {
        //Фиксированные id для тестов
        PizzaId = Guid.NewGuid();
        BurgerId = Guid.NewGuid();
        
        _items.Add(new MenuItem(PizzaId, "Test pizza", 500m));
        _items.Add(new MenuItem(BurgerId, "Test burger", 300m));
    }

    public Guid BurgerId { get; set; }
    public Guid PizzaId { get; set; }

    public ReadOnlyCollection<MenuItem> GetAllItems()
    {
        return _items.AsReadOnly();
    }

    public MenuItem GetItemById(Guid id)
    {
        return _items.All(x => x.Id != id) ? null : _items.First(x => x.Id == id);
    }
}