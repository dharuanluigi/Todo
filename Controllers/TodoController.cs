using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Models;

namespace Todo.Controllers
{
  [Route("api/todo")]
  [ApiController]
  public class TodoController : ControllerBase
  {
    [HttpGet]
    public IActionResult Get([FromServices] AppDbContext context)
    {
      var todos = context.Todos.ToList();

      if (todos is null)
      {
        return NotFound();
      }

      return Ok(todos);
    }

    [HttpPost]
    public IActionResult Post([FromBody] TodoModel todo, [FromServices] AppDbContext context)
    {
      context.Todos.Add(todo);
      context.SaveChanges();
      return Created($"/api/todo/{todo.Id}", todo);
    }

    [HttpGet("api/todo/{id:int}")]
    public IActionResult GetById([FromRoute] int Id, [FromServices] AppDbContext context)
    {
      var todo = context.Todos.FirstOrDefault(t => t.Id == Id);

      if (todo is null)
      {
        return NotFound();
      }

      return Ok(todo);
    }

    [HttpDelete("todo/{id:int}")]
    public IActionResult DeleteTodo([FromRoute] int Id, [FromServices] AppDbContext context)
    {
      var todo = context.Todos.FirstOrDefault(t => t.Id == Id);

      if (todo is null)
      {
        return NotFound();
      }

      context.Todos.Remove(todo);
      context.SaveChanges();
      return NoContent();
    }

    [HttpPut("todo/{id:int}")]
    public IActionResult UpdateTodo(
      [FromRoute] int Id,
      [FromBody] TodoModel newTodo,
      [FromServices] AppDbContext context
    )
    {
      var todo = context.Todos.FirstOrDefault(t => t.Id == Id);

      if (todo is null)
      {
        return NotFound();
      }

      todo.Title = newTodo.Title;
      todo.IsDone = newTodo.IsDone;

      context.Todos.Update(todo);
      context.SaveChanges();
      return NoContent();
    }
  }
}