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

        // GET: api/todos/{id}/items/{iid}
        [HttpGet("todos/{id:int}/items/{iid:int}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int id, int iid)
        {
            var todoItem = await _context.TodoItems.Include(t => t.Todo)
            .FirstOrDefaultAsync(i => i.TodoId == id && i.Id == iid);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/todos/{id}/items/{iid}
        [HttpPut("todos/{id:int}/items/{iid:int}")]
        public async Task<IActionResult> PutTodoItem(int id, int iid, ToDoItemHelperClass toDoItemHelperClass)
        {
            var todoItem = new TodoItem
            {
                Id = iid,
                Name = toDoItemHelperClass.Name,
                IsComplete = toDoItemHelperClass.IsComplete,
                TodoId = id
            };

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(iid) || !_context.Todos.Any(t => t.Id == id))
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

        // POST: api/todos/{id}/items
        [HttpPost("todos/{id:int}/items")]
        public async Task<ActionResult<TodoItem>> PostTodoItem(int id, ToDoItemHelperClass toDoItemHelperClass)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return BadRequest("Invalid id: Todo not found.");
            }
            var todoItem = new TodoItem
            {
                Name = toDoItemHelperClass.Name,
                IsComplete = toDoItemHelperClass.IsComplete,
                TodoId = id,
                Todo = todo
            };
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = id, iid = todoItem.Id }, todoItem);
        }

        // DELETE: api/todos/{id}/items/{iid}
        [HttpDelete("todos/{id:int}/items/{iid:int}")]
        public async Task<IActionResult> DeleteTodoItem(int id, int iid)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

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
