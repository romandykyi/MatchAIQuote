using System.Globalization;
using System.Runtime.CompilerServices;
using CsvHelper;

namespace MatchAIQuote.Core;

public class DataProvider
{
    private readonly HttpClient _http;
    public IReadOnlyDictionary<string, IReadOnlyList<string>> LanguageToCategories { get; }

    private DataProvider(HttpClient http, IReadOnlyDictionary<string, IReadOnlyList<string>> languageToCategory)
    {
        _http = http;
        LanguageToCategories = languageToCategory;
    }

    public async Task<List<QuizEntry>> LoadEntries(string language, string category)
    {
        if (!LanguageToCategories.ContainsKey(language))
        {
            throw new ArgumentException($"Invalid language \"{language}\"", nameof(language));
        }

        var quotesResponse = await _http.GetAsync($"/data/quotes-{category}.csv");
        if (quotesResponse.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new InvalidOperationException($"The status code (for quotes) {quotesResponse.StatusCode} is unexpected.");
        }

        var transResponse = await _http.GetAsync($"/data/{language}/trans-{category}.csv");
        if (transResponse.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new InvalidOperationException($"The status code (for translations) {quotesResponse.StatusCode} is unexpected.");
        }

        var quotesStream = await quotesResponse.Content.ReadAsStreamAsync();
        using StreamReader quotesReader = new(quotesStream);
        using CsvReader quotesCsv = new(quotesReader, CultureInfo.InvariantCulture);

        var transStream = await transResponse.Content.ReadAsStreamAsync();
        using StreamReader transReader = new(transStream);
        using CsvReader transCsv = new(transReader, CultureInfo.InvariantCulture);

        Quote originQuote = new();
        Quote transQuote = new();
        var quotes = quotesCsv.EnumerateRecordsAsync(originQuote).GetAsyncEnumerator();
        var translations = transCsv.EnumerateRecordsAsync(transQuote).GetAsyncEnumerator();

        List<QuizEntry> entries = [];
        while (await quotes.MoveNextAsync() && await translations.MoveNextAsync())
        {
            entries.Add(QuizEntry.FromQuotes(originQuote, transQuote));
        }
        return entries;
    }

    public static async Task<DataProvider> CreateAsync(HttpClient http)
    {
        var response = await http.GetAsync("/data/index.csv");
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new InvalidOperationException($"The status code {response.StatusCode} is unexpected.");
        }

        var stream = await response.Content.ReadAsStreamAsync();
        using StreamReader reader = new(stream);
        using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
        var entries = await csv.GetRecordsAsync<IndexEntry>()
            .GroupBy(x => x.Language)
            .ToDictionaryAsync(
                x => x.Key, 
                x => (IReadOnlyList<string>)x.Select(x => x.Category).ToList()
                );

        return new(http, entries);
    }
}