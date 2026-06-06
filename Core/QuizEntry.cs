using MatchAIQuote.Core;

namespace MatchAIQuote;

public record class QuizEntry(int Index, string Quote, string Translation)
{
    public static QuizEntry FromQuotes(Quote origin, Quote translation)
    {
        return new(origin.Index, origin.Text, translation.Text);
    }
}