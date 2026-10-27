using FluentAssertions;
using NUnit.Framework;
using TDD.Task;

namespace TDD;

[TestFixture]
public class GameTests
{
    private Game game;

    [SetUp]
    public void SetUp()
    {
        game = new Game();
    }

    [Test]
    public void HaveZeroScore_BeforeAnyRolls()
    {
        new Game()
            .GetScore()
            .Should().Be(0);
    }

    [Test]
    public void HavePositiveScore_AfterAnyRoll()
    {
        game.Roll(5);
        game.GetScore().Should().Be(5);
    }

    [Test]
    public void PinsSumsPerFrame_AfterTwoRoll()
    {
        game.Roll(5);
        game.Roll(3);
        game.GetScore().Should().Be(8);
    }

    [Test]
    public void SpareAddingBonus_AfterRoll()
    {
        game.Roll(5);
        game.Roll(5);
        // current score 10 -> hit spare
        game.Roll(6);
        game.GetScore().Should().Be(5 + 5 + 6 + 6);
    }

    [Test]
    public void SpareAddingBonus_AfterManyRoll()
    {
        game.Roll(1);
        game.Roll(4);
        game.Roll(4);
        game.Roll(5);
        game.Roll(6);
        game.Roll(4);
        game.Roll(5);
        game.GetScore().Should().Be(29 + 5);
    }
}