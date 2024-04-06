using StaffManager.Core.Order.PeopleOut.Models;
using System.Text.RegularExpressions;
using Xceed.Words.NET;

namespace StaffManager.Core.Order.PeopleOut;

internal static class PeopleOutParser
{
    private const string SectionStartPattern = "що вибули";
    private const string WhereToPattern = "У відрядження до";
    private const string ReasonPattern = "Підстава:";

    private static readonly Regex _tripleNumerationRegex = new(@"\d+\.\d+\.\d+\.", RegexOptions.Compiled);

    public static List<PersonInfo> Parse(DocX document)
    {
        var peopleOutTextIndex = document.IndexOfFirst(SectionStartPattern);
        if (peopleOutTextIndex == -1)
            return new List<PersonInfo>();

        var peopleOutParagraphIndex = document.IndexOfParagraph(peopleOutTextIndex);

        var groups = GetGroups(document, peopleOutParagraphIndex);

        return GetPeople(document, groups);
    }

    private static List<PersonInfo> GetPeople(DocX document, List<GroupInfo> groups)
    {
        var people = new List<PersonInfo>();

        foreach (var group in groups)
        {
            var currentParagraphIndex = group.StartParagraphIndex + 2;

            do
            {
                var text = document.Paragraphs[currentParagraphIndex].Text;

                var person = ParsePersonInfo(text);

                person.EnrichWithGroupInfo(group);

                people.Add(person);

                currentParagraphIndex += 3;

            } while (currentParagraphIndex < group.EndParagraphIndex);
        }

        return people;
    }

    private static List<GroupInfo> GetGroups(DocX document, int startParagraphIndex)
    {
        var groups = new List<GroupInfo>();

        var currentParagraphIndex = startParagraphIndex;

        while (true)
        {
            currentParagraphIndex += 2;

            var text = document.Paragraphs[currentParagraphIndex].Text;

            var whereToIndex = text.IndexOf(WhereToPattern);
            if (whereToIndex == -1)
                break;

            var groupInfo = GetGroupInfo(document, currentParagraphIndex);
            groups.Add(groupInfo);

            currentParagraphIndex = groupInfo.EndParagraphIndex;
        }

        return groups;
    }

    private static GroupInfo GetGroupInfo(DocX document, int startParagraphIndex)
    {
        var group = new GroupInfo();

        var headerText = document.Paragraphs[startParagraphIndex].Text;

        var whereToIndex = headerText.IndexOf(WhereToPattern);
        ParseGroupHeader(group, headerText, whereToIndex + WhereToPattern.Length);

        group.StartParagraphIndex = startParagraphIndex;

        var currentParagraphIndex = startParagraphIndex;

        while (true)
        {
            currentParagraphIndex++;

            var text = document.Paragraphs[currentParagraphIndex].Text;

            if (!text.StartsWith(ReasonPattern))
                continue;

            break;
        }

        var footerText = document.Paragraphs[currentParagraphIndex].Text;
        ParseGroupFooter(group, footerText, ReasonPattern.Length);

        group.EndParagraphIndex = currentParagraphIndex;

        return group;
    }

    private static void ParseGroupHeader(GroupInfo group, string text, int startIndex)
    {
        var firstCommaIndex = text.IndexOf(',');

        group.ExtendedDestination = text[startIndex..firstCommaIndex].Trim();

        var lastWhiteSpaceIndex = group.ExtendedDestination.LastIndexOf(' ');
        group.Destination = group.ExtendedDestination[lastWhiteSpaceIndex..];

        var lastCommaIndex = text.LastIndexOf(',') + 1;
        var colonIndex = text.IndexOf(':');

        group.Period = text[lastCommaIndex..colonIndex].Trim();
    }

    private static void ParseGroupFooter(GroupInfo group, string text, int startIndex)
    {
        group.Reason = text[startIndex..text.Length].Trim();
    }

    private static PersonInfo ParsePersonInfo(string text)
    {
        var person = new PersonInfo();

        var textWithoutNumeration = _tripleNumerationRegex.Replace(text, string.Empty).Trim();

        var firstWhiteSpaceIndex = textWithoutNumeration.IndexOf(' ');
        person.Rank = textWithoutNumeration[..firstWhiteSpaceIndex];

        var startOfNameIndex = firstWhiteSpaceIndex + 1;
        var firstCommaIndex = textWithoutNumeration.IndexOf(',');
        person.FullName = textWithoutNumeration[startOfNameIndex..firstCommaIndex].Trim();

        var startWorkPlaceIndex = firstCommaIndex + 1;
        var lastCommaIndex = textWithoutNumeration.LastIndexOf(',');
        person.WorkPlace = textWithoutNumeration[startWorkPlaceIndex..lastCommaIndex].Trim();

        return person;
    }
}
