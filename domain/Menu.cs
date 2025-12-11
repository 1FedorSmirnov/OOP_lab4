using System.Collections.ObjectModel;

namespace Lab4FoodDelivery.domain;

public sealed class Menu: IMenu
{
    private static readonly Menu _instance = new();
    public static Menu Instance => _instance;

    private readonly List<MenuItem> _items = [];

    private Menu()
    {
        
    }
    
    public ReadOnlyCollection<MenuItem> GetAllItems()
    {
       return _items.AsReadOnly();
    }

    public MenuItem GetItemById(Guid id)
    {
        return _items.FirstOrDefault(x => x.Id == id);
    }
}