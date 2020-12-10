using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.ViewModels;
using MvxTaskManager.Core.ViewModels;

namespace MvxTaskManager.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<ProcessesViewModel>();
        }
    }
}
