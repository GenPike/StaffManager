using StaffManager.Core.Order.PeopleOut;
using Xceed.Words.NET;

namespace StaffManager.Core.Order;

public class OrderParser
{
    private readonly string _filePath;

    public OrderParser(string filePath)
    {
        ArgumentNullException.ThrowIfNull(filePath);

        _filePath = filePath;
    }

    public OrderInfo Parse()
    {
        using var document = DocX.Load(_filePath);

        var order = new OrderInfo();

        order.PeopleOut = PeopleOutParser.Parse(document);

        return order;
    }
}
