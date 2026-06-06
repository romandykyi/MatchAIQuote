using CsvHelper.Configuration.Attributes;

namespace MatchAIQuote.Core;

public class IndexEntry(string language, string category)
{
    [Name("language")]
    public string Language { get; set; } = language;
    [Name("category")]
    public string Category { get; set; } = category;
}