using StaffManager.Core.Order;
using StaffManager.Core.Order.PeopleOut.Models;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace StaffManager.Core.BusinessTrip;

public sealed class BusinessTripCertifGenerator
{
    private const string RankPattern = "<Rank>";
    private const string FullNamePattern = "<FullName>";
    private const string WorkPlacePattern = "<WorkPlace>";
    private const string ExtendedDestinationPattern = "<ExtendedDestination>";
    private const string PeriodPattern = "<Period>";
    private const string ReasonPattern = "<Reason>";
    private const string DestinationPattern = "<Destination>";

    private readonly string _templateFilePath;
    private readonly string _targetFileDirectory;

    public BusinessTripCertifGenerator(
        string templateFilePath,
        string targetFilePath)
    {
        ArgumentNullException.ThrowIfNull(templateFilePath);
        ArgumentNullException.ThrowIfNull(targetFilePath);

        _templateFilePath = templateFilePath;
        _targetFileDirectory = targetFilePath;
    }

    public void Produce(OrderInfo order)
    {
        foreach (var group in order.PeopleOut)
        {
            using var templateDocument = DocX.Load(_templateFilePath);
            using var document = CreateTripFile(group);

            var peopleCounter = 1;

            foreach (var person in group.People)
            {
                if (peopleCounter != 1)
                    document.InsertDocument(templateDocument);

                document.ReplaceText(new StringReplaceTextOptions { SearchValue = RankPattern, NewValue = person.Rank });
                document.ReplaceText(new StringReplaceTextOptions { SearchValue = FullNamePattern, NewValue = person.FullName });
                document.ReplaceText(new StringReplaceTextOptions { SearchValue = WorkPlacePattern, NewValue = person.WorkPlace });
                document.ReplaceText(new StringReplaceTextOptions
                {
                    SearchValue = ExtendedDestinationPattern,
                    NewValue = group.ExtendedDestination
                });
                document.ReplaceText(new StringReplaceTextOptions { SearchValue = PeriodPattern, NewValue = group.Period });
                document.ReplaceText(new StringReplaceTextOptions { SearchValue = ReasonPattern, NewValue = group.Reason });
                document.ReplaceText(new StringReplaceTextOptions { SearchValue = DestinationPattern, NewValue = group.Destination });

                peopleCounter++;
            }

            document.Save();
        }
    }

    private DocX CreateTripFile(GroupInfo group)
    {
        var peopleSurnames = group.People.Select(x => x.Surname);

        var templateFileName = Path.GetFileNameWithoutExtension(_templateFilePath);
        var templateFileExtension = Path.GetExtension(_templateFilePath);

        var targetFileName = $"{templateFileName}_{group.Destination}_{string.Join('_', peopleSurnames)}{templateFileExtension}";

        var targetFilePath = Path.Combine(_targetFileDirectory, targetFileName);

        if (File.Exists(targetFilePath))
            File.Delete(targetFilePath);

        File.Copy(_templateFilePath, targetFilePath);

        return DocX.Load(targetFilePath);
    }
}
