using Xceed.Words.NET;

namespace StaffManager.Core;

internal static class DocumentExtensions
{
    public static int IndexOfFirst(this DocX document, string text)
    {
        var foundTextIndexes = document.FindAll(text);
        if (foundTextIndexes.Count == 0)
            return -1;

        return foundTextIndexes.First();
    }

    public static int IndexOfParagraph(this DocX document, int textIndex)
    {
        var peopleOutParagraphIndex = 0;
        foreach (var parag in document.Paragraphs)
        {
            if (parag.StartIndex <= textIndex && parag.EndIndex > textIndex)
                break;

            peopleOutParagraphIndex++;
        }

        return peopleOutParagraphIndex;
    }
}
