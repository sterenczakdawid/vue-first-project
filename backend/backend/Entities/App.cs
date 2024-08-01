using System.Numerics;

namespace backend.Entities
{
  public class App
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Created { get; set; }
    public string Edited { get; set; }
    public int ServerId { get; set; }
  }

  public class AppDTO : App
  {
    public List<int> tasksIds { get; set; }
  }
}
