namespace StaffManager.Core.Order.PeopleOut.Models;

public sealed class GroupInfo
{
    public string ExtendedDestination { get; set; }

    public string Destination { get; set; }

    public string Period { get; set; }

    public string Reason { get; set; }

    public List<PersonInfo> People { get; } = new List<PersonInfo>();

    internal int StartParagraphIndex { get; set; }

    internal int EndParagraphIndex { get; set; }
}
