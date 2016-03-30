using System;
using System.Linq;

namespace Yatzhee
{
    public class Category
    {
        private readonly int _match;

        public static Category Ones = new Category(1); 
        public static Category Twos = new Category(2);

        private Category(int match)
        {
            _match = match;
        }

        public int Score(string roll)
        {
            return roll.Count(ch => ch == AsciiValue(_match)) * _match;
        }

        private static int AsciiValue(int v)
        {
            return v + '0';
        }
    }
}