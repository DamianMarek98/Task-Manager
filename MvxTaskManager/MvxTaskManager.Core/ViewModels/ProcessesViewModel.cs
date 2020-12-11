using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using MvvmCross.ViewModels;

namespace MvxTaskManager.Core.ViewModels
{
    public class ProcessesViewModel : MvxViewModel
    {
        public ProcessesViewModel()
        {
            LoadProcesses();
        }

        private ObservableCollection<Process> _processes = new ObservableCollection<Process>();

        private Process _selectedProcess;

        private static string _title = "Process:";

        public ObservableCollection<Process> Processes
        {
            get { return _processes; }
            set { SetProperty(ref _processes, value);  }
        }

        public string Title
        {
            get { return _title; }
        }

        public Process SelectedProcess
        {
            get { return _selectedProcess; }
            set
            {
                _selectedProcess = value;
            }
        }

        public void LoadProcesses()
        {
            Processes.Clear();
            foreach (var p in Process.GetProcesses())
            {
                Processes.Add(p);
            }
        }
    }
}
