using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MvxTaskManager.Core.Models;

namespace MvxTaskManager.Core.Services
{
    public class ProcessService
    {
        public static ObservableCollection<ProcessModel> GetProcesses(ObservableCollection<SupportedProcessModel> supportedProcesses)
        {
            ObservableCollection<ProcessModel> processes = new ObservableCollection<ProcessModel>();
            foreach (var p in Process.GetProcesses())
            {
                processes.Add(new ProcessModel(p.Id, p.ProcessName, p.WorkingSet64, p.BasePriority, IsSupported(p.Id, supportedProcesses)));
            }

            return processes;
        }

        public static bool IsSupported(int PID, ObservableCollection<SupportedProcessModel> supportedProcesses)
        {
            return supportedProcesses.FirstOrDefault(p => p.PID == PID) != null;
        }
    }
}
