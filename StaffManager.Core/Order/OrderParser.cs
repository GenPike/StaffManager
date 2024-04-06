using StaffManager.Core.Order.PeopleOut;
using Xceed.Words.NET;

namespace StaffManager.Core.Order;

public static class OrderParser
{
    public static OrderInfo Parse(string filePath)
    {
        ArgumentNullException.ThrowIfNull(filePath);

        using var document = DocX.Load(filePath);

        var order = new OrderInfo();

        order.PeopleOut = PeopleOutParser.Parse(document);

        return order;
    }
}
