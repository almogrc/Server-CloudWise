namespace BuisnessLogic.Extentions
{
    using BuisnessLogic.Collector.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class EnumExtensions
    {
        public static string GetStringValue(this Enum value)
        {
            var attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(QueryValueAttribute), false)
                .SingleOrDefault() as QueryValueAttribute;

            return attribute?.Value ?? string.Empty;
        }
    }
}
