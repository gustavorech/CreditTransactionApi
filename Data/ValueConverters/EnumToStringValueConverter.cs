using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CreditTransactionApi.Data;

public class EnumToStringValueConverter<T> : ValueConverter<T, string> where T : Enum
{
    public EnumToStringValueConverter() : base(
        v => v.ToString(),
        v => (T)Enum.Parse(typeof(T), v))
    {
    }
}