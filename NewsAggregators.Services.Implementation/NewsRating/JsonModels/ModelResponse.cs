namespace NewsAggregator.Services.Implementation.NewsRating.JsonModels
{
    public  class ModelResponse
    {
        public string text { get; set; }
        public LemmasList annotations { get; set; }
    }
}