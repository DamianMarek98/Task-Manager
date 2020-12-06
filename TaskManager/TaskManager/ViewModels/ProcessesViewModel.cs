using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    public class ProcessesViewModel : Screen
    {
        private string _title = "Processes:";
        private ObservableCollection<Process> _processes = new ObservableCollection<Process>();
        private Process _selectedProcess;

        public ProcessesViewModel()
        {
            LoadProcesses();
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }


        public ObservableCollection<Process> Processes
        {
            get { return _processes; }
            set { _processes = value; }
        }


        public Process SelectedProcess
        {
            get { return _selectedProcess; }
            set
            {
                _selectedProcess = value;
                NotifyOfPropertyChange(() => SelectedProcess);
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
