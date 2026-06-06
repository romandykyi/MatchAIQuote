using System.Globalization;
using CsvHelper;

namespace MatchAIQuote.Core;

public class DataProvider
{
    public IReadOnlyDictionary<string, IReadOnlyList<string>> LanguageToCategory { get; }

    private DataProvider(IReadOnlyDictionary<string, IReadOnlyList<string>> languageToCategory)
    {
        LanguageToCategory = languageToCategory;
    }

    public static async Task<DataProvider> LoadAsync(HttpClient http)
    {
        var response = await http.GetAsync("/data/index.csv");
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new InvalidOperationException($"The status code {response.StatusCode} is unexpected.");
        }

        var stream = await response.Content.ReadAsStreamAsync();
        StreamReader reader = new(stream);
        CsvReader csv = new(reader, CultureInfo.InvariantCulture);
        var entries = await csv.GetRecordsAsync<IndexEntry>()
            .GroupBy(x => x.Language)
            .ToDictionaryAsync(
                x => x.Key, 
                x => (IReadOnlyList<string>)x.Select(x => x.Category).ToList()
                );

        return new(entries);
    }
}