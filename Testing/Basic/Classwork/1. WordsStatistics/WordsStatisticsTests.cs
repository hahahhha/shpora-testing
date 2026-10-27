using Basic.Task.WordsStatistics.WordsStatistics;
using FluentAssertions;
using NUnit.Framework;

namespace Basic.Task.WordsStatistics;

// Документация по FluentAssertions с примерами : https://github.com/fluentassertions/fluentassertions/wiki

[TestFixture]
public class WordsStatisticsTests
{

    private IWordsStatistics wordsStatistics;

    [SetUp]
    public void SetUp()
    {
        wordsStatistics = CreateStatistics();
    }

    public virtual IWordsStatistics CreateStatistics()
    {
        // меняется на разные реализации при запуске exe
        return new WordsStatisticsImpl();
    }


    [Test]
    public void GetStatistics_IsEmpty_AfterCreation()
    {
        wordsStatistics.GetStatistics().Should().BeEmpty();
    }

    [Test]
    public void GetStatistics_ContainsItem_AfterAddition()
    {
        wordsStatistics.AddWord("abc");
        wordsStatistics.GetStatistics().Should().Equal(new WordCount("abc", 1));
    }

    [Test]
    public void GetStatistics_ContainsManyItems_AfterAdditionOfDifferentWords()
    {
        wordsStatistics.AddWord("abc");
        wordsStatistics.AddWord("def");
        wordsStatistics.GetStatistics().Should().HaveCount(2);
    }

    [Test]
    public void GetStatistics_ThrowsArgumentNullException_AfterAdditionOfNullWord()
    {
        Action addNullWord = () => wordsStatistics.AddWord(null);
        addNullWord.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void GetStatistics_AddsOnlyTenSymbolsForLongWord()
    {
        wordsStatistics.AddWord("longlonglonglong");
        wordsStatistics.GetStatistics().First().Word.Length.Should().Be(10);
    }

    [Test]
    public void GetStatistics_AddsFirstTenSymbolsForLongWord()
    {
        var testWord = "longlonglonglong";
        var expected = testWord.Substring(0, 10);

        wordsStatistics.AddWord(testWord);
        wordsStatistics.GetStatistics().First().Word.Should().Be(expected);
    }

    [Test]
    public void GetStatistics_AddsShortWordCorrectly()
    {
        var testWord = "short";

        wordsStatistics.AddWord(testWord);
        wordsStatistics.GetStatistics().Should().Equal(new WordCount("short", 1));
    }

    [Test]
    public void GetStatistics_KeepsWordLowerCase()
    {
        var testWord = "WORD";

        wordsStatistics.AddWord(testWord);
        wordsStatistics.GetStatistics().Should().Equal(new WordCount(testWord.ToLower(), 1));
    }

    [Test]
    public void GetStatistics_ReturnsWordCountCorrectly()
    {
        var word = "someword";
        wordsStatistics.AddWord(word);
        var pair = wordsStatistics
            .GetStatistics()
            .Where(p => p.Word == word)
            .First();
        pair.Count.Should().Be(1);
    }

    [Test]
    public void GetStatistics_IncrementsCorrectly()
    {
        var word = "someword";
        wordsStatistics.AddWord(word);
        var pair = wordsStatistics
            .GetStatistics()
            .Where(p => p.Word == word)
            .First();
        pair.Count.Should().Be(1);
        wordsStatistics.AddWord(word);
        pair = wordsStatistics
            .GetStatistics()
            .Where(p => p.Word == word)
            .First();
        pair.Count.Should().Be(2);
    }

    [Test]
    public void GetStatistics_DoesNotAddWhitespaceWord()
    {
        var word = "   ";
        wordsStatistics.AddWord(word);
        wordsStatistics.GetStatistics().Should().BeEmpty();
    }

    public static IEnumerable<TestCaseData> SortTestCases()
    {
        // simple case
        yield return new TestCaseData(
            new[] {"a", "b"},
            new[] {"a", "b"}
        );

        // register does not matter case
        yield return new TestCaseData(
            new[] { "a", "B" },
            new[] { "a", "b" }
        );

        // simple word amount test
        yield return new TestCaseData(
            new[] { "a", "a", "B" },
            new[] { "a", "b" }
        );

        // register does not matter test if amount is greater
        yield return new TestCaseData(
            new[] { "a", "A", "a", "b", "b" },
            new[] { "a", "b" }
        );

        // amount greater simple test 
        yield return new TestCaseData(
            new[] { "a", "A", "a", "b", "b", "b", "b", "b" },
            new[] { "b", "a" }
        );

        
    }

    [TestCaseSource(nameof(SortTestCases))]
    public void GetStatistics_SortsCorrectly(string[] testData, string[] expectedSorted)
    { 
        foreach (var testCase in testData) wordsStatistics.AddWord(testCase);
        var pairUnmatched = false;
        foreach (var pair in wordsStatistics.GetStatistics().Zip(expectedSorted))
        {
            if (pair.First.Word != pair.Second)
            {
                pairUnmatched = true;
                break;
            }
        }
        pairUnmatched.Should().BeFalse();
    }
}