using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Reflection;

namespace GestaoVarejo;

public class QueryableEntity
{
    public static string TableName<T> => this.GetType().GetCustomAttribute<TableAttribute>()!.Name;

    public void Create(string[] values)
    {
        var properties = this.GetType().GetProperties();
        for(int i = 0; i < properties.Length; i++)
        {
            var property = properties[i];
            object value = values[i];

            if(property.PropertyType == typeof(int))
            {
                value = int.Parse(value.ToString()!);
            }

            if(property.PropertyType == typeof(decimal))
            {
                value = decimal.Parse(value!.ToString()!);
            }

            if(property.PropertyType == typeof(DateTime))
            {
                value = DateTime.ParseExact(value!.ToString()!, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            }

            property.SetValue(this, value);
        }
    }
}
