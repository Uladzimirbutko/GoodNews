using System;
using System.Collections.Generic;
using NewsAggregator.Core.DataTransferObjects;


namespace NewsAggregator.Models.ViewModels.News
{
    public class NewsWithCommentsViewModel
    {
        public Guid Id { get; set; } //PK auto is all ended *Id (NewsId)
        public string Article { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public string TitleImage { get; set; }
        public string Category { get; set; }
        public float? Rating { get; set; }
        public DateTime? PublicationDate { get; set; }
        public bool IsAdmin { get; set; }


        public IEnumerable<CommentDto> Comments { get; set; }
    }
}