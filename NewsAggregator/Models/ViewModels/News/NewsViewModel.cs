using System;

namespace NewsAggregator.Models.ViewModels.News
{
    public class NewsViewModel
    {
        public Guid Id { get; set; } //PK auto is all ended *Id (NewsId)
        public string Article { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public float Rating { get; set; }
        public string? Category { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string RssSourceName { get; set; }
        
    }
}