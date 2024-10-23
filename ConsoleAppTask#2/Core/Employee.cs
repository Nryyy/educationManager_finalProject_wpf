public class Employee
{
    public string Name { get; set; }
    public string Position { get; set; }
    public string Schedule { get; set; }
    public double Rating { get; set; }

    public Employee(string name, string position, string schedule, double rating)
    {
        Name = name;
        Position = position;
        Schedule = schedule;
        Rating = rating;
    }
}
