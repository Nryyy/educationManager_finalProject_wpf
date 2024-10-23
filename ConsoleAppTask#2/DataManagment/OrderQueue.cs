public class OrderQueue
{
    private Queue<Order> orders = new Queue<Order>();

    public void AddOrder(Order order)
    {
        orders.Enqueue(order);
    }

    public Order ProcessOrder()
    {
        return orders.Dequeue();
    }

    public TimeSpan AverageWaitTime()
    {
        if (orders.Count == 0) return TimeSpan.Zero;

        var totalWaitTime = orders.Sum(order => (DateTime.Now - order.OrderTime).TotalMinutes);
        return TimeSpan.FromMinutes(totalWaitTime / orders.Count);
    }
}
