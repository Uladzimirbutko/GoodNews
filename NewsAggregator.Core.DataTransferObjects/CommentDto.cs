using System;

namespace NewsAggregator.Core.DataTransferObjects
{
    public class CommentDto : IDtoModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime PublicationDate { get; set; }
        public string UserEmail { get; set; }
        public Guid NewsId { get; set; } //FK
        public Guid UserId { get; set; } //FK
        

    }
}