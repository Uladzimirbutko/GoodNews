﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Implementation;
using Serilog;

namespace NewsAggregator.Services.Implementation.Services
{
    public class CommentService :ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CommentDto> GetCommentById(Guid commentId)
        {
            return _mapper.Map<CommentDto>(await _unitOfWork.Comments
                .GetById(commentId));
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByNewsId(Guid newsId)
        {
            return await _unitOfWork.Comments.FindBy(n => n.NewsId.Equals(newsId))
                .OrderByDescending(time => time.PublicationDate)
                .Select(comment => _mapper.Map<CommentDto>(comment))
                .ToListAsync();
        }

        public async Task Add(CommentDto comment)
        {
            try
            {
                await _unitOfWork.Comments.Add(_mapper.Map<Comment>(comment));

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log.Error($"Add comment failed {e.Message}");
            }

            Log.Information("Add comment completed");
        }

        public async Task Edit(CommentDto comment)
        {
            try
            {
                await _unitOfWork.Comments.Update(_mapper.Map<Comment>(comment));

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log.Error($"Edit comment failed {e.Message}");
            }

            Log.Information("Edit comment completed");
        }

        public async Task Delete(CommentDto comment)
        {
            try
            {
                await _unitOfWork.Comments.Remove(await _unitOfWork.Comments.GetById(comment.Id));

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log.Error($"Delete comment failed {e.Message}");
            }

            Log.Information("Delete comment completed");
        }
    }
}