using System;
using System.Collections.Generic;
using System.Text;

namespace MvxTaskManager.Core.Services
{
    class TextValidationService
    {
        public static bool IsTextNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            return reg.IsMatch(str);
        }
    }
}
