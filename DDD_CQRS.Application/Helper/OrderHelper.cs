using DDD_CQRS.Domain;
namespace DDD_CQRS.Application.Helper;

public class OrderHelper
{
    private static readonly Dictionary<string, int> ProductSales = new();

    public static (string Name, int Quantity) UpdateProductSales(Order order)
    {
        foreach (var item in order.Items)
        {
            if (!ProductSales.TryAdd(item.Dish.Name, item.Quantity))
                ProductSales[item.Dish.Name] += item.Quantity;
        }

        return GetMostPopularProduct();
    }

    public static (string Name, int Quantity) UpdateProductSales(string name, int quantity)
    {
        if (!ProductSales.TryAdd(name, quantity))
            ProductSales[name] += quantity;

        return GetMostPopularProduct();
    }

    private static (string Name, int Quantity) GetMostPopularProduct()
    {
        var mostPopularProduct = ProductSales.MaxBy(p => p.Value);
        
        return (mostPopularProduct.Key, mostPopularProduct.Value);
    }
}
