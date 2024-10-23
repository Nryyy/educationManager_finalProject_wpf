public class EmployeeManager
{
    private Dictionary<int, Employee> employees = new Dictionary<int, Employee>();

    public void AddEmployee(int id, Employee employee)
    {
        employees[id] = employee;
    }

    public Employee GetEmployeeById(int id)
    {
        employees.TryGetValue(id, out var employee);
        return employee;
    }

    public void UpdateEmployeeScheduleOrRating(int id, string newSchedule = null, double? newRating = null)
    {
        if (employees.TryGetValue(id, out var employee))
        {
            if (newSchedule != null)
            {
                employee.Schedule = newSchedule;
            }
            if (newRating.HasValue)
            {
                employee.Rating = newRating.Value;
            }
        }
    }
}
