using System;
using System.Collections.Generic;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.Models.ViewModels.Comment
{
    public class CommentsListViewModel
    {
        public Guid NewsId { get; set; }
        public IEnumerable<CommentDto> Comments { get; set; }
    }
}