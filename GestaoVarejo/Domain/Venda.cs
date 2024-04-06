using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoVarejo.Domain;

[Table("venda")]
public class Venda
{
    public int Id { get; set; }
    public string? Nfe { get; set; }
    public DateTime Data { get; set; }
    public decimal ValorTotal { get; set; }
    public int ClienteId { get; set; }
    public int FuncionarioId { get; set; }
}