using System.Linq;

namespace Yatzhee
{
    public class YatzheeScorer
    {
        public int Score(string roll, Category category)
        {
            return roll.Count(c => c == '1');
        }
    }
}
