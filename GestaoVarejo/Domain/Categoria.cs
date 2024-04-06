using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoVarejo.Domain;

[Table("categoria")]
public class Categoria
{
    public int Id { get; set; }
    public string? Nome { get; set; }
}