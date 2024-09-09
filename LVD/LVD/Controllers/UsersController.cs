using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LVD.Models;
using LVD.Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LVD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ChatDbContext _context;
        private readonly GeneralFunction _generalFunction;

        public UsersController(ChatDbContext context, GeneralFunction generalFunction)
        {
            _context = context;
            _generalFunction = generalFunction;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            var userDtos = users.Select(user => new UserDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Status = user.Status,
                Type = user.Type
            }).ToList();

            return Ok(userDtos);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Status = user.Status,
                Type = user.Type
            };

            return Ok(userDto);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDTO userDto)
        {
            if (id != userDto.UserId)
            {
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Username = userDto.Username;
            user.ProfilePictureUrl = userDto.ProfilePictureUrl;
            user.Status = userDto.Status;
            user.Type = userDto.Type;

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_generalFunction.IsIdExists(id, "u"))
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

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO userDto)
        {
            var user = new User
            {
                // Generate a unique ID
                UserId = _generalFunction.GenerateUniqueId("u"),
                Username = userDto.Username,
                Password = userDto.Password,
                ProfilePictureUrl = userDto.ProfilePictureUrl,
                Status = userDto.Status,
                Type = userDto.Type
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            userDto.UserId = user.UserId;

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, userDto);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
