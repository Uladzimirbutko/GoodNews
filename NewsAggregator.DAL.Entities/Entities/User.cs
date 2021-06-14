﻿using System;
using System.Collections.Generic;

namespace NewsAggregator.DAL.Core.Entities
{
    public class User : IBaseEntity
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int Age { get; set; }
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

    }
}