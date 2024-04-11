using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace GestaoVarejo;

public static class ConsoleHelper 
{
    public static void PrintEntityData<T>(Repository repository, string nomeEntidade) where T : QueryableEntity
    {
        var items = repository.GetAll<T>();
        Console.WriteLine($"{nomeEntidade}'s:");
        foreach (var item in items)
        {
            Console.WriteLine(item.ToDisplayString());
        }
    }

    public static void CreateEntity<T>(Repository repository, string nomeEntidade) where T :QueryableEntity
    {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

        var values = new List<string>();

        Console.WriteLine($"Preencha os valores para {nomeEntidade}:");
        foreach (var property in properties)
        {
            // Ignora a propriedade Id se for autoincrementável/gerada pelo banco
            if (property.Name == "Id") continue;

            string aviso = ""; // Inicializa a variável de aviso vazia

            // Define o aviso baseado no tipo da propriedade e se é nullable
            bool isNullable = Nullable.GetUnderlyingType(property.PropertyType) != null;
            if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
            {
                aviso = $"formato yyyy-mm-dd, {(isNullable ? "opcional" : "obrigatório")}";
            }
            else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
            {
                aviso = $"formato 00.00, {(isNullable ? "opcional" : "obrigatório")}";
            }
            else
            {
                aviso = isNullable ? "opcional" : "obrigatório";
            }

            if(property.Name.EndsWith("Id")) 
            {
                var atributoFk = property.GetCustomAttributes()
                    .FirstOrDefault(attr => attr.GetType().IsGenericType &&
                    attr.GetType().GetGenericTypeDefinition() == typeof(FkAttribute<>));
                if (atributoFk != null)
                {
                    // Extrai o tipo genérico associado ao atributo
                    Type referencedType = atributoFk.GetType().GenericTypeArguments[0];
                    var method = typeof(ConsoleHelper).GetMethod(nameof(ConsoleHelper.PrintEntityData))!
                        .MakeGenericMethod(new Type[] { referencedType });
                    method.Invoke(null, new object[] { repository, referencedType.GetCustomAttribute<DisplayAttribute>()!.Name!});
                }
            }

            // Solicita a entrada do usuário com o aviso adequado
            Console.Write($"{property.Name} ({aviso}): ");
            string? value = Console.ReadLine();
            values.Add(value ?? string.Empty);
        }

        // Chamada genérica ao método Create<T> usando reflection
        repository.Create<T>(values.ToArray());

        Console.WriteLine("Entidade criada com sucesso !");
    }
}