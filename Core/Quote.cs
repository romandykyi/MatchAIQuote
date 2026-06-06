using CsvHelper.Configuration.Attributes;

namespace MatchAIQuote.Core;

public class Quote(int index, string text)
{
    [Name("index")]
    public int Index { get; set; } = index;
    [Name("quote_text")]
    public string Text { get; set; } = text;

    public Quote() : this(0, string.Empty) {}
}