using StaffManager.Core.Order;

namespace StaffManager.Tests;

public class OrderParserTests
{
    [Fact]
    public void LoadFile()
    {
        // Arrange
        var orderFilePath = @"C:\Users\oleksii.ishchuk\Downloads\НАКАЗ.docx";

        // Act
        var order = OrderParser.Parse(orderFilePath);

        // Assert

    }
}