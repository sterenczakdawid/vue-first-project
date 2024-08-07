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
  public class AppController : ControllerBase
  {
    private readonly DataContext _context;
    public AppController(DataContext context)
    {
      _context = context;
    }

    [EnableCors("AllowAllOrigins")]
    [HttpGet]
    public async Task<ActionResult<List<App>>> GetAllApps(int page = 1, int pageSize = 5, int? serverId = null, string search = "")
    {
      var query = _context.Apps.AsQueryable();

      if (serverId.HasValue)
      {
        query = query.Where(t => t.ServerId == serverId);
      }

      if (!string.IsNullOrEmpty(search))
      {
        query = query.Where(t => t.Name.Contains(search));
      }

      var totalApps = await query.CountAsync();
      List<App> apps;
      if (pageSize <= 0)
      {
        apps = await query.ToListAsync();
      }
      else
      {
        apps = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
      }
      Response.Headers.Add("X-Total-Count", totalApps.ToString());

      return Ok(apps);
    }

    [EnableCors("AllowAllOrigins")]
    [HttpGet("{id}")]
    public async Task<ActionResult<App>> GetApp(int id)
    {
      var app = await _context.Apps.FindAsync(id);
      if (app is null)
      {
        return BadRequest("Task not found");
      }

      return Ok(app);
    }

    [EnableCors("AllowAllOrigins")]
    [HttpPost]
    public async Task<ActionResult<List<App>>> AddApp(AppDTO app)
    {
      var newApp = new App
      {
        Name = app.Name,
        Created = app.Created,
        Edited = app.Edited,
        ServerId = app.ServerId
      };
      var existingApp = await _context.Apps.FirstOrDefaultAsync(t => t.Name == app.Name);
      if (existingApp != null)
      {
        return BadRequest("App with the same name already exists");
      }

      _context.Apps.Add(newApp);
      await _context.SaveChangesAsync();

      if (app.tasksIds != null && app.tasksIds.Any())
      {
        var tasksToUpdate = await _context.Tasks.Where(t => app.tasksIds.Contains(t.Id)).ToListAsync();
        foreach (var task in tasksToUpdate)
        {
          task.AppId = newApp.Id;
        }
        await _context.SaveChangesAsync();
      }

      return Ok(await _context.Apps.ToListAsync());
    }

    [EnableCors("AllowAllOrigins")]
    [HttpPut]
    public async Task<ActionResult<List<App>>> UpdateApp(AppDTO updatedApp)
    {
      var dbApp = await _context.Apps.FindAsync(updatedApp.Id);
      if (dbApp is null)
      {
        return BadRequest("Task not found");
      }
      var existingApp = await _context.Apps.FirstOrDefaultAsync(t => t.Name == updatedApp.Name);
      if (existingApp != null)
      {
        return BadRequest("App with the same name already exists");
      }

      dbApp.Name = updatedApp.Name;
      dbApp.Edited = updatedApp.Edited;
      dbApp.ServerId = updatedApp.ServerId;
      Console.WriteLine(updatedApp.tasksIds[0]);

      // Clear existing task associations
      var existingTasks = await _context.Tasks.Where(t => t.AppId == dbApp.Id).ToListAsync();
      foreach (var task in existingTasks)
      {
        task.AppId = 0;
      }

      // Associate tasks based on tasksIds
      var tasksToUpdate = await _context.Tasks.Where(t => updatedApp.tasksIds.Contains(t.Id)).ToListAsync();
      foreach (var task in tasksToUpdate)
      {
        task.AppId = dbApp.Id;
      }

      await _context.SaveChangesAsync();

      return Ok(await _context.Apps.ToListAsync());
    }

    [EnableCors("AllowAllOrigins")]
    [HttpDelete]
    public async Task<ActionResult<List<App>>> DeleteApp(int id)
    {
      var dbApp = await _context.Apps.FindAsync(id);
      if (dbApp is null)
      {
        return BadRequest("Task not found");
      }

      _context.Apps.Remove(dbApp);
      await _context.SaveChangesAsync();

      return Ok(await _context.Apps.ToListAsync());
    }

    [EnableCors("AllowAllOrigins")]
    [HttpDelete("server/{serverId}")]
    public async Task<ActionResult<List<App>>> RemoveServerApps(int serverId)
    {
      var apps = await _context.Apps.Where(a => a.ServerId == serverId).ToListAsync();

      _context.Apps.RemoveRange(apps);
      await _context.SaveChangesAsync();

      return Ok(await _context.Apps.ToListAsync());
    }
  }
}
