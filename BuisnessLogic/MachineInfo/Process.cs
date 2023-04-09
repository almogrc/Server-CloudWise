using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.MachineInfo
{
    public class Process
    {
        public Process(string name, int PID, string path, string port)
        {
            Name = name;
            this.PID = PID;
            Path = path;
            Port = port;
        }

        public virtual string Name { get; }
        public virtual int PID { get; }
        public virtual string Path { get; }
        public virtual string Port { get; }

    }
}
