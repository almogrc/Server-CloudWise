namespace BuisnessLogic.Collector.Enums.Atributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class QueryValueAttribute : Attribute
    {
        public string Value { get; set; }

        public QueryValueAttribute(string value)
        {
            Value = value;
        }
    }
}
