namespace GestaoVarejo.Domain;

public class Fornecedor
{
    public int Id { get; set; }
    public string? Cnpj { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public int EnderecoId { get; set; }
}