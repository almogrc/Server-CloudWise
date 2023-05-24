using BuisnessLogic.Exceptions;
using System;
using BuisnessLogic.Loggers;

namespace BuisnessLogic.Model
{
    public class Machine
    {
        public string IP { get; }
        internal MachineDataManager MachineDataManager { get; private set; }   
        internal CollectManager CollectManager { get; private set; }

        public Machine()
        {
            MachineDataManager = new MachineDataManager();
            CollectManager = new CollectManager();
            Logger.Instance.Info($"Machine created.");
        }
        public void CollectInformation()
        {
            //CollectManager.processExporter.Collect();
            //MachineDataManager.Groups = CollectManager.processExporter.Builder.GetResult();
            try
            {
                CollectManager.nodeExporter.Collect();
                MachineDataManager.MachineData = CollectManager.nodeExporter.Builder.GetResult();
            }catch(UnexpectedTypeException utex)
            {
                Logger.Instance.Error(utex.Message + Environment.NewLine + utex.StackTrace);
            }
            catch(UnsuccessfulResponseException urex)
            {
                Logger.Instance.Error(urex.Message + Environment.NewLine + urex.StackTrace);
            }
            catch(Exception ex)
            {
                Logger.Instance.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
    }
}
