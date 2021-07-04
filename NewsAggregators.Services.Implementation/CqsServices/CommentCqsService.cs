using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Interfaces.Services;
using NewsAggregator.DAL.CQRS.Commands.CommentCommands;
using NewsAggregator.DAL.CQRS.Queries.CommentQueries;
using Serilog;

namespace NewsAggregator.Services.Implementation.CqsServices
{
    public class CommentCqsService : ICommentService
    {
        private readonly IMediator _mediator;

        public CommentCqsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CommentDto> GetCommentById(Guid commentId)
        {
            try
            {
                return await _mediator.Send(new GetCommentByIdQuery( commentId));
            }
            catch (Exception e)
            {
                Log.Error($"Error Get Comment By Id {e.Message} {commentId}");
                return null;
            }
        }


        public async Task<IEnumerable<CommentDto>> GetCommentsByNewsId(Guid newsId)
        {
            try
            {
                return await _mediator.Send(new GetCommentsByNewsIdQuery(newsId));
            }
            catch (Exception e)
            {
                Log.Error($"Error GetCommentsByNewsId {e.Message} {newsId}");
                return null;
            }
        }

        public async Task<int> Add(CommentDto comment)
        {
            try
            {
                return await _mediator.Send(new AddCommentCommand(comment));
            }
            catch (Exception e)
            {
                Log.Error($"Error Added comment {e.Message} {comment.Id}");
                return 0;
            }
        }


        public async Task<int> Edit(CommentDto comment)
        {
            try
            {
                var commentDto = await GetCommentById(comment.Id);
                commentDto.Text = comment.Text;
                return await _mediator.Send(new EditCommentCommand(commentDto));
            }
            catch (Exception e)
            {
                Log.Error($"Error Edit Comment  {e.Message} {comment.Id}");
                return 0;
            }
        }

        public async Task<int> Delete(Guid commentId)
        {
            try
            {
                var commentDto = await GetCommentById(commentId);
                return await _mediator.Send(new DeleteCommentCommand(commentDto));
            }
            catch (Exception e)
            {
                Log.Error($"Error Delete Comment {e.Message} {commentId}");
                return 0;
            }
        }
    }
}