using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoVarejo.Domain;

[Table("compra")]
[Display(Name = "Compra")]
public class Compra : QueryableEntity
{ 
    [Column("id")]
    [Display(Name = "Id")]
    public int Id { get; set; }
    [Column("nfe")]
    [Display(Name = "NFE")]
    public string Nfe { get; set; } = string.Empty;
    [Column("data")]
    [Display(Name = "Data")]
    public DateTime Data { get; set; } = DateTime.Now;
    [Column("valor_total")]
    [Display(Name = "Valor Total")]
    public decimal ValorTotal { get; set; }
    [Column("fornecedor_id")]
    [Display(Name = "Fornecedor")]
    public int FornecedorId { get; set; }
}