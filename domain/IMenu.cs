using System.Collections.ObjectModel;

namespace Lab4FoodDelivery.domain;

public interface IMenu
{
    ReadOnlyCollection<MenuItem> GetAllItems();
    MenuItem GetItemById(Guid id);
}