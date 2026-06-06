using System.Diagnostics.CodeAnalysis;

namespace MatchAIQuote.Core;

public class Quiz(List<QuizEntry> entries, Random random, int answersCount = 4)
{
    private int _timesPlayed = 0;
    private readonly List<QuizEntry> _entries = entries;
    private readonly Random _random = random;
    public int AnswersCount { get; } = answersCount;

    public int EntriesCount => _entries.Count;
    public int TimesPlayed => _timesPlayed;

    public bool TryGetNext([NotNullWhen(true)] out QuizQuestion? question)
    {
        if (_timesPlayed >= _entries.Count || _entries.Count < AnswersCount) {
            question = null;
            return false;
        }

        int transIndex = _random.Next(_timesPlayed, _entries.Count);
        string transText = _entries[transIndex].Translation;

        // Put the correct answer into the answers array
        string[] answers = new string[AnswersCount];
        answers[0] = _entries[transIndex].Quote;

        // If there is a used quote - move it to the end of the "used quotes space"
        if (_timesPlayed > 0)
        {
            (_entries[0], _entries[_timesPlayed]) = (_entries[_timesPlayed], _entries[0]);
        }
        // Put the used quote as the first element of array
        (_entries[0], _entries[transIndex]) = (_entries[transIndex], _entries[0]);

        // Find non-repeating random answers
        for (int i = 1; i < AnswersCount; i++)
        {
            int rndIndex = _random.Next(1, _entries.Count - i - 1);
            answers[i] = _entries[rndIndex].Quote;
            // Swap with the "end" element to avoid repetition
            (_entries[^i], _entries[rndIndex]) = (_entries[^i], _entries[rndIndex]);
        }

        // Shuffle answers
        int correctIndex = 0;
        for (int i = 0; i < AnswersCount - 1; i++)
        {
            int rndIndex = _random.Next(i, AnswersCount);
            if (i == correctIndex) correctIndex = rndIndex;
            (answers[rndIndex], answers[i]) = (answers[i], answers[rndIndex]);
        }

        _timesPlayed++;

        question = new(transText, answers, correctIndex);
        return true;
    }
}