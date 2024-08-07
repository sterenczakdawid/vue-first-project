using backend.Data;
using backend.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
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

    //[EnableCors("AllowAllOrigins")]
    //[HttpGet]
    //public async Task<ActionResult<List<MyTask>>> GetAllTasks(int page = 1, int pageSize = 10)
    //{
    //  //var tasks = await _context.Tasks.ToListAsync();
    //  var tasks = await _context.Tasks.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    //  var totalTasks = await _context.Tasks.CountAsync();

    //  Response.Headers.Add("X-Total-Count", totalTasks.ToString());

    //  return Ok(tasks);
    //}

    [EnableCors("AllowAllOrigins")]
    [HttpGet]
    public async Task<ActionResult<List<MyTask>>> GetAllTasks(int page = 1, int pageSize = 5, int? serverId = null, int? appId = null, string search = "", string sortBy = "", bool? sortDesc = false)
    {
      var query = _context.Tasks.AsQueryable();

      if (serverId.HasValue)
      {
        query = query.Where(t => t.ServerId == serverId);
      }

      if (appId.HasValue)
      {
        query = query.Where(t => t.AppId == appId);
      }

      if (!string.IsNullOrEmpty(search))
      {
        query = query.Where(t => t.Name.Contains(search));
      }

      // Sortowanie
      if (!string.IsNullOrEmpty(sortBy))
      {
        if (sortDesc.GetValueOrDefault())
        {
          query = query.OrderByDescending(e => EF.Property<object>(e, sortBy));
        }
        else
        {
          query = query.OrderBy(e => EF.Property<object>(e, sortBy));
        }
      }

      var totalTasks = await query.CountAsync();
      List<MyTask> tasks;
      if (pageSize <= 0)
      {
        tasks = await query.ToListAsync();
      }
      else
      {
        tasks = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
      }

      // Dodawanie nagłówka z całkowitą liczbą zadań
      Response.Headers.Add("X-Total-Count", totalTasks.ToString());

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
      var existingTask = await _context.Tasks.FirstOrDefaultAsync(t => t.Name == task.Name);
      if (existingTask != null)
      {
        return BadRequest("Task with the same name already exists");
      }
      if (task.AppId is null)
      {
        task.AppId = 0;
      }

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
      var existingTask = await _context.Tasks.FirstOrDefaultAsync(t => t.Name == updatedTask.Name);
      if (existingTask != null)
      {
        return BadRequest("Task with the same name already exists");
      }

      dbTask.Name = updatedTask.Name;
      dbTask.Edited = updatedTask.Edited;
      if(updatedTask.AppId is null)
      {
        dbTask.AppId = 0;
      } else
      {
        dbTask.AppId = updatedTask.AppId;
      }
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

      _context.Tasks.RemoveRange(tasks);
      await _context.SaveChangesAsync();

      return Ok(await _context.Tasks.ToListAsync());
    }
  }
}
