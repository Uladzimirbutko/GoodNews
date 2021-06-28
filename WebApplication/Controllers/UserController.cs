using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using Serilog;



namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromBody]UserDto user)   
        {
            try
            {
                if (string.IsNullOrEmpty(user.Email))
                {
                    var getAll = await _userService.GetAllUsers();
                    return Ok(getAll);
                }

                var userByEmail = await _userService.GetUserByEmail(user.Email);
                return Ok(userByEmail);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return NotFound(e.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var user = await _userService.GetUserById(id);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return NotFound();
            }
        }
    }
}
