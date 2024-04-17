using System.Data;
using Dapper;

namespace GestaoVarejo;

public class ReportService
{
    private readonly IDbConnection _dbContext;

    public ReportService(IDbConnection connection)
    {
        _dbContext = connection;
    }

    public List<(string NomeFuncionario, decimal ValorTotalVendas, string Estado)> GetTopBestSellersInRange(DateTime dataInicial, DateTime dataFinal)
    {
        var query = @"
            SELECT CONCAT(f.nome, ' ', f.ultimo_nome) AS NomeFuncionario,
                SUM(v.valor_total) AS ValorTotalVendas,
                e.estado AS Estado
            FROM Venda v
            INNER JOIN Funcionario f ON v.funcionario_id = f.Id
            INNER JOIN Endereco e ON f.endereco_id = e.Id
            WHERE v.data >= @DataInicial
                AND v.data < @DataFinal
            GROUP BY NomeFuncionario, Estado
            ORDER BY SUM(v.valor_total) DESC
            LIMIT 3
            ";

        var results = _dbContext.Query<(string, decimal, string)>(query, new
        {
            DataInicial = dataInicial,
            DataFinal = dataFinal
        }).AsList();

        var bestSellers = new List<(string, decimal, string)>(results);

        return bestSellers;
    }

    public List<(string NomeCliente, decimal ValorTotalCompras, string Cidade, string Estado)> GetTopClientsInRange(DateTime dataInicial, DateTime dataFinal)
    {
        var query = @"
            SELECT CONCAT(c.nome, ' ', c.ultimo_nome) AS NomeCliente,
                SUM(v.valor_total) AS ValorTotalCompras,
                e.cidade AS Cidade,
                e.estado AS Estado
            FROM Cliente c
            INNER JOIN Venda v ON c.Id = v.cliente_id
            INNER JOIN Endereco e ON c.endereco_id = e.Id
            WHERE v.data >= @DataInicial
                AND v.data < @DataFinal
            GROUP BY NomeCliente, Cidade, Estado
            ORDER BY SUM(v.valor_total) DESC
            LIMIT 3;
        ";

        var results = _dbContext.Query<(string, decimal, string, string)>(query, new
        {
            DataInicial = dataInicial,
            DataFinal = dataFinal
        }).AsList();

        var topClients = new List<(string, decimal, string, string)>(results);

        return topClients;
    }

    public List<(string Estado, string NomeFuncionario, string NomeCliente, DateTime DataVenda, decimal ValorTotalVenda)> GetSalesByRegionInRange(DateTime dataInicial, DateTime dataFinal)
    {
        var query = @"
            SELECT e.estado AS Estado,
                CONCAT(f.nome, ' ', f.ultimo_nome) AS NomeFuncionario,
                CONCAT(c.nome, ' ', c.ultimo_nome) AS NomeCliente,
                v.data AS DataVenda,
                v.valor_total AS ValorTotalVenda
            FROM Venda v
            INNER JOIN Funcionario f 
                ON v.funcionario_id = f.id
            INNER JOIN Cliente c 
                ON v.cliente_id = c.id
            INNER JOIN Endereco e 
                ON c.endereco_id = e.id
            WHERE v.data >= @DataInicial
                AND v.data < @DataFinal
            ORDER BY e.estado, v.data DESC;
        ";

        var results = _dbContext.Query<(string Estado, string NomeFuncionario, string NomeCliente, DateTime DataVenda, decimal ValorTotalVenda)>(query, new
        {
            DataInicial = dataInicial,
            DataFinal = dataFinal
        }).AsList();

        return results;
    }

    public List<(int ProdutoId, 
        DateTime DataFabricacao, 
        DateTime? DataValidade, 
        DateTime DataCompra, 
        decimal ValorUnitarioCompra, 
        string NomeProduto, 
        string EmailFornecedor, 
        decimal ValorTotalCompra)> 
    GetProductPurchaseInRange(DateTime dataInicial, DateTime dataFinal)
    {
        dataFinal = dataFinal.AddDays(1); // Adiciona 1 dia à data final para incluir o último dia no intervalo

        var query = @"
            SELECT p.id AS ProdutoId,
                p.data_fabricacao AS DataFabricacao,
                p.data_validade AS DataValidade,
                co.data AS DataCompra,
                p.valor_unitario_compra AS ValorUnitarioCompra,
                cp.nome AS NomeProduto,
                f.email AS EmailFornecedor,
                co.valor_total AS ValorTotalCompra
            FROM produto p
            INNER JOIN catalogo_produto cp 
                ON p.catalogo_produto_id = cp.id
            INNER JOIN compra co 
                ON p.compra_id = co.id
            INNER JOIN fornecedor f 
                ON co.fornecedor_id = f.id
            WHERE co.data >= @DataInicial
                AND co.data < @DataFinal
            ORDER BY p.data_validade ASC;
        ";

        var results = _dbContext.Query<(int, DateTime, DateTime?, DateTime, decimal, string, string, decimal)>(query,
            new
            {
                DataInicial = dataInicial,
                DataFinal = dataFinal
            }).AsList();

        var purchases = results.Select(r => (
            ProdutoId: r.Item1,
            DataFabricacao: r.Item2,
            DataValidade: r.Item3,
            DataCompra: r.Item4,
            PrecoProduto: r.Item5,
            NomeProduto: r.Item6,
            EmailFornecedor: r.Item7,
            ValorTotalCompra: r.Item8
        )).ToList();

        return purchases;
    }
}