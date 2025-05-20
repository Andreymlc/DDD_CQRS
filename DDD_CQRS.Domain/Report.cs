namespace DDD_CQRS.Domain;

public class Report
{
    public int NumberOrders { get; init; }
    public decimal Income { get; init; }
    public int NumberCompletedOrders { get; init; }
    public DateTimeOffset CurrentDate { get; } = DateTimeOffset.Now;

    public override string ToString() =>
        $"""
         ++++++++++++++++++++++ Отчет ++++++++++++++++++++++
         Дата: {CurrentDate:g}
         Заказов получено: {NumberOrders}
         Заказов выполнено: {NumberCompletedOrders}
         Доход: {Income}
         +++++++++++++++++++++++++++++++++++++++++++++++++++
         """;
}
