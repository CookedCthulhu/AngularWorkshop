using AngularWorkshop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace AngularWorkshop.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController : Controller
{
    private static readonly List<Todo> todos = new();
    private static readonly object lck = new();

    [HttpGet]
    public IActionResult GetTodos()
    {
        var result = default(Todo[]);
        lock (lck)
        {
            result = todos.ToArray();
        }
        return Ok(result);
    }

    [HttpPost]
    public IActionResult AddOrUpdateTodo(Todo item)
    {
        if (item is null) return BadRequest();

        lock (lck)
        {
            var idx = todos.FindLastIndex(x => x.Id == item.Id);
            if (idx < 0) todos.Add(item);
            else todos[idx] = item;
        }

        return Ok();
    }

    [HttpDelete]
    public IActionResult DeleteTodo(int id)
    {
        lock (lck)
        {
            var idx = todos.FindLastIndex(x => x.Id == id);
            if (idx >= 0) todos.RemoveAt(idx);
        }

        return Ok();
    }
}
