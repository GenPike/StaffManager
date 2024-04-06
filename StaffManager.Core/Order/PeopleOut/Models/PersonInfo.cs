namespace StaffManager.Core.Order.PeopleOut.Models;

public sealed class PersonInfo
{
    public string Rank { get; set; }

    public string FullName { get; set; }

    public string WorkPlace { get; set; }

    public string Destination { get; private set; }
    
    public string ExtendedDestination { get; private set; }

    public string Period { get; private set; }

    public string Reason { get; private set; }

    internal void EnrichWithGroupInfo(GroupInfo groupInfo)
    {
        Period = groupInfo.Period;
        Reason = groupInfo.Reason;
        Destination = groupInfo.Destination;
        ExtendedDestination = groupInfo.ExtendedDestination;
    }
}
