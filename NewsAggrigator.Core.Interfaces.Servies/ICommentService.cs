using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.Core.Services.Interfaces
{
    public interface ICommentService
    {
        Task<CommentDto> GetCommentById(Guid commentId);
        Task<IEnumerable<CommentDto>> GetCommentsByNewsId(Guid newsId);

        Task<int> Add(CommentDto comment);

        Task<int> Edit(CommentDto comment);

        Task<int> Delete(Guid commentId);
    }
}