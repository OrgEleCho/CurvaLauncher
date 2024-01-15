using System;
using System.ComponentModel;
using System.Reflection;

namespace CurvaLauncher.Utilities;

static class EnumUtils
{
    public static string GetDisplayString(object value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        Type type = value.GetType();

        if (!type.IsEnum)
            throw new ArgumentException("Type of value must be Enum", nameof(value));


        var enumName = Enum.GetName(type, value);

        if (enumName == null)
            throw new ArgumentException("No name for enum value", nameof(value));

        MemberInfo[] memberInfo = type.GetMember(enumName);
        if (memberInfo != null && memberInfo.Length > 0)
        {
            object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attrs != null && attrs.Length > 0)
            {
                //Pull out the description value
                return ((DescriptionAttribute)attrs[0]).Description;
            }
        }

        return enumName;
    }

    public static string GetDisplayString<TEnum>(TEnum value) where TEnum : struct, Enum
    {
        Type type = typeof(TEnum);


        var enumName = Enum.GetName(type, value);

        if (enumName == null)
            throw new ArgumentException("No name for enum value", nameof(value));

        MemberInfo[] memberInfo = type.GetMember(enumName);
        if (memberInfo != null && memberInfo.Length > 0)
        {
            object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attrs != null && attrs.Length > 0)
            {
                //Pull out the description value
                return ((DescriptionAttribute)attrs[0]).Description;
            }
        }

        return enumName;
    }
}