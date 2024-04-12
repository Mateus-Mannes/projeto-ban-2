using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
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

    public void Create<T>(string[] values) where T : QueryableEntity
    {
        var tableName = QueryableEntity.TableName<T>();

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.Name != "Id");

        var columns = string.Join(", ", properties.Select(p => p.GetCustomAttribute<ColumnAttribute>()!.Name));

        var parameterNames = string.Join(", ", properties.Select((p, index) => $"@param{index}"));

        var query = $"INSERT INTO {tableName} ({columns}) VALUES ({parameterNames})";

        // Cria um dicionário para mapear os valores aos parâmetros
        var parameterDictionary = new DynamicParameters();
        int i = 0;
        foreach (var property in properties)
        {
            if (i < values.Length)
            {
                var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                // Converte o valor para o tipo correto da propriedade, tratando nulls de forma apropriada
                var safeValue = (values[i] == null) ? null : Convert.ChangeType(values[i], propertyType);
                parameterDictionary.Add($"param{i}", safeValue);
            }
            i++;
        }
        
        _connection.Execute(query, parameterDictionary);
    }

    public void Delete<T>(int id) where T : QueryableEntity
    {
        var tableName = QueryableEntity.TableName<T>();
        var query = $"DELETE FROM {tableName} WHERE Id = @Id";

        _connection.Execute(query, new { Id = id });
    }

    public void Update<T>(int id, string[] values) where T : QueryableEntity
    {
        var tableName = QueryableEntity.TableName<T>();

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.Name != "Id");

        var setClause = string.Join(", ", properties.Select((p, index) => 
            $"{p.GetCustomAttribute<ColumnAttribute>()!.Name} = @param{index}"));

        var query = $"UPDATE {tableName} SET {setClause} WHERE Id = @Id";

        var parameterDictionary = new DynamicParameters();
        int i = 0;
        foreach (var property in properties)
        {
            if (i < values.Length)
            {
                var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                // Converte o valor para o tipo correto da propriedade, tratando nulls de forma apropriada
                var safeValue = (values[i] == null) ? null : Convert.ChangeType(values[i], propertyType);
                parameterDictionary.Add($"param{i}", safeValue);
            }
            i++;
        }

        parameterDictionary.Add("Id", id);

        _connection.Execute(query, parameterDictionary);
    }
}
