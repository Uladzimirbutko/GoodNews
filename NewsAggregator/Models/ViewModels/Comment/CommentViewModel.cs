using System;

namespace NewsAggregator.Models.ViewModels.Comment

{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime PublicationDate { get; set; }

    }
}