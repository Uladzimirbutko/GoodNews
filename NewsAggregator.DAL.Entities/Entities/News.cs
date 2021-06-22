using System;
using System.Collections.Generic;

namespace NewsAggregator.DAL.Core.Entities
{
    public sealed class News : IBaseEntity
    {
        // [Key] // next prop is doing PK
        public Guid Id { get; set; } //PK auto is all ended *Id (NewsId)
        public string Article { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public string Summary { get; set; }
        public string? TitleImage { get; set; }
        public string? Category { get; set; }
        public float Rating { get; set; }
        public DateTime? PublicationDate { get; set; }

        public Guid RssSourceId { get; set; } //FK
        public RssSource RssSource { get; set; } // для сопоставления таблиц

        public ICollection<Comment> Comments { get; set; }

    }
}
