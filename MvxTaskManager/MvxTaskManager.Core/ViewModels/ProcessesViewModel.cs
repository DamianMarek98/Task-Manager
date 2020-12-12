using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Timers;

namespace MvxTaskManager.Core.ViewModels
{
    public class ProcessesViewModel : MvxViewModel
    {
        private System.Timers.Timer _reloadTimer;

        public ProcessesViewModel()
        {
            LoadProcesses();
            ReloadProcesses = new MvxCommand(LoadProcesses);
            InitTimer();
        }

        private ObservableCollection<Process> _processes = new ObservableCollection<Process>();

        private Process _selectedProcess;

        private static string _title = "Process:";

        public IMvxCommand ReloadProcesses { get; set; }
        public IMvxCommand SetReloadProcessesInterval { get; set; }

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

        private void InitTimer()
        {
            _reloadTimer = new System.Timers.Timer(2000);
            _reloadTimer.Elapsed += ReloadProcessesOnTimedEvent;
            _reloadTimer.AutoReset = true;
            _reloadTimer.Enabled = true;
        }

        private void ReloadProcessesOnTimedEvent(Object source, ElapsedEventArgs e)
        {
            LoadProcesses();
        }

        public void SetTimerInterval(int seconds)
        {
            _reloadTimer.Interval = seconds * 100;
        }
    }
}
