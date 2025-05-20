using System.Text;
using DDD_CQRS.Application.Command;
using DDD_CQRS.Application.Query;
using DDD_CQRS.Domain;
using MediatR;

namespace DDD_CQRS;

public class ConsoleUI(IMediator mediator)
{
    private static Order? _currentOrder;
    private OrderStatus _currentOrderStatus = OrderStatus.Created;
    
    public void Start()
    {
        int choice;
        do
        {
            ShowMainMenu();
            choice = ReadIntInput();
            try
            {
                Console.WriteLine("\n");
                HandleMainMenuChoice(choice);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        } while (choice != 0);
    }

    private static void ShowMainMenu()
    {
        Console.WriteLine();
        if (_currentOrder is not null)
            Console.WriteLine($"\nТекущий заказ: {_currentOrder.Id}({_currentOrder.Status}) ");
            
        Console.WriteLine(
            """
            =============== Система управления заказами ===============
            1. Создать новый заказ
            2. Посмотреть историю заказов (последних n)
            3. Посмотреть отчет
            """
        );
        
        if (_currentOrder is not null)
        {
            Console.WriteLine(
                """
                4. Выбрать заказ
                5. Добавить блюдо в текущий заказ
                6. Изменить количество определенного блюда в текущем заказе
                7. Получить статус текущего заказа
                8. Изменить статус текущего заказа
                """
            );
        }

        Console.Write("Выберите действие: ");
    }
    
    private void HandleMainMenuChoice(int choice)
    {
        switch (choice)
        {
            case 0: 
                Console.WriteLine("Выход из программы...");
                break;
            case 1:
                CreateNewOrder();
                break;
            case 2:
                GetOrderHistory();
                break;
            case 3:
                GetReport();
                break;
            case 4:
                ChoiceOrder();
                break;
            case 5:
                AddDishToOrder();
                break;
            case 6:
                ChangeDishesQuantityInOrderItem();
                break;
            case 7:
                GetOrderStatus();
                break;
            case 8:
                ChangeOrderStatus();
                break;
        }
    }

    private void ChoiceOrder()
    {
        Console.WriteLine("Все заказы:");
        var orders = GetAllOrders();

        for (var i = 0; i < orders.Count; i++)
            Console.WriteLine($"{i + 1} -> {orders[i].Id}");

        Console.Write("Выберите заказ: ");
        var choice = ReadIntInput();
        
        _currentOrder = orders[choice - 1];
        _currentOrderStatus = _currentOrder.Status;
        
        Console.WriteLine("Заказ выбран");
    }

    private void CreateNewOrder()
    {
        var random = new Random();
        var orderItems = new List<OrderItem>();
        Dish[] dishes =
        [
            new(Guid.NewGuid(), "Шашлык", 479), new(Guid.NewGuid(), "Теплый салат", 320),
            new(Guid.NewGuid(), "Чизкейк", 400)
        ];

        foreach (var dish in dishes)
        {
            var quantity = 1 + random.Next(5);
            orderItems.Add(OrderItem.Create(dish, quantity));
        }

        _currentOrder = mediator.Send(new CreateOrder { OrderItems = orderItems }).Result;
        _currentOrderStatus = _currentOrder.Status;
    }

    private void GetOrderHistory()
    {
        Console.Write("Введите кол-во заказов: ");
        var numberDays = ReadIntInput();
        
        var history = mediator.Send(new GetOrderHistory { NumberDays = numberDays }).Result;
        
        if (history.Count > 0)
            Console.Write(FormatOrdersTable(history));
        else
            Console.WriteLine("В данный момент нет ни одного заказа");
    }

    private void GetReport()
    {
        var report = mediator.Send(new GetReport()).Result;

        Console.WriteLine();
        Console.WriteLine(report);
    }

    private void AddDishToOrder()
    {
        Console.Write("Введите название блюда: ");
        var name = Console.ReadLine() ?? throw new ArgumentException("Введите название");
        Console.Write("Введите цену блюда: ");
        var price = decimal.Parse(Console.ReadLine()!);
        Console.Write("Введите количество блюда: ");
        var quantity = ReadIntInput();

        mediator.Send(new AddDishToOrder
        {
            OrderId = _currentOrder!.Id,
            Name = name,
            Price = price,
            Quantity = quantity
        }).Wait();
        
        _currentOrderStatus = _currentOrder.Status;
    }

    private void ChangeDishesQuantityInOrderItem()
    {
        var items = _currentOrder!.Items;
        
        Console.WriteLine("Выберете продукт для изменения количества: ");
        for (var i = 0; i < items.Count ; i++)
            Console.WriteLine($"{i + 1} -> {items[i]}");

        Console.Write("Выберете блюдо: ");
        var choice = ReadIntInput();

        Console.Write("Новое количество: ");
        var quantity = ReadIntInput();

        mediator.Send(new ChangeDishesQuantityInOrderItem
        {
            OrderId = _currentOrder.Id,
            DishId = items[choice - 1].Dish.Id,
            NewQuantity = quantity
        }).Wait();
    }

    private void GetOrderStatus() =>
        Console.WriteLine($"{mediator.Send(new GetOrderStatus { OrderId = _currentOrder!.Id }).Result}");

    private void ChangeOrderStatus()
    {
        var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();
        Console.WriteLine("Доступные статусы:");
        for (var i = 0; i < statuses.Length; i++)
            Console.WriteLine($"{i + 1} -> {statuses[i].ToRussianString()}");
        
        Console.WriteLine("Выберите новый статус:");
        var choice = ReadIntInput();
        
        mediator.Send(new ChangeOrderStatus { OrderId = _currentOrder!.Id, OrderStatus = statuses[choice - 1] }).Wait();
    }

    private IReadOnlyList<Order> GetAllOrders() => mediator.Send(new GetAllOrders()).Result;
    
    private static string FormatOrdersTable(IReadOnlyList<Order> orders)
    {
        var sb = new StringBuilder().Append('\n');

        for (var i = 0; i < orders.Count; i++)
        {
            sb.AppendLine(orders[i].ToString());
        
            if (i < orders.Count - 1)
                sb.AppendLine("---------------------------------------------------------\n");
        }
    
        return sb.ToString();
    }

    private static int ReadIntInput()
    {
        try
        {
            return int.Parse(Console.ReadLine()!);
        }
        catch
        {
            return -1;
        }
    }
    
    private static Guid ReadGuidInput()
    {
        try
        {
            return Guid.Parse(Console.ReadLine()!);
        }
        catch
        {
            return Guid.Empty;
        }
    }
}
