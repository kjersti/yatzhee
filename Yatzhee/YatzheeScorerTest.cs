using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Yatzhee
{
    [TestFixture]
    public class YatzheeScorerTest
    {
        [TestCase("1,2,3,4,5", 1)]
        [TestCase("2,2,3,4,5", 0)]
        [TestCase("1,1,1,4,5", 3)]
        public void ScoreOnes(string roll, int expectedScore)
        {
            var score = new YatzheeScorer().Score(roll, Category.Ones);
            score.Should().Be(expectedScore); 
        }

        [TestCase("1,2,3,4,5", 2)]
        [TestCase("2,2,3,4,5", 4)]
        [TestCase("1,1,1,4,5", 0)]
        public void ScoreTwos(string roll, int expectedScore)
        {
            var score = new YatzheeScorer().Score(roll, Category.Twos);
            score.Should().Be(expectedScore);
        }

        [TestCase("1,2,3,4,5", 3)]
        [TestCase("2,3,3,3,5", 9)]
        [TestCase("1,1,1,4,5", 0)]
        public void ScoreThrees(string roll, int expectedScore)
        {
            var score = new YatzheeScorer().Score(roll, Category.Threes);
            score.Should().Be(expectedScore);
        }

        [TestCase("1,2,3,4,5", 4)]
        [TestCase("2,3,3,3,5", 0)]
        [TestCase("1,4,4,4,4", 16)]
        public void ScoreFours(string roll, int expectedScore)
        {
            var score = new YatzheeScorer().Score(roll, Category.Fours);
            score.Should().Be(expectedScore);
        }

        [TestCase("1,2,3,4,5", 5)]
        [TestCase("2,3,3,3,1", 0)]
        [TestCase("5,5,4,5,4", 15)]
        public void ScoreFives(string roll, int expectedScore)
        {
            var score = new YatzheeScorer().Score(roll, Category.Fives);
            score.Should().Be(expectedScore);
        }

        [TestCase("1,2,3,4,5", 0)]
        [TestCase("2,3,3,3,1", 6)]
        [TestCase("5,5,4,5,4", 10)]
        public void ScoreOnePair(string roll, int expectedScore)
        {
            var score = new YatzheeScorer().Score(roll, Category.OnePair);
            score.Should().Be(expectedScore);
        }
    }
}