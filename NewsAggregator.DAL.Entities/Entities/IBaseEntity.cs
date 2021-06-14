using System;

namespace NewsAggregator.DAL.Core.Entities
{
    public interface IBaseEntity
    {
         Guid Id { get; set; }

    }
}