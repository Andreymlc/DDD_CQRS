using System.Text;

namespace DDD_CQRS.Domain;

public class Order : Entity<Guid>
{
    public Guid Id { get; private set; }
    public decimal Cost { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; }
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
    
    private readonly List<OrderItem> _items;

    public Order(IEnumerable<OrderItem> dishes)
    {
        Id = Guid.NewGuid();
        _items = dishes.ToList();
        CreatedAt = DateTimeOffset.Now;
        Status = OrderStatus.Created;
        Cost = CalculateTotalPrice();
    }

    public void AddItem(Dish dish, int quantity)
    {
        ArgumentNullException.ThrowIfNull(dish);
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Количество должно быть положительным");

        _items.Add(OrderItem.Create(dish, quantity));
        Cost = CalculateTotalPrice();
    }

    public void ChangeDishesQuantityInOrderItem(int newQuantity, Guid dishId)
    {
        var itemForChange = _items
                                .FirstOrDefault(item => item.Dish.Id == dishId)
                                 ?? throw new NullReferenceException("Блюдо не найден");
        
        itemForChange.ChangeDishesQuantity(newQuantity);
    }

    public void RemoveItem(Guid dishId)
    {
        var itemForRemove = _items
            .FirstOrDefault(item => item.Dish.Id == dishId)
             ?? throw new NullReferenceException("Блюдо не найден");
        
        _items.Remove(itemForRemove);
        Cost = CalculateTotalPrice();
    }

    public void ChangeStatus(OrderStatus newStatus)
    {
        if (!IsValidStatusTransition(Status, newStatus))
            throw new InvalidOperationException($"Недопустимый переход статуса:" +
                                                $" {Status.ToRussianString()} -> {newStatus.ToRussianString()}");

        Status = newStatus;
    }
    
    private static bool IsValidStatusTransition(OrderStatus current, OrderStatus next)
    {
        return current switch
        {
            OrderStatus.Created => next is OrderStatus.Preparing or OrderStatus.Canceled,
            OrderStatus.Preparing => next is OrderStatus.Completed or OrderStatus.Canceled,
            OrderStatus.Completed => next is OrderStatus.Canceled,
            _ => false
        };
    }
    
    public decimal CalculateTotalPrice() => _items.Sum(item => item.TotalPrice);
    
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine("++++++++++++++++++++++++++++++++++++++++++++++");
        sb.AppendLine($" ЗАКАЗ №: {Id,-28}");
        sb.AppendLine("++++++++++++++++++++++++++++++++++++++++++++++");
        sb.AppendLine($" Сумма: {Cost} руб.{"",23}");
        sb.AppendLine($" Статус: {Status.ToRussianString(),-25}");
        sb.AppendLine($" Дата создания: {CreatedAt:dd.MM.yyyy HH:mm}{"",12}");
        sb.AppendLine("++++++++++++++++++++++++++++++++++++++++++++++");
        sb.AppendLine($" Блюда:{"",32}");
    
        foreach (var item in _items)
            sb.AppendLine($"   • {item.Dish.Name,-25} × {item.Quantity,2} ");
    
        sb.AppendLine("++++++++++++++++++++++++++++++++++++++++++++++");
        return sb.ToString();
    }
}
