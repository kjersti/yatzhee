using System;
using System.Collections.Generic;
using System.Linq;

namespace Yatzhee
{
    public abstract class Category
    {
        public static Category Ones = new Sum(1);
        public static Category Twos = new Sum(2);
        public static Category Threes = new Sum(3);
        public static Category Fours = new Sum(4);
        public static Category Fives = new Sum(5);
        public static Category Sixes = new Sum(6);

        public static Category OnePair = new Count(2);
        public static Category TwoPairs = new Combine(new List<Category> { OnePair, OnePair }, true);
        public static Category ThreeOfAKind = new Count(3);
        public static Category FourOfAKind = new Count(4);
        public static Category SmallStraight = new Match(new [] {1,2,3,4,5});
        public static Category LargeStraight = new Match(new[] { 2,3,4,5,6 });
        public static Category FullHouse = new Combine(new List<Category> { ThreeOfAKind, OnePair }, true);
        public static Category Chance = new Combine(new List<Category> { Ones, Twos, Threes, Fours, Fives, Sixes });
        public static Category Yatzhee = new Yatzhee();

        public static Category[] Categories = {
            Ones, Twos,Threes, Fours, Fives, Sixes,
            OnePair, TwoPairs, ThreeOfAKind, FourOfAKind,
            SmallStraight, LargeStraight, FullHouse, 
            Chance, Yatzhee};

        public static string CategoryName(Category category)
        {
            if (category.Equals(Ones)) return "Ones";
            if (category.Equals(Twos)) return "Twos";
            if (category.Equals(Threes)) return "Threes";
            if (category.Equals(Fours)) return "Fours";
            if (category.Equals(Fives)) return "Fives";
            if (category.Equals(Sixes)) return "Sixes";

            if (category.Equals(OnePair)) return "One pair";
            if (category.Equals(TwoPairs)) return "Two pairs";
            if (category.Equals(ThreeOfAKind)) return "Three of a kind";
            if (category.Equals(FourOfAKind)) return "Four of a kind";
            if (category.Equals(SmallStraight)) return "Small straight";
            if (category.Equals(LargeStraight)) return "Large straight";
            if (category.Equals(FullHouse)) return "Full house";
            if (category.Equals(Chance)) return "Chance";
            if (category.Equals(Yatzhee)) return "Yatzhee!";

            throw new Exception("Invalid category!");            
        }

        public abstract Tuple<int, List<string>> Score(List<string> roll);
    }

    internal class Sum : Category
    {
        private readonly int _match;

        protected internal Sum(int match)
        {
            _match = match;
        }

        public override Tuple<int, List<string>> Score(List<string> roll)
        {
            var count = roll.Count(ch => _match == int.Parse(ch));

            return new Tuple<int, List<string>>(
                count * _match, 
                Enumerable.Repeat(_match.ToString(), count).ToList());
        }
    }

    internal class Count : Category
    {
        private readonly int _count;

        protected internal Count(int count)
        {
            _count = count;
        }

        public override Tuple<int, List<string>> Score(List<string> roll)
        {
            var highest = 
                roll.GroupBy(c => c)
                .Where(grp => grp.Count() >= _count)
                .OrderByDescending(grp => grp.Key)
                .FirstOrDefault();

            if (highest == null)
                return new Tuple<int, List<string>>(0, new List<string>());

            return new Tuple<int, List<string>>(int.Parse(highest.Key) * _count, Enumerable.Repeat(highest.Key, _count).ToList());
        }
    }

    internal class Match : Category
    {
        private readonly int[] _values;

        protected internal Match(int[] values)
        {
            _values = values;
        }

        public override Tuple<int, List<string>> Score(List<string> roll)
        {
            var diceValues =
                roll.Select(int.Parse)
                    .OrderBy(v => v)
                    .Take(_values.Length)
                    .ToArray();

            if (!_values.SequenceEqual(diceValues))
                return new Tuple<int, List<string>>(0, new List<string>());

            return new Tuple<int, List<string>>(_values.Sum(), _values.Select(v => v.ToString()).ToList());
        }
    }

    internal class Yatzhee : Category
    {
        public override Tuple<int, List<string>> Score(List<string> roll)
        {
            var countDistinct = new HashSet<string>(roll).Count;

            if (countDistinct != 1)
                return new Tuple<int, List<string>>(0, new List<string>());

            return new Tuple<int, List<string>>(50, roll);
        }
    }

    internal class Combine : Category
    {
        private readonly List<Category> _categories;
        private readonly bool _and;

        protected internal Combine(List<Category> categories, bool and = false)
        {
            _categories = categories;
            _and = and;
        }

        public override Tuple<int, List<string>> Score(List<string> roll)
        {
            var combinedScore = new Tuple<int, List<string>>(0, new List<string>());            
            foreach (var category in _categories)
            {
                var score = category.Score(Subtract(roll, combinedScore.Item2));
                if (_and && score.Item1 == 0)
                {
                    combinedScore = new Tuple<int, List<string>>(0, new List<string>());
                    break;
                }

                combinedScore = new Tuple<int, List<string>>(combinedScore.Item1 + score.Item1, combinedScore.Item2.Concat(score.Item2).ToList());

            }
            return combinedScore;
        }

        private static List<string> Subtract(List<string> one, IEnumerable<string> subtract)
        {
            foreach (var s in subtract)
            {
                one.Remove(s);
            }

            return one;
        }
    }
}