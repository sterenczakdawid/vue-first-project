using backend.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace backend.Data
{
  public static class DataSeeder
  {
    public static void Initialize(IServiceProvider serviceProvider)
    {
      using (var context = new DataContext(serviceProvider.GetRequiredService<DbContextOptions<DataContext>>()))
      {
        if (context.Servers.Any() && context.Apps.Any() && context.Tasks.Any() )
        {
          return;   // DB has been seeded
        }
        if(!context.Servers.Any()) {
          var serversData = File.ReadAllText("./Data/Files/servers.json");
          var servers = JsonSerializer.Deserialize<List<Server>>(serversData);
          context.Database.OpenConnection();
          context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Servers ON");
          context.Servers.AddRange(servers);
          context.SaveChanges();
          context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Servers OFF");
          context.Database.CloseConnection();
        }
        if(!context.Apps.Any())
        {
          var appsData = File.ReadAllText("./Data/Files/applications.json");
          var apps = JsonSerializer.Deserialize<List<App>>(appsData);
          context.Database.OpenConnection();
          context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Apps ON");
          context.Apps.AddRange(apps);
          context.SaveChanges();
          context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Apps OFF");
          context.Database.CloseConnection();
        }
        if(!context.Tasks.Any())
        {
          var tasksData = File.ReadAllText("./Data/Files/tasks.json");
          var tasks = JsonSerializer.Deserialize<List<MyTask>>(tasksData);
          context.Database.OpenConnection();
          context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Tasks ON");
          context.Tasks.AddRange(tasks);
          context.SaveChanges();
          context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Tasks OFF");
          context.Database.CloseConnection();
        }
        //context.SaveChanges();
      }
    }
  }
}
