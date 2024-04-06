using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoVarejo.Domain;

[Table("catalogo_produto")]
public class CatalogoProduto
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int CategoriaId { get; set; }
}