using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoVarejo.Domain;

[Table("compra")]
public class Compra 
{
    public int Id { get; set; }
    public string? Nfe { get; set; }
    public DateTime Data { get; set; }
    public decimal ValorTotal { get; set; }
    public int FornecedorId { get; set; }
}