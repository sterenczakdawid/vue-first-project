using backend.Data;
using backend.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<ActionResult<List<App>>> GetAllApps()
    {
      var apps = await _context.Apps.ToListAsync();

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
    public async Task<ActionResult<List<App>>> AddApp(App app)
    {
      _context.Apps.Add(app);
      await _context.SaveChangesAsync();

      return Ok(await _context.Apps.ToListAsync());
    }

    [EnableCors("AllowAllOrigins")]
    [HttpPut]
    public async Task<ActionResult<List<App>>> UpdateApp(App updatedApp)
    {
      var dbApp = await _context.Apps.FindAsync(updatedApp.Id);
      if (dbApp is null)
      {
        return BadRequest("Task not found");
      }

      dbApp.Name = updatedApp.Name;

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
  }
}
