using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Timers;
using System.Windows;
using MvxTaskManager.Core.Services;

namespace MvxTaskManager.Core.ViewModels
{
    public class ProcessesViewModel : MvxViewModel
    {
        private System.Timers.Timer _reloadTimer;

        public ProcessesViewModel()
        {
            LoadProcesses();
            ReloadProcesses = new MvxCommand(LoadProcesses);
            SetReloadProcessesInterval = new MvxCommand(SetTimerInterval);
            InitTimer();
        }

        private ObservableCollection<Process> _processes = new ObservableCollection<Process>();
        private readonly SynchronizationContext _syncContext = SynchronizationContext.Current;

        private Process _selectedProcess;

        private string _reloadTimeText;

        public string ReloadTimeText
        {
            get { return _reloadTimeText; }
            set
            {
                _reloadTimeText = value;
                RaisePropertyChanged(() => ReloadTimeText);
            }
        }

        public int ReloadTimeSeconds { get; set; }

        public IMvxCommand ReloadProcesses { get; set; }
        public IMvxCommand SetReloadProcessesInterval { get; set; }

        public ObservableCollection<Process> Processes
        {
            get { return _processes; }
            set
            {
                if (_processes == value)
                    return;
                SetProperty(ref _processes, value);
                RaisePropertyChanged("Processes");
            }
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
            Processes = ProcessService.GetProcesses();
        }

        private void InitTimer()
        {
            ReloadTimeSeconds = 10000;
            _reloadTimer = new System.Timers.Timer(ReloadTimeSeconds);
            ReloadTimeText = (ReloadTimeSeconds / 1000).ToString();
            _reloadTimer.Elapsed += ReloadProcessesOnTimedEvent;
            _reloadTimer.AutoReset = true;
            _reloadTimer.Enabled = true;
        }

        private void ReloadProcessesOnTimedEvent(Object source, ElapsedEventArgs e)
        {
            _syncContext.Send(x => { Processes = ProcessService.GetProcesses(); }, null);
        }

        public void SetTimerInterval()
        {
            try
            {
                ReloadTimeSeconds = Int32.Parse(ReloadTimeText) * 1000;
                _reloadTimer.Interval = ReloadTimeSeconds;
            }
            catch (System.FormatException exception)
            {
                Console.WriteLine("Wrong input time" + exception.Message);
                ReloadTimeText = (ReloadTimeSeconds / 1000).ToString();
            }
        }
    }
}
