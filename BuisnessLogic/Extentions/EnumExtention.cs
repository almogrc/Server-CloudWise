﻿namespace BuisnessLogic.Extentions
{
    using BuisnessLogic.Collector.Enums.Atributes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public static class EnumExtensions
    {
        public static string GetQueryValue(this Enum value)
        {
            var attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(QueryValueAttribute), false)
                .SingleOrDefault() as QueryValueAttribute;

            return attribute?.Value ?? string.Empty;
        }
        public static string GetTypeValue(this Enum value)
        {
            var attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(TypeValueAttribute), false)
                .SingleOrDefault() as TypeValueAttribute;

            return attribute?.Value ?? string.Empty;
        }
        public static bool HasAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
        {
            FieldInfo fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            return fieldInfo.GetCustomAttribute<TAttribute>() != null;
        }
    }
}
