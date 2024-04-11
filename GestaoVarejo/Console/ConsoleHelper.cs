using System.Reflection;

namespace GestaoVarejo;

public static class ConsoleHelper 
{
    public static void PrintEntityData<T>(Repository repository, string nomeEntidade) where T : QueryableEntity
    {
        var items = repository.GetAll<T>();
        Console.WriteLine($"{nomeEntidade}'s:");
        Console.WriteLine();
        foreach (var item in items)
        {
            Console.WriteLine(item.ToDisplayString());
        }
    }

    private static void CreateEntity(Repository repository, Type entityType)
    {
        var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

        var values = new List<string>();

        Console.WriteLine($"Preencha os valores para {entityType.Name}:");
        foreach (var property in properties)
        {
            // Ignora a propriedade Id se for autoincrementável/gerada pelo banco
            if (property.Name == "Id") continue;

            Console.WriteLine($"{property.Name} ({property.PropertyType.Name}):");
            string value = Console.ReadLine();
            values.Add(value ?? string.Empty);
        }

        // Chamada genérica ao método Create<T> usando reflection
        MethodInfo createMethod = typeof(Repository).GetMethod(nameof(Repository.Create)).MakeGenericMethod(new Type[] { entityType });
        createMethod.Invoke(repository, new object[] { values.ToArray() });
    }
}