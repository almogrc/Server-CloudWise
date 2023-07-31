using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Model
{
    public class Groups
    {
        public Dictionary<string, List<DataPoint>> GroupNameToGroupData { get; private set; }

        public Groups()
        {
            GroupNameToGroupData = new Dictionary<string, List<DataPoint>>();
        }
    }
}
