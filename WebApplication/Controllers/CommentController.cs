using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Interfaces.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;

        public CommentController(ICommentService commentService, IUserService userService)
        {
            _commentService = commentService;
            _userService = userService;
        }
        // GET: api/<CommentController>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var comment = await _commentService.GetCommentById(id);
                if (comment == null)
                {
                    var getCommentByNews = await _commentService.GetCommentsByNewsId(id);
                    if (getCommentByNews == null)
                    {
                        return BadRequest("There is no such Id in the database");
                    }
                    return Ok(getCommentByNews);
                }
                return Ok(comment);
            }
            catch (Exception e)
            {
                return BadRequest($"Error {e.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CommentDto comment)
        {
            try
            {
                comment.Id = Guid.NewGuid();
                comment.UserEmail = HttpContext.User.Claims
                    .FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))
                    ?.Value;
                comment.PublicationDate = DateTime.Now;
                var userDto = await _userService.GetUserByEmail(comment.UserEmail);
                comment.UserId = userDto.Id;
                var res = await _commentService.Add(comment);
                if (res == 0)
                {
                    return BadRequest("Comment doesn't added");

                }
                return Ok(comment);
            }
            catch (Exception e)
            {
                return BadRequest($"Error {e.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(CommentDto comment)
        {
            try
            {

                var res = await _commentService.Edit(comment);
                if (res == 0)
                {
                    return BadRequest("Comment doesn't Edit");

                }
                return Ok("Comment succeeded edited");
            }
            catch (Exception e)
            {
                return BadRequest($"Error {e.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var res = await _commentService.Delete(id);
                if (res == 0)
                {
                    return BadRequest("Comment doesn't Deleted");

                }
                return Ok("Comment succeeded deleted");
            }
            catch (Exception e)
            {
                return BadRequest($"Error {e.Message}");
            }
        }



    }
}
