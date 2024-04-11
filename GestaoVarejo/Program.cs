using Dapper;
using GestaoVarejo;
using GestaoVarejo.Domain;
using Npgsql;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Iniciando sistema ...");

var connString = "Host=db;Username=postgres;Password=postgres;Database=gv";
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
var dataSource = dataSourceBuilder.Build();
var conn = await dataSource.OpenConnectionAsync();

var repository = new Repository(conn);
var categorias = repository.GetAll<Categoria>();

// Example 1: Print category names
Console.WriteLine("Example 1: Print category names");
foreach (var categoria in categorias)
{
    Console.WriteLine(categoria.Nome);
}

// Example 2: Print product names
Console.WriteLine("Example 2: Print product names");
var catalogos = repository.GetAll<CatalogoProduto>();
foreach (var produto in catalogos)
{
    Console.WriteLine(produto.Nome);
}

// Example 3: Print client names
Console.WriteLine("Example 3: Print client names");
var clientes = repository.GetAll<Cliente>();
foreach (var e in clientes)
{
    Console.WriteLine(e.Nome);
}

// Example 4: Print supplier CNPJs
Console.WriteLine("Example 4: Print supplier CNPJs");
var fornecedores = repository.GetAll<Fornecedor>();
foreach (var fornecedor in fornecedores)
{
    Console.WriteLine(fornecedor.Cnpj);
}

// Example 5: Print product IDs
Console.WriteLine("Example 5: Print product IDs");
var produtos = repository.GetAll<Produto>();
foreach (var produto in produtos)
{
    Console.WriteLine(produto.Id);
}

// Example 6: Print sale IDs
Console.WriteLine("Example 6: Print sale IDs");
var vendas = repository.GetAll<Venda>();
foreach (var venda in vendas)
{
    Console.WriteLine(venda.Id);
}

// Example 7: Print address streets
Console.WriteLine("Example 7: Print address streets");
var enderecos = repository.GetAll<Endereco>();
foreach (var e in enderecos)
{
    Console.WriteLine(e.Rua);
}

// Example 8: Print purchase NFEs
Console.WriteLine("Example 8: Print purchase NFEs");
var compras = repository.GetAll<Compra>();
foreach (var e in compras)
{
    Console.WriteLine(e.Nfe);
}

// Example 9: Print employee emails
Console.WriteLine("Example 9: Print employee emails");
var funcionarios = repository.GetAll<Funcionario>();
foreach (var e in funcionarios)
{
    Console.WriteLine(e.Email);
}



