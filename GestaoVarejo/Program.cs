using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Dapper;
using GestaoVarejo;
using GestaoVarejo.Domain;
using Npgsql;


Console.WriteLine("Iniciando sistema ...");

var connString = "Host=db;Username=postgres;Password=postgres;Database=gv";
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
var dataSource = dataSourceBuilder.Build();
var conn = await dataSource.OpenConnectionAsync();
var repository = new Repository(conn);

var entityTypes = Assembly.GetExecutingAssembly().GetTypes()
    .Where(t => t.IsSubclassOf(typeof(QueryableEntity)) && !t.IsAbstract);

var displayNames = new List<string>(); 
foreach (var type in entityTypes) 
    displayNames.Add(type.GetCustomAttribute<DisplayAttribute>()?.Name ?? type.Name);

while (true)
{
    Console.WriteLine("\nEscolha uma opção:");
    Console.WriteLine("1. Consultar entidade");
    Console.WriteLine("2. Criar registro de entidade");
    Console.WriteLine("3. Sair");
    Console.Write("Sua escolha: ");
    
    string? escolha = Console.ReadLine();
    if (escolha == "3") break; 

    switch (escolha)
    {
        case "1":
            ConsultarEntidade(repository);
            break;
        case "2":
            CriarRegistroEntidade(repository);
            break;
        default:
            Console.WriteLine("Opção inválida. Tente novamente.");
            break;
    }
}

void ConsultarEntidade(Repository repository)
{
    
    Console.WriteLine("\nSelecione uma entidade para ler:");

    int index = 1;
    foreach (var displayName in displayNames) Console.WriteLine($"{index++}. {displayName}");
    Console.WriteLine($"{index}. Sair");
    Console.Write("Sua escolha: ");

    int.TryParse(Console.ReadLine(), out int consulta);

    if (consulta >= 1 && consulta < index)
    {
        Console.WriteLine();
        var selectedType = entityTypes.ElementAt(consulta - 1);
        var method = typeof(ConsoleHelper).GetMethod(nameof(ConsoleHelper.PrintEntityData))!
            .MakeGenericMethod(new Type[] { selectedType });
        method.Invoke(null, new object[] { repository, displayNames[consulta - 1]});
    }
    else
    {
        Console.WriteLine("Opção inválida. Tente novamente.");
    }
    
}

void CriarRegistroEntidade(Repository repository)
{
    Console.WriteLine("\nSelecione uma entidade para criar:");

    int index = 1;
    foreach (var displayName in displayNames) Console.WriteLine($"{index++}. {displayName}");
    Console.WriteLine($"{index}. Sair");
    Console.Write("Sua escolha: ");

    int.TryParse(Console.ReadLine(), out int consulta);

    if (consulta >= 1 && consulta < index)
    {
        Console.WriteLine();
        var selectedType = entityTypes.ElementAt(consulta - 1);
        var method = typeof(ConsoleHelper).GetMethod(nameof(ConsoleHelper.CreateEntity))!
            .MakeGenericMethod(new Type[] { selectedType });
        try 
        {
            method.Invoke(null, new object[] { repository, displayNames[consulta - 1]});
        }
        catch
        {
            Console.WriteLine("Não foi possível criar a entidade. Verifique se os dados foram preenchidos corretamente.");
        }
    }
    else
    {
        Console.WriteLine("Opção inválida. Tente novamente.");
    }
}




