using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities
{
  public class Server
  {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Created { get; set; }
    public string Edited { get; set; }
  }
}
