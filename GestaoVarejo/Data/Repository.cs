using System.Data;

namespace GestaoVarejo;

public class Repository<T> where T : QueryableEntity
{
    private readonly IDbConnection _connection;

    public Repository(IDbConnection connection)
    {
        _connection = connection;
    }

    // do the select command
    public IEnumerable<T> Select()
    {
        var query = $"SELECT * FROM {typeof(T).Name.ToLower()}";
        return _connection.Query<T>(query);
    }
}
