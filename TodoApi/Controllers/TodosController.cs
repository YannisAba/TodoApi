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
    public class TodosController : ControllerBase
    {
        private readonly DBContext _context;

        public TodosController(DBContext context)
        {
            _context = context;
        }

        // GET: api/todos
        [HttpGet("todos")]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            return await _context.Todos.Include(t => t.TodoItems).ToListAsync();
        }

        // GET: api/todos/{id}
        [HttpGet("todos/{id:int}")]
        public async Task<ActionResult<Todo>> GetTodo(int id)
        {
            /*var todo = await _context.Todos.FindAsync(id);*/

            var todo = await _context.Todos
                .Include(t => t.TodoItems)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (todo == null)
            {
                return NotFound();
            }

            return todo;
        }

        // PUT: api/todos/{id}
        [HttpPut("todos/{id:int}")]
        public async Task<IActionResult> PutTodo(int id, ToDoHelperClass toDoHelperClass)
        {
            var user = await _context.Users.FindAsync(toDoHelperClass.UserId);
            if (user == null)
            {
                return BadRequest("Invalid UserId: User not found.");
            }
            var todo = new Todo
            {
                Id = id,
                Name = toDoHelperClass.Name,
                UserId = toDoHelperClass.UserId
            };

            _context.Entry(todo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
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

        // POST: api/todos
        [HttpPost("todos")]
        public async Task<ActionResult<Todo>> PostTodo(ToDoHelperClass toDoHelperClass)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (toDoHelperClass.UserId != null)
            {
                var user = await _context.Users.FindAsync(toDoHelperClass.UserId);
                if (user == null)
                {
                    return BadRequest("Invalid UserId: User not found.");
                }
                var todo = new Todo
                {
                    Name = toDoHelperClass.Name,
                    UserId = toDoHelperClass.UserId,
                    User = user
                };
                _context.Todos.Add(todo);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/todos/{id}
        [HttpDelete("todos/{id:int}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var todo = await _context.Todos.Include(t => t.TodoItems).FirstOrDefaultAsync(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.TodoItems.RemoveRange(todo.TodoItems);
            if (todo == null)
            {
                return NotFound();
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoExists(int id)
        {
            return _context.Todos.Any(e => e.Id == id);
        }
    }
}
