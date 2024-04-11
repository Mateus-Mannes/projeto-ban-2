using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Reflection;

namespace GestaoVarejo;

public class QueryableEntity
{
    public static string TableName<T>() where T : QueryableEntity 
        => typeof(T).GetCustomAttribute<TableAttribute>()!.Name;

    public void FillValues(string[] values)
    {
         var properties = this.GetType().GetProperties();
        for (int i = 0; i < properties.Length; i++)
        {
            if (i >= values.Length)
                break;

            var property = properties[i];
            var propertyType = property.PropertyType;
            string stringValue = values[i];
            object value = null;

            if (Nullable.GetUnderlyingType(propertyType) != null && !string.IsNullOrEmpty(stringValue))
            {
                var underlyingType = Nullable.GetUnderlyingType(propertyType);
                if (underlyingType == typeof(int))
                {
                    value = int.Parse(stringValue);
                }
                else if (underlyingType == typeof(decimal))
                {
                    value = decimal.Parse(stringValue);
                }
                else if (underlyingType == typeof(DateTime))
                {
                    value = DateTime.Parse(stringValue);
                }
                else if (propertyType == typeof(string)) 
                {
                    value = stringValue;
                }
            }
            else if (!string.IsNullOrEmpty(stringValue))
            {
                if (propertyType == typeof(int))
                {
                    value = int.Parse(stringValue);
                }
                else if (propertyType == typeof(decimal))
                {
                    value = decimal.Parse(stringValue);
                }
                else if (propertyType == typeof(DateTime))
                {
                    value = DateTime.Parse(stringValue);
                }
                else if (propertyType == typeof(string)) 
                {
                    value = stringValue;
                }
            }

            property.SetValue(this, value);
        }
    }
}
