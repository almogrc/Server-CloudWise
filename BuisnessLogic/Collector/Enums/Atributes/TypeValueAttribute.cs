using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Collector.Enums.Atributes
{
    public class TypeValueAttribute : Attribute
    {
        public string Value { get; set; }

        public TypeValueAttribute(string value)
        {
            Value = value;
        }
    }
}
