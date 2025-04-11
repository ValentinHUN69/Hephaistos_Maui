using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hephaistos_Maui.Models
{
    public class ClassItem
    {
        public string SubjectName { get; set; } = string.Empty;
        public string DayOfWeek { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;

        public string TimeRange => $"{StartTime[..5]} - {EndTime[..5]}";
    }
}
