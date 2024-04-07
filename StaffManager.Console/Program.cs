using StaffManager.Core.BusinessTrip;
using StaffManager.Core.Order;

var orderFilePath = @"C:\Users\oleksii.ishchuk\Downloads\НАКАЗ.docx";

var orderParser = new OrderParser(orderFilePath);
var order = orderParser.Parse();

var templateFilePath = @"C:\Users\oleksii.ishchuk\Downloads\Посвідчення_про_відрядження.docx";
var targetFileDirectory = @"C:\Users\oleksii.ishchuk\Desktop";

var businessTripGenerator = new BusinessTripCertifGenerator(templateFilePath, targetFileDirectory);
businessTripGenerator.Produce(order);
