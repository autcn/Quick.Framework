namespace MessageDemo
{
    public class SearchFinishedMessage
    {
        public SearchFinishedMessage(string keyword)
        {
            Keyword = keyword;
        }

        public string Keyword { get; }
    }
}
