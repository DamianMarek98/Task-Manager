using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Timers;
using MvxTaskManager.Core.Models;
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
            SetSelectedProcessCommand = new MvxCommand(SetSelectedProcess);
            SaveSelectedProcessPriority = new MvxCommand(SaveSelectedProcess);
            KillSelectedProcessCommand = new MvxCommand(KillSelectedProcess);
            StartSupportingCommand = new MvxCommand(StartSupporting);
            StopSupportingCommand = new MvxCommand(StopSupporting);
            InitTimer();
        }

        public IMvxCommand ReloadProcesses { get; set; }
        public IMvxCommand SetReloadProcessesInterval { get; set; }
        public IMvxCommand SetSelectedProcessCommand { get; set; }
        public IMvxCommand SaveSelectedProcessPriority { get; set; }
        public IMvxCommand KillSelectedProcessCommand { get; set; }
        public IMvxCommand StartSupportingCommand { get; set; }
        public IMvxCommand StopSupportingCommand { get; set; }

        private ObservableCollection<ProcessModel> _processes = new ObservableCollection<ProcessModel>();
        private ObservableCollection<SupportedProcessModel> _supportedProcesses = new ObservableCollection<SupportedProcessModel>();
        private readonly SynchronizationContext _syncContext = SynchronizationContext.Current;

        private ProcessModel _selectedProcess;

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


        public ObservableCollection<ProcessModel> Processes
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

        public ObservableCollection<SupportedProcessModel> SupportedProcesses
        {
            get { return _supportedProcesses; }
            set
            {
                if (_supportedProcesses == value)
                    return;
                SetProperty(ref _supportedProcesses, value);
            }
        }

        public ProcessModel SelectedProcess
        {
            get { return _selectedProcess; }
            set
            {
                _selectedProcess = value;
            }
        }


        private int _selectedPid;

        public int SelectedPID
        {
            get => _selectedPid;
            set => SetProperty(ref _selectedPid, value);
        }

        private string _selectedName;

        public string SelectedName
        {
            get => _selectedName;
            set => SetProperty(ref _selectedName, value);
        }

        private string _selectedWorkingSet;

        public string SelectedWorkingSet
        {
            get => _selectedWorkingSet;
            set => SetProperty(ref _selectedWorkingSet, value);
        }


        private ProcessPriorityClass _selectedPriority;
        public ProcessPriorityClass SelectedPriority
        {
            get => _selectedPriority;
            set => SetProperty(ref _selectedPriority, value);
        }

        private bool _selectedHierarchyVisibile;
        public bool SelectedHierarchyVisible
        {
            get => _selectedHierarchyVisibile;
            set => SetProperty(ref _selectedHierarchyVisibile, value);
        }


        public void LoadProcesses()
        {
            Processes.Clear();
            Processes = ProcessService.GetProcesses(SupportedProcesses);
        }

        public void SetSelectedProcess()
        {
            if (SelectedProcess != null)
            {

                var p = Process.GetProcessById(SelectedProcess.PID);
                SelectedPID = p.Id;
                SelectedName = p.ProcessName;
                SelectedWorkingSet = $"{(p.WorkingSet64 / 1024.00 / 1024.00).ToString("0.##")} MB";
                try
                {
                    SelectedPriority = p.PriorityClass;
                    SelectedHierarchyVisible = true;
                }
                catch (Exception)
                {
                    SelectedHierarchyVisible = false;
                }
            }
        }

        public void SaveSelectedProcess()
        {
            if (SelectedPID != 0 && SelectedHierarchyVisible == true)
            {
                var p = Process.GetProcessById(SelectedPID);
                p.PriorityClass = SelectedPriority;
                LoadProcesses();
            }
        }

        public void KillSelectedProcess()
        {
            if (SelectedPID != 0)
            {
                Process process = Process.GetProcessById(SelectedPID);
                process.Kill();
                Processes.Remove(SelectedProcess);
            }
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
            _syncContext.Send(x => { Processes = ProcessService.GetProcesses(SupportedProcesses); }, null);
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

        public void StartSupporting()
        {
            if (SelectedPID != 0)
            {
                var process = Process.GetProcessById(SelectedPID);
                SupportedProcesses.Add(new SupportedProcessModel(SelectedPID, process.MainModule.FileName));
                LoadProcesses();
            }
        }

        public void StopSupporting()
        {
            if (SelectedPID != 0)
            {
                var processToStopSupport = SupportedProcesses.FirstOrDefault(p => p.PID == SelectedPID);
                if (processToStopSupport != null)
                {
                    SupportedProcesses.Remove(processToStopSupport);
                    LoadProcesses();
                }
            }
        }
    }
}
