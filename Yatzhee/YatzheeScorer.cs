namespace Yatzhee
{
    public class YatzheeScorer
    {
        public int Score(string roll, Category category)
        {
            return category.Score(roll);
        }
    }
}
