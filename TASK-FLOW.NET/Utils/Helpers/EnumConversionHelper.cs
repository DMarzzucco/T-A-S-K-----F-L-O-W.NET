using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TASK_FLOW.NET.Utils.Helpers
{
    public static class EnumConversionHelper
    {
        public static ValueConverter<TEnum, string> EnumConversion<TEnum>() where TEnum : struct, Enum
        {
            return new ValueConverter<TEnum, string>(
                v => v.ToString(),
                v => (TEnum)Enum.Parse(typeof(TEnum), v)
                );
        }
    }
}
