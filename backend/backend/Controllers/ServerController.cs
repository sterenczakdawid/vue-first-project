using backend.Data;
using backend.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ServerController : ControllerBase
  {
    private readonly DataContext _context;
    public ServerController(DataContext context)
    {
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Server>>> GetAllServers()
    {
      var servers = await _context.Servers.ToListAsync();

      return Ok(servers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Server>> GetServer(int id)
    {
      var server = await _context.Servers.FindAsync(id);
      if (server is null)
      {
        return BadRequest("Server not found");
      }

      return Ok(server);
    }

    [HttpPost]
    public async Task<ActionResult<List<Server>>> AddServer(Server server)
    {
      _context.Servers.Add(server);
      await _context.SaveChangesAsync();

      return Ok(await _context.Servers.ToListAsync());
    }

    [HttpPut]
    public async Task<ActionResult<List<Server>>> UpdateServer(Server updatedServer)
    {
      var dbServer = await _context.Servers.FindAsync(updatedServer.Id);
      if (dbServer is null)
      {
        return BadRequest("Server not found");
      }

      dbServer.Name = updatedServer.Name;

      await _context.SaveChangesAsync();

      return Ok(await _context.Servers.ToListAsync());
    }

    [HttpDelete]
    public async Task<ActionResult<List<Server>>> DeleteServer(int id)
    {
      var dbServer = await _context.Servers.FindAsync(id);
      if (dbServer is null)
      {
        return BadRequest("Task not found");
      }

      _context.Servers.Remove(dbServer);
      await _context.SaveChangesAsync();

      return Ok(await _context.Servers.ToListAsync());
    }
  }
}
