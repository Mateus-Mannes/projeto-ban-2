using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoVarejo.Domain;

[Table("funcionario")]
public class Funcionario
{
    public int Id { get; set; }
    public string? Cpf { get; set; }
    public string? Nome { get; set; }
    public string? UltimoNome { get; set; }
    public decimal Salario { get; set; }
    public string? Email { get; set; }
    public int EnderecoId { get; set; }
}