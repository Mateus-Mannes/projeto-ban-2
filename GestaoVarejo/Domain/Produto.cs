namespace GestaoVarejo.Domain;

public class Produto 
{
    public int Id { get; set; }
    public int CatalogoProdutoId { get; set; } 
    public DateTime DataFabricacao { get; set; }
    public DateTime? DataValidade { get; set; }
    public DateTime? DataEntrega { get; set; } 
    public decimal ValorUnitarioCompra { get; set; }
    public int CompraId { get; set; } 
    public int? VendaId { get; set; }
}