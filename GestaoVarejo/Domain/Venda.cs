using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoVarejo.Domain;

[Table("venda")]
[Display(Name = "Venda")]
public class Venda
{
    [Column("id")]
    [Display(Name = "Id")]
    public int Id { get; set; }
    [Column("nfe")]
    [Display(Name = "NFE")]
    public string Nfe { get; set; } = string.Empty;
    [Column("data")]
    [Display(Name = "Data")]
    public DateTime Data { get; set; }
    [Column("valor_total")]
    [Display(Name = "Valor Total")]
    public decimal ValorTotal { get; set; }
    [Column("cliente_id")]
    [Display(Name = "Cliente")]
    public int ClienteId { get; set; }
    [Column("funcionario_id")]
    [Display(Name = "Funcionário")]
    public int FuncionarioId { get; set; }
}