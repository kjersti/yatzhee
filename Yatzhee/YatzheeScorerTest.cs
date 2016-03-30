using FluentAssertions;
using NUnit.Framework;

namespace Yatzhee
{
    [TestFixture]
    public class YatzheeScorerTest
    {
        [TestCase("1,2,3,4,5", 1)]
        [TestCase("2,2,3,4,5", 0)]
        public void ScoreOnes(string roll, int expectedScore)
        {
            var score = new YatzheeScorer().Score(roll, Category.Ones);
            score.Should().Be(expectedScore);
        }
    }

    public enum Category
    {
        Ones
    }
}