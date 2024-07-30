namespace backend.Entities
{
  public class MyTask
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Created { get; set; }
    public string Edited { get; set; }
    public int ServerId { get; set; }
    public int AppId { get; set; }
  }
}
