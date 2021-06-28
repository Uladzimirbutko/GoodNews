using System;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.Core.DataTransferObjects
{
    public class NewsDto  // паттерн DTO просто модели которые предназначены для транспортировки данных
    {
        public Guid Id { get; set; } 
        public string Article { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public string Summary { get; set; }
        public string? TitleImage { get; set; }
        public string? Category { get; set; }
        public float? Rating { get; set; }
        public DateTime? PublicationDate { get; set; }

        public Guid RssSourceId { get; set; } 
        public RssSource RssSource { get; set; }
    }
}
