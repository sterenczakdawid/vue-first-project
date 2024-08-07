using backend.Data;
using backend.Entities;
using ClosedXML.Excel;
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
      var existingTask = await _context.Tasks.FirstOrDefaultAsync(t => t.Name == updatedTask.Name && t.Id != updatedTask.Id);
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

    [EnableCors("AllowAllOrigins")]
    [HttpGet("export")]
    //GetAllTasks(int page = 1, int pageSize = 5, int? serverId = null, int? appId = null, string search = "", string sortBy = "", bool? sortDesc = false)
    public async Task<IActionResult> ExportTasksToExcel() 
    //public async Task<IActionResult> ExportTasksToExcel(int? serverId = null, int? appId = null, string search = "", string sortBy = "", bool? sortDesc = false)
    {
      var query = _context.Tasks.AsQueryable();

      //if (serverId.HasValue)
      //{
      //  query = query.Where(t => t.ServerId == serverId);
      //}

      //if (appId.HasValue)
      //{
      //  query = query.Where(t => t.AppId == appId);
      //}

      //if (!string.IsNullOrEmpty(search))
      //{
      //  query = query.Where(t => t.Name.Contains(search));
      //}

      //if (!string.IsNullOrEmpty(sortBy))
      //{
      //  if (sortDesc.GetValueOrDefault())
      //  {
      //    query = query.OrderByDescending(e => EF.Property<object>(e, sortBy));
      //  }
      //  else
      //  {
      //    query = query.OrderBy(e => EF.Property<object>(e, sortBy));
      //  }
      //}

      var tasks = await query.ToListAsync();
      tasks = await _context.Tasks.ToListAsync();

      using (var workbook = new XLWorkbook())
      {
        var worksheet = workbook.Worksheets.Add("Tasks");
        var currentRow = 1;
        worksheet.Cell(currentRow, 1).Value = "Id";
        worksheet.Cell(currentRow, 2).Value = "Name";
        worksheet.Cell(currentRow, 3).Value = "Created";
        worksheet.Cell(currentRow, 4).Value = "Edited";
        worksheet.Cell(currentRow, 5).Value = "ServerId";
        worksheet.Cell(currentRow, 6).Value = "AppId";

        foreach (var task in tasks)
        {
          currentRow++;
          worksheet.Cell(currentRow, 1).Value = task.Id;
          worksheet.Cell(currentRow, 2).Value = task.Name;
          worksheet.Cell(currentRow, 3).Value = task.Created;
          worksheet.Cell(currentRow, 4).Value = task.Edited;
          worksheet.Cell(currentRow, 5).Value = task.ServerId;
          worksheet.Cell(currentRow, 6).Value = task.AppId;
        }

        using (var stream = new MemoryStream())
        {
          workbook.SaveAs(stream);
          var content = stream.ToArray();

          return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "tasks.xlsx");
        }
      }
    }
  }


}
