using FluentAssertions;
using NUnit.Framework;

namespace Yatzhee
{
    [TestFixture]
    public class YatzheeScorerTest
    {
        private YatzheeScorer _scorer;

        [SetUp]
        public void SetUp()
        {
            _scorer = new YatzheeScorer(Category.Categories);
        }

        [TestCase("1,2,3,4,5", 1)]
        [TestCase("2,2,3,4,5", 0)]
        [TestCase("1,1,1,4,5", 3)]
        public void ScoreOnes(string roll, int expectedScore)
        {
            var score = _scorer.Score(roll, Category.Ones);
            score.Should().Be(expectedScore); 
        }

        [TestCase("1,2,3,4,5", 2)]
        [TestCase("2,2,3,4,5", 4)]
        [TestCase("1,1,1,4,5", 0)]
        public void ScoreTwos(string roll, int expectedScore)
        {
            var score = _scorer.Score(roll, Category.Twos);
            score.Should().Be(expectedScore);
        }

        [TestCase("1,2,3,4,5", 3)]
        [TestCase("2,3,3,3,5", 9)]
        [TestCase("1,1,1,4,5", 0)]
        public void ScoreThrees(string roll, int expectedScore)
        {
            var score = _scorer.Score(roll, Category.Threes);
            score.Should().Be(expectedScore);
        }

        [TestCase("1,2,3,4,5", 4)]
        [TestCase("2,3,3,3,5", 0)]
        [TestCase("1,4,4,4,4", 16)]
        public void ScoreFours(string roll, int expectedScore)
        {
            var score = _scorer.Score(roll, Category.Fours);
            score.Should().Be(expectedScore);
        }

        [TestCase("1,2,3,4,5", 5)]
        [TestCase("2,3,3,3,1", 0)]
        [TestCase("5,5,4,5,4", 15)]
        public void ScoreFives(string roll, int expectedScore)
        {
            var score = _scorer.Score(roll, Category.Fives);
            score.Should().Be(expectedScore);
        }

        [TestCase("1,2,3,4,5", 0)]
        [TestCase("2,3,3,3,1", 6)]
        [TestCase("5,5,4,5,4", 10)]
        public void ScoreOnePair(string roll, int expectedScore)
        {
            var score = _scorer.Score(roll, Category.OnePair);
            score.Should().Be(expectedScore);
        }

        [TestCase("1,2,3,4,5", 0)]
        [TestCase("2,3,3,3,2", 10)]
        [TestCase("5,5,4,5,4", 18)]
        [TestCase("5,5,5,5,5", 20)]
        public void ScoreTwoPairs(string roll, int expectedScore)
        {
            var score = _scorer.Score(roll, Category.TwoPairs);
            score.Should().Be(expectedScore);
        }

        [TestCase("1,2,3,4,5", 0)]
        [TestCase("2,3,3,3,2", 9)]
        [TestCase("5,5,4,5,4", 15)]
        [TestCase("4,4,4,5,5", 12)]
        public void ScoreThreeOfAKind(string roll, int expectedScore)
        {
            var score = _scorer.Score(roll, Category.ThreeOfAKind);
            score.Should().Be(expectedScore);
        }

        [TestCase("1,2,3,4,5", 0)]
        [TestCase("2,3,3,3,3", 12)]
        [TestCase("5,5,4,5,4", 0)]
        [TestCase("4,4,4,4,4", 16)]
        public void ScoreFourOfAKind(string roll, int expectedScore)
        {
            var score = _scorer.Score(roll, Category.FourOfAKind);
            score.Should().Be(expectedScore);
        }

        [TestCase("1,2,3,4,6", 0)]
        [TestCase("1,2,3,4,5", 15)]
        [TestCase("2,3,4,5,6", 0)]
        public void ScoreSmallStraight(string roll, int expectedScore)
        {
            var score = _scorer.Score(roll, Category.SmallStraight);
            score.Should().Be(expectedScore);
        }

        [TestCase("1,2,3,4,6", 0)]
        [TestCase("1,2,3,4,5", 0)]
        [TestCase("2,3,4,5,6", 20)]
        public void ScoreLargeStraight(string roll, int expectedScore)
        {
            var score = _scorer.Score(roll, Category.LargeStraight);
            score.Should().Be(expectedScore);
        }

        [TestCase("1,2,3,4,6", 0)]
        [TestCase("1,1,3,3,3", 11)]
        [TestCase("5,6,5,6,6", 28)]
        public void ScoreFullHouse(string roll, int expectedScore)
        {
            var score = _scorer.Score(roll, Category.FullHouse);
            score.Should().Be(expectedScore);
        }

        [TestCase("1,2,3,4,6", 16)]
        [TestCase("1,1,3,3,3", 11)]
        [TestCase("5,6,5,6,6", 28)]
        [TestCase("1,1,1,1,1", 5)]
        public void ScoreChance(string roll, int expectedScore)
        {
            var score = _scorer.Score(roll, Category.Chance);
            score.Should().Be(expectedScore);
        }

        [TestCase("1,2,3,4,6", 0)]
        [TestCase("1,1,3,3,3", 0)]
        [TestCase("5,6,5,6,6", 0)]
        [TestCase("1,1,1,1,1", 50)]
        public void ScoreYatzhee(string roll, int expectedScore)
        {
            var score = _scorer.Score(roll, Category.Yatzhee);
            score.Should().Be(expectedScore);
        }

        [Test]
        public void FindBestScore() 
        {
            //Excluding chance from this test to make it more sensible. Chance will not always be an option while playing a real game.
            var scorer = new YatzheeScorer(new[] {
                Category.Ones, Category.Twos,Category.Threes, Category.Fours, Category.Fives, Category.Sixes,
                Category.OnePair, Category.TwoPairs, Category.ThreeOfAKind, Category.FourOfAKind,
                Category.SmallStraight, Category.LargeStraight, Category.FullHouse, Category.Yatzhee });

            var yatzhee = scorer.MaxScore("1,1,1,1,1");
            yatzhee.Item1.Should().Be(50);
            yatzhee.Item2.Should().Be(Category.Yatzhee);

            var house = scorer.MaxScore("1,1,6,6,6");
            house.Item1.Should().Be(20);
            house.Item2.Should().Be(Category.FullHouse);

            var sixes = scorer.MaxScore("1,2,6,6,6");
            sixes.Item1.Should().Be(18);
            //This could equally well be three of a kind, but I have chosen to pick the first in the list
            sixes.Item2.Should().Be(Category.Sixes);

        }
    }
}