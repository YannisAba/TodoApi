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
    public class TodoItemsController : ControllerBase
    {
        private readonly DBContext _context;

        public TodoItemsController(DBContext context)
        {
            _context = context;
        }

        // GET: api/todos/:id/items/:iid
        [HttpGet("todos/:id/items/:iid")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int id, int iid)
        {
            /*var todoItem = await _context.TodoItems.FindAsync(id);*/

            var todoItem = await _context.TodoItems.Include(t => t.Todo)
            .FirstOrDefaultAsync(i => i.TodoId == id && i.Id == iid);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/todos/:id/items/:iid
        [HttpPut("todos/:id/items/:iid")]
        public async Task<IActionResult> PutTodoItem(int iid, TodoItem todoItem)
        {
            if (iid != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(iid))
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

        // POST: api/todos/:id/items
        [HttpPost("todos/:id/items")]
        public async Task<ActionResult<TodoItem>> PostTodoItem(ToDoItemHelperClass toDoItemHelperClass)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var todo = await _context.Todos.FindAsync(toDoItemHelperClass.TodoId);
            if (todo == null)
            {
                return BadRequest("Invalid UserId: User not found.");
            }
            var todoItem = new TodoItem
            {
                Name = toDoItemHelperClass.Name,
                IsComplete = toDoItemHelperClass.IsComplete,
                TodoId = toDoItemHelperClass.TodoId,
                Todo = todo
            };
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/todos/:id/items/:iid
        [HttpDelete("todos/:id/items/:iid")]
        public async Task<IActionResult> DeleteTodoItem(int iid)
        {
            var todoItem = await _context.TodoItems.FindAsync(iid);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(int id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
