using backend.Data;
using backend.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace backend.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TaskController : ControllerBase
  {
    private readonly DataContext _context;
    public TaskController(DataContext context)
    {
      _context = context;
    }

    [EnableCors("AllowAllOrigins")]
    [HttpGet]
    public async Task<ActionResult<List<MyTask>>> GetAllTasks()
    {
      var tasks = await _context.Tasks.ToListAsync();

      return Ok(tasks);
    }

    [EnableCors("AllowAllOrigins")]
    [HttpGet("{id}")]
    public async Task<ActionResult<MyTask>> GetTask(int id)
    {
      var task = await _context.Tasks.FindAsync(id);
      if (task is null)
      {
        return BadRequest("Task not found");
      }

      return Ok(task);
    }

    [EnableCors("AllowAllOrigins")]
    [HttpPost]
    public async Task<ActionResult<List<MyTask>>> AddTask(MyTask task)
    {
      _context.Tasks.Add(task);
      await _context.SaveChangesAsync();

      return Ok(await _context.Tasks.ToListAsync());
    }

    [EnableCors("AllowAllOrigins")]
    [HttpPut]
    public async Task<ActionResult<List<MyTask>>> UpdateTask(MyTask updatedTask)
    {
      var dbTask = await _context.Tasks.FindAsync(updatedTask.Id);
      if (dbTask is null)
      {
        return BadRequest("Task not found");
      }

      dbTask.Name = updatedTask.Name;
      dbTask.Edited = updatedTask.Edited;
      dbTask.AppId = updatedTask.AppId;
      dbTask.ServerId = updatedTask.ServerId;

      await _context.SaveChangesAsync();

      return Ok(await _context.Tasks.ToListAsync());
    }

    [EnableCors("AllowAllOrigins")]
    [HttpDelete]
    public async Task<ActionResult<List<MyTask>>> DeleteTask(int id)
    {
      var dbTask = await _context.Tasks.FindAsync(id);
      if (dbTask is null)
      {
        return BadRequest("Task not found");
      }

      _context.Tasks.Remove(dbTask);
      await _context.SaveChangesAsync();

      return Ok(await _context.Tasks.ToListAsync());
    }

    [EnableCors("AllowAllOrigins")]
    [HttpDelete("server/{serverId}")]
    public async Task<ActionResult<List<MyTask>>> RemoveServerTasks(int serverId) {
      var tasks = await _context.Tasks.Where(t => t.ServerId == serverId).ToListAsync();
      if (tasks.Count == 0)
      {
        return BadRequest("No tasks found for the given server.");
      }

      _context.Tasks.RemoveRange(tasks);
      await _context.SaveChangesAsync();

      return Ok(await _context.Tasks.ToListAsync());
    }
  }
}
