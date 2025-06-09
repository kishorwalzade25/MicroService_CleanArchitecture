using eCommerce.Core.DTO;
using eCommerce.Core.Entities;
using eCommerce.Core.Repository;
using eCommerce.Infrastructure.eCommerceDbContext;
using eCommerce.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersController : ControllerBase
    {
        private readonly eCommDbContext _context;
        private readonly IUsersRepository _usersRepository;
        private readonly IUsersService _usersService;

        public ApplicationUsersController(eCommDbContext context, IUsersRepository usersRepository,IUsersService usersService)
        {
            _context = context;
            _usersRepository = usersRepository;
            _usersService = usersService;
        }

        // GET: api/ApplicationUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetApplicationUsers()
        {
            return await _context.ApplicationUsers.ToListAsync();
        }

        // GET: api/ApplicationUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetApplicationUser(int id)
        {
            var applicationUser = await _context.ApplicationUsers.FindAsync(id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            return applicationUser;
        }

        // PUT: api/ApplicationUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplicationUser(int id, ApplicationUser applicationUser)
        {
            if (id != applicationUser.UserID)
            {
                return BadRequest();
            }

            _context.Entry(applicationUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ApplicationUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> PostApplicationUser(ApplicationUser applicationUser)
        {
            //_context.ApplicationUsers.Add(applicationUser);
            //await _context.SaveChangesAsync();

            var user = await _usersRepository.AddUser(applicationUser);
            if (user == null) { return BadRequest(); }
            return Ok(user);

            //return CreatedAtAction("GetApplicationUser", new { id = applicationUser.UserID }, applicationUser);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> login([FromBody] LoginRequest loginRequest ) 
        {
            var userResponse= await _usersService.Login(loginRequest);
            if (userResponse == null) { return Unauthorized(); }
            return Ok(userResponse);
        }





        // DELETE: api/ApplicationUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicationUser(int id)
        {
            var applicationUser = await _context.ApplicationUsers.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            _context.ApplicationUsers.Remove(applicationUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApplicationUserExists(int id)
        {
            return _context.ApplicationUsers.Any(e => e.UserID == id);
        }
    }
}
