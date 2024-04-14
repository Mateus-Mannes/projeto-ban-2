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

    public List<(string NomeFuncionario, decimal ValorTotalVendas)> GetTopBestSellersLastMonth()
    {
        DateTime primeiroDiaMesAtual = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        DateTime primeiroDiaMesAnterior = primeiroDiaMesAtual.AddMonths(-1);

        var query = @"
            SELECT CONCAT(f.nome, ' ', f.ultimo_nome) AS NomeFuncionario,
                SUM(v.valor_total) AS ValorTotalVendas
            FROM Venda v
            INNER JOIN Funcionario f
                ON v.funcionario_id = f.Id
            WHERE v.data >= @PrimeiroDiaMesAnterior
                AND v.data < @PrimeiroDiaMesAtual
            GROUP BY NomeFuncionario
            ORDER BY
                SUM(v.valor_total) DESC
            LIMIT 3
            ";

        var results = _dbContext.Query<(string, decimal)>(query,
        new 
        {
            PrimeiroDiaMesAnterior = primeiroDiaMesAnterior,
            PrimeiroDiaMesAtual = primeiroDiaMesAtual
        }).AsList();
        var bestSellers = new List<(string, decimal)>(results);

        return bestSellers;
    }

    public List<(string NomeCliente, decimal ValorTotalCompras)> GetTopClientsLastMonth()
    {
        DateTime primeiroDiaMesAtual = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        DateTime primeiroDiaMesAnterior = primeiroDiaMesAtual.AddMonths(-1);

        var query = @"
            SELECT CONCAT(c.nome, ' ', c.ultimo_nome) AS NomeCliente,
                SUM(v.valor_total) AS ValorTotalCompras
            FROM Cliente c
            INNER JOIN Venda v 
                ON c.Id = v.cliente_id
            WHERE v.data >= @PrimeiroDiaMesAnterior
                AND v.data < @PrimeiroDiaMesAtual
            GROUP BY NomeCliente
            ORDER BY SUM(v.valor_total) DESC
            LIMIT 3;
        ";

        var results = _dbContext.Query<(string, decimal)>(query,
            new 
            {
                PrimeiroDiaMesAnterior = primeiroDiaMesAnterior,
                PrimeiroDiaMesAtual = primeiroDiaMesAtual
            }).AsList();
        var topClients = new List<(string, decimal)>(results);

        return topClients;
    }

    public List<(string Estado, string NomeFuncionario, string NomeCliente, DateTime DataVenda, decimal ValorTotalVenda)> GetSalesByRegionLastMonth()
    {
        DateTime primeiroDiaMesAtual = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        DateTime primeiroDiaMesAnterior = primeiroDiaMesAtual.AddMonths(-1);

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
            WHERE v.data >= @PrimeiroDiaMesAnterior
                AND v.data < @PrimeiroDiaMesAtual
            ORDER BY e.estado, v.data DESC;
        ";

        var results = _dbContext.Query<(string Estado, string NomeFuncionario, string NomeCliente, DateTime DataVenda, decimal ValorTotalVenda)>(query,
            new 
            {
                PrimeiroDiaMesAnterior = primeiroDiaMesAnterior,
                PrimeiroDiaMesAtual = primeiroDiaMesAtual
            }).AsList();

        return results;
    }
}
