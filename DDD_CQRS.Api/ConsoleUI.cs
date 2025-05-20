using System.Text;
using DDD_CQRS.Application.Command;
using DDD_CQRS.Application.Query;
using DDD_CQRS.Domain;
using MediatR;

namespace DDD_CQRS.Api;

public class ConsoleUI(IMediator mediator)
{
    private Order? _currentOrder;
    private OrderStatus _currentOrderStatus = OrderStatus.Created;
    
    public void Start()
    {
        int choice;
        do
        {
            ShowMainMenu();
            choice = ReadIntInput();
            HandleMainMenuChoice(choice);
        } while (choice != 0);
    }

    private static void ShowMainMenu()
    {
        Console.Write(
            """
            
            =============== Система управления заказами ===============
            1. Создать новый заказ
            2. Завершить заказ
            3. Добавить блюдо в заказ
            4. Изменить количество определенного блюда в заказе
            5. Вывести все заказы 
            6. Посмотреть историю заказов
            7. Посмотреть отчет
            8. Получить статус заказа
            Выберите действие: 
            """
            );
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
                CompleteOrder();
                break;
            case 3:
                Console.WriteLine("Добавить блюдо в заказ");
                break;
            case 4:
                Console.WriteLine("Изменить количество определенного блюда в заказе");
                break;
            case 5:
                GetAllOrders();
                break;
            case 6:
                Console.WriteLine("Посмотреть историю заказов");
                break;
            case 7:
                Console.WriteLine("Посмотреть отчет");
                break;
            case 8:
                Console.WriteLine("Получить статус заказа");
                break;
        }
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
            var quantity = 5 + random.Next(26);
            orderItems.Add(OrderItem.Create(dish, quantity));
        }

        _currentOrder = mediator.Send(new CreateOrder { OrderItems = orderItems }).Result;
        _currentOrderStatus = _currentOrder.Status;
        Console.WriteLine($"Создан новый заказ #{_currentOrder.Id} | Статус - {_currentOrderStatus.ToRussianString()}");
    }

    public void CompleteOrder()
    {
        Console.Write("Введите Id заказа: ");
        var id = ReadGuidInput();

        mediator.Send(new CompleteOrder { OrderId = id });
        Console.WriteLine("Заказ успешно завершен");
    }

    public void GetAllOrders()
    {
        var order = mediator.Send(new GetAllOrders()).Result;

        Console.WriteLine(FormatOrdersTable(order));
    }
    
    private static string FormatOrdersTable(IReadOnlyList<Order> orders)
    {
        var sb = new StringBuilder();

        foreach (var order in orders)
            sb.AppendLine(order.ToString());
    
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
