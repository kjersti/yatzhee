using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
        public static Category TwoPairs = new Combine(new List<Category>() { OnePair, OnePair });
        public static Category ThreeOfAKind = new Count(3);
        public static Category FourOfAKind = new Count(4);
        public static Category SmallStraight = new DiceMatch(new [] {1,2,3,4,5});
        public static Category LargeStraight = new DiceMatch(new[] { 2,3,4,5,6 });
        public static Category FullHouse = new Combine(new List<Category>() { ThreeOfAKind, OnePair });
        public static Category Chance = new Combine(new List<Category>() { Ones, Twos, Threes, Fours, Fives, Sixes });

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
            return new Tuple<int, List<string>>(count * _match, Enumerable.Repeat(_match.ToString(), count).ToList());
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

    internal class DiceMatch : Category
    {
        private readonly int[] _values;

        protected internal DiceMatch(int[] values)
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

    internal class Combine : Category
    {
        private readonly List<Category> _categories;

        protected internal Combine(List<Category> categories)
        {
            _categories = categories;
        }

        public override Tuple<int, List<string>> Score(List<string> roll)
        {
            return _categories.Aggregate(new Tuple<int, List<string>>(0, new List<string>()),
                (score, c) =>
                {
                    var newScore = c.Score(Subtract(roll, score.Item2));
                    return new Tuple<int, List<string>>(score.Item1 + newScore.Item1, score.Item2.Concat(newScore.Item2).ToList());
                });
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