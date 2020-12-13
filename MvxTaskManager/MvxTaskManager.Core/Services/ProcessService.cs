using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace MvxTaskManager.Core.Services
{
    public class ProcessService
    {
        public static ObservableCollection<Process> GetProcesses()
        {
            ObservableCollection<Process> processes = new ObservableCollection<Process>();
            foreach (var p in Process.GetProcesses())
            {
                processes.Add(p);
            }

            return processes;
        }
    }
}
