using Newtonsoft.Json;
using Server_cloudata.DTO;
using Server_cloudata.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server_cloudata.ServerDataManager
{
    public class ServerUtils
    {
        public static string SessionCookie = "sessionCookie";
        public static string MachineId = "machineId";
        public static string ResultJson(string message)
        {
            return $"{{\"result\":\"{message}\"}}";
        }
        public static string ErrorJson(string message)
        {
            return $"{{\"error\":\"{message}\"}}";
        }
        public static async Task<List<VirtualMachineDTO>> CheckMachineStatus(List<VirtualMachine> virtualMachines, string machinesPrometheusJson)
        {
            Dictionary<string, VirtualMachineDTO> virtualMachinesDTO = new Dictionary<string, VirtualMachineDTO>();
            var json = JsonConvert.DeserializeObject<dynamic>(machinesPrometheusJson);
            json = json["data"];
            json = json["activeTargets"];
            foreach (var virtualMachine in virtualMachines)
            {
                foreach (var target in json)
                {

                    if (!virtualMachinesDTO.ContainsKey(virtualMachine.Name) && target["scrapeUrl"].ToString().Contains(virtualMachine.Address))
                    {
                        if (target["health"] == "up")
                        {
                            virtualMachinesDTO.Add(virtualMachine.Name,
                                new VirtualMachineDTO() { Name = virtualMachine.Name, DNSAddress = virtualMachine.Address, Supplier = virtualMachine.Supplier, Status = "UP" });
                        }
                        else
                        {
                            virtualMachinesDTO.Add(virtualMachine.Name,
                                new VirtualMachineDTO() { Name = virtualMachine.Name, DNSAddress = virtualMachine.Address, Supplier = virtualMachine.Supplier, Status = "DOWN" });
                        }
                    }
                }
                if (!virtualMachinesDTO.ContainsKey(virtualMachine.Name))
                {
                    virtualMachinesDTO.Add(virtualMachine.Name,
                        new VirtualMachineDTO() { Name = virtualMachine.Name, DNSAddress = virtualMachine.Address, Supplier = virtualMachine.Supplier, Status = "NOTCONNECTED" });
                }
            }
            return virtualMachinesDTO.Values.ToList();

        }
        // public Dictionary<string, HttpSessionState>
        // private Dictionary<string, > _
    }
}
