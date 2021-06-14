using System;
using System.Collections.Generic;

namespace NewsAggregator.DAL.Core.Entities
{
    public  class RssSource : IBaseEntity
    {
        public Guid Id { get; set; }
        public string SourceName { get; set; }
        public string Url { get; set; }

        public ICollection<News> NewsCollection { get; set; }

    }
}