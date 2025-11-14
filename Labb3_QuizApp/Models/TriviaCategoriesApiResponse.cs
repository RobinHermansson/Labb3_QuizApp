public class TriviaCategory
{
    public int id { get; set; }
    public string name { get; set; }
}

public class TriviaCategoriesResponse
{
    public List<TriviaCategory> trivia_categories { get; set; }
}