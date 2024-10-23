using ConsoleAppTask_2.Core;
using System;
using System.Collections.Generic;
using System.Linq;

public class Menu
{
    private List<Dish> dishes = new List<Dish>();

    public void AddDish(Dish dish)
    {
        dishes.Add(dish);
    }

    public List<Dish> SearchByCategory(string category)
    {
        return dishes.Where(d => d.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Dish> SearchByPriceRange(decimal minPrice, decimal maxPrice)
    {
        return dishes.Where(d => d.Price >= minPrice && d.Price <= maxPrice).ToList();
    }

    public void UpdateDishPrice(int id, decimal newPrice)
    {
        var dish = dishes.FirstOrDefault(d => d.Id == id);
        if (dish != null)
        {
            dish.Price = newPrice;
        }
    }
}
