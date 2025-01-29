using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api")]
    /*[Route("api/[controller]")]*/
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DBContext _context;

        public UsersController(DBContext context)
        {
            _context = context;
        }

        // GET: api/auth/:id
        [HttpGet("auth/:id")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/auth/signup
        [HttpPost("signup")]
        public async Task<ActionResult<User>> SignUp(UserHelper userHelper)
        {
            var user = new User
            {
                Username = userHelper.Username,
                Password = userHelper.Password,
                Email = userHelper.Email
            };
            _context.Users.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.Id))
                {
                    return Conflict("User with this ID already exists.");
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        // POST: api/auth/login
        [HttpPost("auth/login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(new { Message = "Login successful" });
        }

        // GET: api/auth/logout
        [HttpGet("auth/logout")]
        public IActionResult Logout()
        {
            return Ok("User logged out successfully.");
        }

    }
}
