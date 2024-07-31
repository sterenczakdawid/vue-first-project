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
          context.Servers.AddRange(servers);
        }
        if(!context.Apps.Any())
        {
          var appsData = File.ReadAllText("./Data/Files/applications.json");
          var apps = JsonSerializer.Deserialize<List<App>>(appsData);
          context.Apps.AddRange(apps);
        }
        if(!context.Tasks.Any())
        {
          var tasksData = File.ReadAllText("./Data/Files/tasks.json");
          var tasks = JsonSerializer.Deserialize<List<MyTask>>(tasksData);
          context.Tasks.AddRange(tasks);
        }
        context.SaveChanges();
      }
    }
  }
}
