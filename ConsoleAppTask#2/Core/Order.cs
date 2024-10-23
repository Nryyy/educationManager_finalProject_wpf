using ConsoleAppTask_2.Core;

public class Order
{
    public int TableNumber { get; set; }
    public List<Dish> Dishes { get; set; }
    public DateTime OrderTime { get; set; }
    public string Status { get; set; }

    public Order(int tableNumber, List<Dish> dishes)
    {
        TableNumber = tableNumber;
        Dishes = dishes;
        OrderTime = DateTime.Now;
        Status = "Pending";
    }
}
