namespace GestaoVarejo.Domain;

public class CatalogoProduto
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int CategoriaId { get; set; }
}