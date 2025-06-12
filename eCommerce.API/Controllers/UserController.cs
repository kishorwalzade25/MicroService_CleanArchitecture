using eCommerce.Core.DTO;
using eCommerce.Core.Repository;
using eCommerce.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UserController(IUsersService usersService) 
        {
            _usersService = usersService;
        }


        [HttpGet]
        [Route("getUser/{userId:int}")]
        public async Task<IActionResult> get_user(int userId) 
        {
            // for fault tolerance
            //await Task.Delay(100);
            //throw new NotImplementedException();

            if (userId == 0)
            {
                return BadRequest("Invalid User ID");
            }

            UserDTO? response = await _usersService.GetUserByUserID(userId);

            if (response == null)
            {
                return NotFound(response);
            }

            return Ok(response);

        }
    }
}
