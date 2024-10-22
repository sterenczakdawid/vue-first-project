using backend.Data;
using backend.Entities;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Cors;
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

    [EnableCors("AllowAllOrigins")]
    [HttpGet]
    public async Task<ActionResult<List<Server>>> GetAllServers(int page = 1, int pageSize = 5, string search = "", string sortBy = "", bool? sortDesc = false)
    {
      var query = _context.Servers.AsQueryable();


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

      var totalServers = await query.CountAsync();
      List<Server> servers;
      if (pageSize <= 0)
      {
        servers = await query.ToListAsync();
      }
      else
      {
        servers = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
      }

      // Dodawanie nagłówka z całkowitą liczbą zadań
      Response.Headers.Add("X-Total-Count", totalServers.ToString());

      return Ok(servers);
    }

    [EnableCors("AllowAllOrigins")]
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

    [EnableCors("AllowAllOrigins")]
    [HttpPost]
    public async Task<ActionResult<List<Server>>> AddServer(Server server)
    {
      var existingServer = await _context.Servers.FirstOrDefaultAsync(t => t.Name == server.Name);
      if (existingServer != null)
      {
        return BadRequest("Server with the same name already exists");
      }
      _context.Servers.Add(server);
      await _context.SaveChangesAsync();

      return Ok(await _context.Servers.ToListAsync());
    }

    [EnableCors("AllowAllOrigins")]
    [HttpPut]
    public async Task<ActionResult<List<Server>>> UpdateServer(Server updatedServer)
    {
      var dbServer = await _context.Servers.FindAsync(updatedServer.Id);
      if (dbServer is null)
      {
        return BadRequest("Server not found");
      }
      var existingServer = await _context.Servers
                                      .FirstOrDefaultAsync(t => t.Name == updatedServer.Name && t.Id != updatedServer.Id);
      if (existingServer != null)
      {
        return BadRequest("Server with the same name already exists");
      }

      dbServer.Name = updatedServer.Name;
      dbServer.Edited = updatedServer.Edited;

      await _context.SaveChangesAsync();

      return Ok(await _context.Servers.ToListAsync());
    }

    [EnableCors("AllowAllOrigins")]
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

    [EnableCors("AllowAllOrigins")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportServersToExcel()
    {
      var servers = await _context.Servers.ToListAsync();

      using (var workbook = new XLWorkbook())
      {
        var worksheet = workbook.Worksheets.Add("Servers");
        var currentRow = 1;
        worksheet.Cell(currentRow, 1).Value = "Id";
        worksheet.Cell(currentRow, 2).Value = "Name";
        worksheet.Cell(currentRow, 3).Value = "Created";
        worksheet.Cell(currentRow, 4).Value = "Edited";

        foreach (var server in servers)
        {
          currentRow++;
          worksheet.Cell(currentRow, 1).Value = server.Id;
          worksheet.Cell(currentRow, 2).Value = server.Name;
          worksheet.Cell(currentRow, 3).Value = server.Created;
          worksheet.Cell(currentRow, 4).Value = server.Edited;
        }

        using (var stream = new MemoryStream())
        {
          workbook.SaveAs(stream);
          var content = stream.ToArray();

          return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "servers.xlsx");
        }
      }
    }
  }
}
