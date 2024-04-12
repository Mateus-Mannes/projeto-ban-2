using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoVarejo.Domain;

[Table("produto")]
[DisplayName("Produto")]
public class Produto  : QueryableEntity
{
    [Column("id")]
    [DisplayName("Id")]
    public override int Id { get; set; }
    [Column("catalogo_produto_id")]
    [DisplayName("Catalogo Produto Id")]
    public int CatalogoProdutoId { get; set; } 
    [Column("data_fabricacao")]
    [DisplayName("Data Fabricação")]
    public DateTime DataFabricacao { get; set; }
    [Column("data_validade")]
    [DisplayName("Data Validade")]
    public DateTime? DataValidade { get; set; }
    [Column("data_entrega")]
    [DisplayName("Data Entrega")]
    public DateTime? DataEntrega { get; set; } 
    [Column("valor_unitario_compra")]
    [DisplayName("Valor Unitário Compra")]
    public decimal ValorUnitarioCompra { get; set; }
    [Column("compra_id")]
    [DisplayName("Compra Id")]
    [Fk<Compra>]
    public int CompraId { get; set; } 
    [Column("venda_id")]
    [DisplayName("Venda Id")]
    [Fk<Venda>]
    public int? VendaId { get; set; }
}