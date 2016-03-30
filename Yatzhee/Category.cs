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

        public abstract int Score(string roll);
    }

    internal class Sum : Category
    {
        private readonly int _match;

        protected internal Sum(int match)
        {
            _match = match;
        }

        public override int Score(string roll)
        {
            return roll.Count(ch => ch == AsciiValue(_match)) * _match;
        }

        private static int AsciiValue(int v)
        {
            return v + '0';
        }
    }

    internal class Count : Category
    {
        private readonly int _count;

        protected internal Count(int count)
        {
            _count = count;
        }

        public override int Score(string roll)
        {
            var highest = 
                roll.Split(',')
                .GroupBy(c => c)
                .Where(grp => grp.Count() >= _count)
                .OrderByDescending(grp => grp.Key)
                .FirstOrDefault();

            if (highest == null)
                return 0;

            return int.Parse(highest.Key) * _count;
        }
    }
}