using System;
using System.Collections.Generic;
using System.Text;

namespace MvxTaskManager.Core.Models
{
    public class ProcessModel
    {
        public ProcessModel(int pid, string name, long workingSet, int basePriority, bool isTracked = false)
        {
            PID = pid;
            Name = name;
            WorkingSet = workingSet;
            BasePriority = basePriority;
            IsTracked = isTracked;
        }

        public int PID { get; set; }
        public string Name { get; set; }
        public long WorkingSet { get; set; }
        public int BasePriority { get; set; }
        public bool IsTracked { get; set; } = false;
    }
}
