using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hephaistos_Maui.Models
{
    public class DaySchedule
    {
        public string Day { get; set; } = string.Empty;
        public ObservableCollection<ClassItem> Classes { get; set; } = new();
    }
}
