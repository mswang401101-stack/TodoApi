using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoService _todoService;

        public TodoItemsController(TodoService todoService)
        {
            _todoService = todoService;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<List<TodoItem>> Get() =>
            await _todoService.GetAsync();

        // GET: api/TodoItems/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> Get(string id)
        {
            var todoItem = await _todoService.GetAsync(id);

            if (todoItem is null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // POST: api/TodoItems
        [HttpPost]
        public async Task<IActionResult> Post(TodoItem newItem)
        {
            await _todoService.CreateAsync(newItem);
            return CreatedAtAction(nameof(Get), new { id = newItem.Id }, newItem);
        }

        // PUT: api/TodoItems/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, TodoItem updatedItem)
        {
            var todoItem = await _todoService.GetAsync(id);

            if (todoItem is null)
            {
                return NotFound();
            }

            updatedItem.Id = todoItem.Id; // 確保 ID 不變
            await _todoService.UpdateAsync(id, updatedItem);

            return NoContent();
        }

        // DELETE: api/TodoItems/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var todoItem = await _todoService.GetAsync(id);

            if (todoItem is null)
            {
                return NotFound();
            }

            await _todoService.RemoveAsync(id);

            return NoContent();
        }
    }
}