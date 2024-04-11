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
}