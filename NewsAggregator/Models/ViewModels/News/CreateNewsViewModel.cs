using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewsAggregator.Models.ViewModels.News
{
    public class CreateNewsViewModel
    {
        public Guid Id { get; set; } //PK auto is all ended *Id (NewsId)
        public string Article { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public float Rating { get; set; }
        public string? Category { get; set; }
        public DateTime? PublicationDate { get; set; }

        public Guid? RssSourceId { get; set; } //FK

        public SelectList Sources { get; set; }
    }
}