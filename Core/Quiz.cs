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
        // Put the chosen quote as the first element of the array
        (_entries[0], _entries[transIndex]) = (_entries[transIndex], _entries[0]);

        // Find non-repeating random answers (array lookup should be faster than hashset for small answers count)
        int[] usedIndices = new int[AnswersCount - 1];
        for (int i = 1; i < AnswersCount; i++)
        {
            // Naive, but our datasets are big and collision probability is small
            int rndIndex;
            do
            {
                rndIndex = _random.Next(1, _entries.Count);
            } while (usedIndices.IndexOf(rndIndex) != -1);
            usedIndices[i - 1] = rndIndex;

            answers[i] = _entries[rndIndex].Quote;
        }

        // Shuffle answers
        int correctIndex = 0;
        for (int i = 0; i < AnswersCount - 1; i++)
        {
            int rndIndex = _random.Next(i, AnswersCount);
            if (i == correctIndex) correctIndex = rndIndex;
            else if (rndIndex == correctIndex) correctIndex = i;
            (answers[rndIndex], answers[i]) = (answers[i], answers[rndIndex]);
        }

        _timesPlayed++;

        question = new(transText, answers, correctIndex);
        return true;
    }
}