﻿using System;

namespace NewsAggregator.DAL.Core.Entities
{
    public sealed class Comment : IBaseEntity
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime PublicationDate { get; set; }



        public Guid NewsId { get; set; } 
        public News News { get; set; }

        public Guid UserId { get; set; } 
        public string UserEmail { get; set; }
        public User User { get; set; }


    }
}