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
    Console.WriteLine("1. Consultar registros");
    Console.WriteLine("2. Criar registro");
    Console.WriteLine("3. Editar registro");
    Console.WriteLine("4. Deletar registro");
    Console.WriteLine("5. Sair");
    Console.Write("Sua escolha: ");
    
    string? escolha = Console.ReadLine();
    if (escolha == "5") break; 

    switch (escolha)
    {
        case "1":
            ConsultarEntidade(repository);
            break;
        case "2":
            CriarRegistroEntidade(repository);
            break;
        case "3":
            EditarEntidade(repository);
            break;
        case "4":
            DeletarEntidade(repository);
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
        method.Invoke(null, new object[] { repository, displayNames[consulta - 1], null!});
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
        var method = typeof(ConsoleHelper).GetMethod(nameof(ConsoleHelper.CreateUpdateEntity))!
            .MakeGenericMethod(new Type[] { selectedType });
        try 
        {
            method.Invoke(null, new object[] { repository, displayNames[consulta - 1], null!});
            Console.WriteLine("Entidade criada com sucesso !");
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

void DeletarEntidade(Repository repository)
{
    Console.WriteLine("\nSelecione uma entidade para deletar:");

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
        method.Invoke(null, new object[] { repository, displayNames[consulta - 1], null!});

        Console.Write("Escreva o Id do registro para deleção: ");
        if(!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Opção inválida. Tente novamente.");
            return;
        }

        method = typeof(Repository).GetMethod(nameof(Repository.Delete))!.MakeGenericMethod(new Type[] { selectedType });
        try 
        {
            method.Invoke(repository, new object[] { id });
            Console.WriteLine("Registro deletado com sucesso.");
        }
        catch (TargetInvocationException ex)
        {
            if (ex.InnerException is PostgresException && 
                ex.InnerException.Message.Contains("violates foreign key constraint"))
            {
                Console.WriteLine("Não foi possível deletar o registro pois ele está sendo referenciado por outra tabela.");
            }
            else
            {
                throw;
            }
        }
        catch
        {
            Console.WriteLine("Não foi possível deletar a entidade. Verifique se o Id foi informado corretamente.");
        }

    }
    else
    {
        Console.WriteLine("Opção inválida. Tente novamente.");
    }
}

void EditarEntidade(Repository repository)
{
    Console.WriteLine("\nSelecione uma entidade para editar:");

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
        method.Invoke(null, new object[] { repository, displayNames[consulta - 1], null!});

        Console.Write("Escreva o Id do registro para edição: ");
        if(!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Opção inválida. Tente novamente.");
            return;
        }
        method = typeof(ConsoleHelper).GetMethod(nameof(ConsoleHelper.PrintEntityData))!
            .MakeGenericMethod(new Type[] { selectedType });
        method.Invoke(null, new object[] { repository, displayNames[consulta - 1], id});

        Console.WriteLine();
        method = typeof(ConsoleHelper).GetMethod(nameof(ConsoleHelper.CreateUpdateEntity))!
            .MakeGenericMethod(new Type[] { selectedType });
        try 
        {
            method.Invoke(null, new object[] { repository, displayNames[consulta - 1], id });
            Console.WriteLine("Entidade editada com sucesso !");
        }
        catch
        {
            Console.WriteLine("Não foi possível editar a entidade. Verifique se os dados foram preenchidos corretamente.");
        }
    }
    else
    {
        Console.WriteLine("Opção inválida. Tente novamente.");
    }
}




