using System.Data;
using Dapper;

namespace GestaoVarejo;

public class Repository
{
    private readonly IDbConnection _connection;

    public Repository(IDbConnection connection)
    {
        _connection = connection;
    }

    public IEnumerable<T> GetAll<T>() where T : QueryableEntity
    {
        var query = $"SELECT * FROM {QueryableEntity.TableName<T>()}";
        var rows = _connection.Query(query);

        var entities = new List<T>();

        foreach (var row in rows)
        {
            var rowValues = new List<string>();
            foreach (var column in row) rowValues.Add(column.Value?.ToString() ?? string.Empty);
            var entity = Activator.CreateInstance<T>();
            entity.FillValues(rowValues.ToArray());
            entities.Add(entity);
        }

        return entities;
    }
}
