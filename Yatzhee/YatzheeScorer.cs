using System;
using System.Linq;

namespace Yatzhee
{
    public class YatzheeScorer
    {
        private readonly Category[] _availableCategories;

        public YatzheeScorer(Category[] availableCategories)
        {
            _availableCategories = availableCategories;
        }

        public int Score(string roll, Category category)
        {
            return category.Score(roll.Split(',').ToList()).Item1;
        }

        public Tuple<int, Category> MaxScore(string roll)
        {
            return _availableCategories
                .Select(c => new Tuple<int, Category>(Score(roll, c), c))
                .OrderByDescending(t => t.Item1)
                .FirstOrDefault();
        }
    }
}
