using System.ComponentModel.DataAnnotations;
using System.Reflection;
using GestaoVarejo;
using Npgsql;


Console.WriteLine("Iniciando sistema ...");

var connString = "Host=db;Username=postgres;Password=postgres;Database=gv";
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
var dataSource = dataSourceBuilder.Build();
var conn = await dataSource.OpenConnectionAsync();
var repository = new Repository(conn);
var reportService = new ReportService(conn);

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
    Console.WriteLine("5. Relatórios");
    Console.WriteLine("6. Sair");
    Console.Write("Sua escolha: ");
    
    string? escolha = Console.ReadLine();
    if (escolha == "6") break; 

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
        case "5":
            ShowRelatoriosSubMenu(reportService);
            break;
        default:
            Console.WriteLine("Opção inválida. Tente novamente.");
            break;
    }
}

void ShowRelatoriosSubMenu(ReportService reportService)
{
    while (true)
    {
        Console.WriteLine("\nEscolha um relatório:");
        Console.WriteLine("1. Top 3 Vendedores do Último Mês");
        Console.WriteLine("2. Top Clientes do Último Mês");
        Console.WriteLine("3. Vendas por Região do Último Mês");
        Console.WriteLine("4. Voltar");

        Console.Write("Sua escolha: ");
        string? escolha = Console.ReadLine();

        switch (escolha)
        {
            case "1":
                ConsultarTopVendedoresUltimoMes(reportService);
                break;
            case "2":
                ConsultarTopClientesUltimoMes(reportService);
                break;
            case "3":
                ConsultarVendasPorRegiaoUltimoMes(reportService);
                break;
            case "4":
                return;
            default:
                Console.WriteLine("Opção inválida. Tente novamente.");
                break;
        }
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

void ConsultarTopVendedoresUltimoMes(ReportService reportService)
{
    Console.WriteLine("\nConsultando Top 3 Vendedores do Último Mês...\n");

    var topVendedores = reportService.GetTopBestSellersLastMonth();

    if (topVendedores != null && topVendedores.Any())
    {
        Console.WriteLine("Top 3 Vendedores do Último Mês:");
        for (int i = 0; i < topVendedores.Count; i++)
        {
            var (nomeFuncionario, valorTotalVendas) = topVendedores[i];
            Console.WriteLine($"Posição {i + 1}: {nomeFuncionario} - Total de Vendas: R$ {valorTotalVendas:N2}");
        }
    }
    else
    {
        Console.WriteLine("Nenhum vendedor encontrado ou não há dados suficientes para determinar o top 3 do último mês.");
    }
}

void ConsultarTopClientesUltimoMes(ReportService reportService)
{
    Console.WriteLine("\nConsultando Top Clientes do Último Mês...\n");

    var topClients = reportService.GetTopClientsLastMonth();

    if (topClients.Count > 0)
    {
        Console.WriteLine("Top Clientes no último mês:");
        foreach (var client in topClients)
        {
            Console.WriteLine($"Nome: {client.NomeCliente} - Valor Total Compras: R$ {client.ValorTotalCompras:N2}");
        }
    }
    else
    {
        Console.WriteLine("Nenhum cliente encontrado no último mês.");
    }
}

void ConsultarVendasPorRegiaoUltimoMes(ReportService reportService)
{
    Console.WriteLine("\nConsultando Vendas por Região no Último Mês...\n");

    var salesByRegion = reportService.GetSalesByRegionLastMonth();

    if (salesByRegion.Count > 0)
    {
        Console.WriteLine("Relatório de Vendas por Região:");

        string lastState = null!;  // Variável para armazenar o último estado exibido

        foreach (var sale in salesByRegion)
        {
            if (sale.Estado != lastState)
            {
                Console.WriteLine($"Estado: {sale.Estado}");
                lastState = sale.Estado;
            }

            Console.WriteLine($"  - Funcionário: {sale.NomeFuncionario}");
            Console.WriteLine($"  - Cliente: {sale.NomeCliente}");
            Console.WriteLine($"  - Data da Venda: {sale.DataVenda}");
            Console.WriteLine($"  - Valor Total da Venda: R$ {sale.ValorTotalVenda:N2}\n");
        }
    }
    else
    {
        Console.WriteLine("Nenhuma venda encontrada por região no último mês.");
    }
}