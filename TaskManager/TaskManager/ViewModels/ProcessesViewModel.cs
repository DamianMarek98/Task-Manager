using System;
using System.Collections.Generic;
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
        private BindableCollection<ProcessModel> _processes = new BindableCollection<ProcessModel>();
        private ProcessModel _selectedProcess;

        public ProcessesViewModel()
        {
            
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }


        public BindableCollection<ProcessModel> Processes
        {
            get { return _processes; }
            set { _processes = value; }
        }


        public ProcessModel SelectedProcess
        {
            get { return _selectedProcess; }
            set
            {
                _selectedProcess = value;
                NotifyOfPropertyChange(() => SelectedProcess);
            }
        }
    }
}
