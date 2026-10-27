namespace TDD.Task;

class Frame
{
    public int? FirstPins { get; set; }
    public int? SecondPins { get; set; }

    public int? Sum() => FirstPins != null ? FirstPins : 0 + SecondPins != null ? SecondPins : 0;
}

public class Game
{
    private List<int> scores = new List<int>();
    private List<Frame> frames = [];

    private bool spareBonusActive = false;
    private int totalScore = 0;
    private int currentHitNumber = 0;

    public void Roll(int pins)
    {
        //currentHitNumber++;
        //totalScore += pins;

        //if (scores.Count == 0)
        //{
        //    scores.Add(pins);
        //    return;
        //}
        //if (currentHitNumber % 2 == 0)
        //{

        //}


        totalScore += pins;

        if (frames.Count == 0)
        {
            frames.Add(new Frame() { FirstPins = pins });
            return;
        }
        var lastFrame = frames[frames.Count - 1];
        
        // первый бросок в фрейме
        if (lastFrame.SecondPins != null)
        {
            var newFrame = new Frame() { FirstPins = pins };
            if (pins == 10)
            {
                // ...
            }
            else
            {

            }
        }

        // второй бросок в фрейме
        
        
        if (spareBonusActive)
        {
            totalScore += pins;
            spareBonusActive = false;
        }

        
    }

    public int GetScore()
    {
        //if (scores.Count == 0) throw new IndexOutOfRangeException("бросков еще не было");
        //return scores[scores.Count - 1];
        return totalScore;
    }
}


