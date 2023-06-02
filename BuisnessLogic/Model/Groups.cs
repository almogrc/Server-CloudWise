using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Model
{
    internal class Groups
    {
        public Dictionary<string, Group> GroupNameToGroupData { get; private set; }

        public Groups()
        {
            GroupNameToGroupData = new Dictionary<string, Group>();
        }
    }
}
