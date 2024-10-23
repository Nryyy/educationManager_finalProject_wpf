using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAppTask_2.Core;

namespace ConsoleAppTask_2
{
    class Program
    {
        static Menu menu = new Menu();
        static EmployeeManager employeeManager = new EmployeeManager();
        static OrderQueue orderQueue = new OrderQueue();
        static FinancialManager financialManager = new FinancialManager();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n--- Ресторанна система ---");
                Console.WriteLine("1. Управління меню");
                Console.WriteLine("2. Управління співробітниками");
                Console.WriteLine("3. Управління замовленнями");
                Console.WriteLine("4. Управління фінансовими операціями");
                Console.WriteLine("5. Вихід");
                Console.Write("Оберіть дію: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageMenu();
                        break;
                    case "2":
                        ManageEmployees();
                        break;
                    case "3":
                        ManageOrders();
                        break;
                    case "4":
                        ManageFinancials();
                        break;
                    case "5":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }
            }
        }

        static void ManageMenu()
        {
            Console.WriteLine("\n--- Управління меню ---");
            Console.WriteLine("1. Додати нову страву");
            Console.WriteLine("2. Пошук страв за категорією");
            Console.WriteLine("3. Пошук страв за ціновим діапазоном");
            Console.WriteLine("4. Оновити ціну страви");
            Console.Write("Оберіть дію: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Введіть ID страви: ");
                    int id = int.Parse(Console.ReadLine());
                    Console.Write("Введіть назву страви: ");
                    string name = Console.ReadLine();
                    Console.Write("Введіть опис страви: ");
                    string description = Console.ReadLine();
                    Console.Write("Введіть ціну страви: ");
                    decimal price = decimal.Parse(Console.ReadLine());
                    Console.Write("Введіть категорію страви: ");
                    string category = Console.ReadLine();
                    menu.AddDish(new Dish(id, name, description, price, category));
                    Console.WriteLine("Страву додано.");
                    break;
                case "2":
                    Console.Write("Введіть категорію для пошуку: ");
                    category = Console.ReadLine();
                    var dishesByCategory = menu.SearchByCategory(category);
                    foreach (var dish in dishesByCategory)
                    {
                        Console.WriteLine($"{dish.Id} - {dish.Name}: {dish.Price} грн.");
                    }
                    break;
                case "3":
                    Console.Write("Введіть мінімальну ціну: ");
                    decimal minPrice = decimal.Parse(Console.ReadLine());
                    Console.Write("Введіть максимальну ціну: ");
                    decimal maxPrice = decimal.Parse(Console.ReadLine());
                    var dishesByPrice = menu.SearchByPriceRange(minPrice, maxPrice);
                    foreach (var dish in dishesByPrice)
                    {
                        Console.WriteLine($"{dish.Id} - {dish.Name}: {dish.Price} грн.");
                    }
                    break;
                case "4":
                    Console.Write("Введіть ID страви для оновлення: ");
                    id = int.Parse(Console.ReadLine());
                    Console.Write("Введіть нову ціну: ");
                    decimal newPrice = decimal.Parse(Console.ReadLine());
                    menu.UpdateDishPrice(id, newPrice);
                    Console.WriteLine("Ціну оновлено.");
                    break;
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        }

        static void ManageEmployees()
        {
            Console.WriteLine("\n--- Управління співробітниками ---");
            Console.WriteLine("1. Додати нового співробітника");
            Console.WriteLine("2. Отримати інформацію про співробітника");
            Console.WriteLine("3. Оновити графік роботи або рейтинг");
            Console.Write("Оберіть дію: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Введіть ID співробітника: ");
                    int id = int.Parse(Console.ReadLine());
                    Console.Write("Введіть ім'я співробітника: ");
                    string name = Console.ReadLine();
                    Console.Write("Введіть посаду: ");
                    string position = Console.ReadLine();
                    Console.Write("Введіть графік роботи: ");
                    string schedule = Console.ReadLine();
                    Console.Write("Введіть рейтинг співробітника: ");
                    double rating = double.Parse(Console.ReadLine());
                    employeeManager.AddEmployee(id, new Employee(name, position, schedule, rating));
                    Console.WriteLine("Співробітника додано.");
                    break;
                case "2":
                    Console.Write("Введіть ID співробітника: ");
                    id = int.Parse(Console.ReadLine());
                    var employee = employeeManager.GetEmployeeById(id);
                    if (employee != null)
                    {
                        Console.WriteLine($"Ім'я: {employee.Name}, Посада: {employee.Position}, Графік: {employee.Schedule}, Рейтинг: {employee.Rating}");
                    }
                    else
                    {
                        Console.WriteLine("Співробітник не знайдений.");
                    }
                    break;
                case "3":
                    Console.Write("Введіть ID співробітника для оновлення: ");
                    id = int.Parse(Console.ReadLine());
                    Console.Write("Введіть новий графік роботи (або залиште порожнім): ");
                    schedule = Console.ReadLine();

                    // Видаляємо дублююче оголошення змінної rating і використовуємо вже існуючу змінну.
                    Console.Write("Введіть новий рейтинг (або залиште порожнім): ");
                    string ratingInput = Console.ReadLine();

                    // Перевіряємо, чи було введено новий рейтинг
                    double? newRating = string.IsNullOrWhiteSpace(ratingInput) ? (double?)null : double.Parse(ratingInput);

                    // Викликаємо метод з графіком і новим рейтингом
                    employeeManager.UpdateEmployeeScheduleOrRating(id, schedule, newRating);

                    Console.WriteLine("Інформацію оновлено.");
                    break;
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        }

        static void ManageOrders()
        {
            Console.WriteLine("\n--- Управління замовленнями ---");
            Console.WriteLine("1. Додати нове замовлення");
            Console.WriteLine("2. Обробити замовлення");
            Console.WriteLine("3. Оцінка часу очікування");
            Console.Write("Оберіть дію: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Введіть номер столика: ");
                    int tableNumber = int.Parse(Console.ReadLine());
                    List<Dish> orderedDishes = new List<Dish>();
                    Console.WriteLine("Введіть ID страв (введіть -1 для завершення): ");
                    while (true)
                    {
                        int dishId = int.Parse(Console.ReadLine());
                        if (dishId == -1) break;
                        // Реально слід було б перевіряти, чи страва існує, але для спрощення цього не робимо
                        orderedDishes.Add(new Dish(dishId, "Страва " + dishId, "Опис", 100, "Категорія"));
                    }
                    orderQueue.AddOrder(new Order(tableNumber, orderedDishes));
                    Console.WriteLine("Замовлення додано.");
                    break;
                case "2":
                    var processedOrder = orderQueue.ProcessOrder();
                    Console.WriteLine($"Замовлення для столика {processedOrder.TableNumber} оброблено.");
                    break;
                case "3":
                    var avgWaitTime = orderQueue.AverageWaitTime();
                    Console.WriteLine($"Середній час очікування: {avgWaitTime.TotalMinutes} хвилин.");
                    break;
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        }

        static void ManageFinancials()
        {
            Console.WriteLine("\n--- Управління фінансовими операціями ---");
            Console.WriteLine("1. Додати нову операцію");
            Console.WriteLine("2. Скасувати останню операцію");
            Console.WriteLine("3. Переглянути останню операцію");
            Console.Write("Оберіть дію: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Введіть тип операції (Прибуток або Витрата): ");
                    string type = Console.ReadLine();
                    Console.Write("Введіть суму: ");
                    decimal amount = decimal.Parse(Console.ReadLine());
                    Console.Write("Введіть опис: ");
                    string description = Console.ReadLine();
                    financialManager.AddTransaction(new FinancialTransaction(type, amount, description));
                    Console.WriteLine("Операцію додано.");
                    break;
                case "2":
                    var lastTransaction = financialManager.UndoLastTransaction();
                    Console.WriteLine($"Остання операція скасована: {lastTransaction.Type} на суму {lastTransaction.Amount} грн.");
                    break;
                case "3":
                    lastTransaction = financialManager.ViewLastTransaction();
                    Console.WriteLine($"Остання операція: {lastTransaction.Type} на суму {lastTransaction.Amount} грн.");
                    break;
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        }
    }
}
