using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.Core.Services.Interfaces
{
    public interface ICommentService
    {
        Task<CommentDto> GetCommentById(Guid commentId);
        Task<IEnumerable<CommentDto>> GetCommentsByNewsId(Guid newsId);

        Task Add(CommentDto comment);

        Task Edit(CommentDto comment);

        Task Delete(CommentDto comment);
    }
}