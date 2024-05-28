using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Reflection;

namespace MediWeb.Helpers;
public static class EnumExtensions
{
    public static List<string> GetEnumDescriptions<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T))
                   .Cast<T>()
                   .Select(e => GetEnumDescription(e))
                   .ToList();
    }

    private static string GetEnumDescription<T>(T value)
    {
        FieldInfo fi = value?.GetType().GetField(value.ToString());
        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi?.GetCustomAttributes(typeof(DescriptionAttribute), false);

        return (attributes != null && attributes.Length > 0) ? attributes[0]?.Description : value.ToString();
    }
}
