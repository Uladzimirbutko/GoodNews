using System;
using System.Collections.Generic;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.Core.DataTransferObjects
{
    public class NewsDto // паттерн DTO
        /* использование где бы то нибыло entities плохо для отправки данных после покидания базы мы
        начинаем использовать dto которые не используют lazy loading, include и тд.не имеют отношения
        к базе, а просто модели которые предназначены для транспортировки данных*/
    {
        public Guid Id { get; set; } //PK auto is all ended *Id (NewsId)
        public string Article { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public string Summary { get; set; }
        public float Rating { get; set; }
        public string? Category { get; set; }
        public DateTime? PublicationDate { get; set; }

        public Guid RssSourceId { get; set; } //FK
        public RssSource RssSource { get; set; }
    }
}
