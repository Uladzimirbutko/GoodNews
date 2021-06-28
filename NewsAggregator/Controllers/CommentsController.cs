using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.Models.ViewModels.Comment;
using Serilog;

namespace NewsAggregator.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;

        public CommentsController(ICommentService commentService,
            IUserService userService)
        {
            _commentService = commentService;
            _userService = userService;
        }

        public async Task<IActionResult> List(Guid newsId)
        {
            try
            {
                var comments = await _commentService.GetCommentsByNewsId(newsId);
                var userEmail = _commentService.GetCommentsByNewsId(newsId);
                return View(new CommentsListViewModel
                {
                    NewsId = newsId,
                    Comments = comments
                });
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return BadRequest();
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentViewModel model)
        {
            try
            {
                var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimsIdentity.DefaultNameClaimType));
                var userEmail = user?.Value;
                var userId = (await _userService.GetUserByEmail(userEmail)).Id;

                var commentDto = new CommentDto()
                {
                    Id = Guid.NewGuid(),
                    NewsId = model.NewsId,
                    Text = model.CommentText,
                    PublicationDate = DateTime.Now,
                    UserId = userId,
                    UserEmail = userEmail
                };
                await _commentService.Add(commentDto);
                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return BadRequest();
            }
        }
    }
}
