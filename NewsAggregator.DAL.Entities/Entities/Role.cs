using System;
using System.Collections.Generic;

namespace NewsAggregator.DAL.Core.Entities
{
    public class Role : IBaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<User> UserCollection { get; set; }
    }
}