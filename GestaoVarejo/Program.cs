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
    Console.WriteLine("\nSelecione uma entidade para ler:");

    int index = 1;
    foreach (var displayName in displayNames) Console.WriteLine($"{index++}. {displayName}");
    Console.WriteLine($"{index}. Sair");

    if (int.TryParse(Console.ReadLine(), out int choice) && choice == index)
    {
        break; // Sair se a escolha for igual ao último índice
    }

    if (choice >= 1 && choice < index)
    {
        Console.WriteLine();
        var selectedType = entityTypes.ElementAt(choice - 1);
        var method = typeof(ConsoleHelper).GetMethod(nameof(ConsoleHelper.PrintEntityData))!
            .MakeGenericMethod(new Type[] { selectedType });
        method.Invoke(null, new object[] { repository, displayNames[choice - 1]});
    }
    else
    {
        Console.WriteLine("Opção inválida. Tente novamente.");
    }
}

