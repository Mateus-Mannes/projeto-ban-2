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

//var repository = new Repository(); // Suponha que essa seja a sua instância de repositório
    //     while (true)
    //     {
    //         Console.WriteLine("\nEscolha uma opção:");
    //         Console.WriteLine("1. Consultar entidade");
    //         Console.WriteLine("2. Criar registro de entidade");
    //         Console.WriteLine("3. Sair");
    //         Console.Write("Sua escolha: ");
            
    //         if (!int.TryParse(Console.ReadLine(), out int mainChoice) || mainChoice == 3)
    //         {
    //             break; // Sair se a escolha for "Sair" ou entrada inválida
    //         }

    //         switch (mainChoice)
    //         {
    //             case 1:
    //                 // Lógica para consultar entidades
    //                 ConsultarEntidade(repository);
    //                 break;
    //             case 2:
    //                 // Lógica para criar registros de entidades
    //                 CriarRegistroEntidade(repository);
    //                 break;
    //             default:
    //                 Console.WriteLine("Opção inválida. Tente novamente.");
    //                 break;
    //         }
    //     }
    // }

    // private static void ConsultarEntidade(Repository repository)
    // {
    //     // Aqui vai a lógica existente para escolher e consultar uma entidade.
    //     // Você pode reaproveitar o código que já tinha para a seleção e consulta de entidades,
    //     // agora movido para dentro deste método.
    // }

    // private static void CriarRegistroEntidade(Repository repository)
    // {
    //     // Aqui você vai solicitar ao usuário que escolha uma entidade para a qual deseja criar um registro.
    //     // Após a escolha da entidade, chame o método CreateEntity para coletar os valores e criar o registro.
    //     // Essa lógica será similar à de ConsultarEntidade, mas terminará chamando CreateEntity ao invés de PrintEntityData.
    // }


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

