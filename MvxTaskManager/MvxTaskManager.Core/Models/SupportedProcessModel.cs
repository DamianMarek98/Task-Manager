using System;
using System.Collections.Generic;
using System.Text;

namespace MvxTaskManager.Core.Models
{
    public class SupportedProcessModel
    {
        public SupportedProcessModel(int pid, string filePath)
        {
            PID = pid;
            FilePath = filePath;
        }

        public int PID { get; set; }
        public string FilePath { get; set; }
    }
}
