namespace StaffManager.Core.Order.PeopleOut.Models;

public sealed class GroupInfo
{
    public string ExtendedDestination { get; set; }

    public string Destination { get; set; }

    public string Period { get; set; }

    public string Reason { get; set; }

    public int StartParagraphIndex { get; set; }

    public int EndParagraphIndex { get; set; }
}
