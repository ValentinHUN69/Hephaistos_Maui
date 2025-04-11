using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hephaistos_Maui.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int? StartYear { get; set; }  
        public string Status { get; set; } = string.Empty;
        public bool Active { get; set; } = true;

        public string? MajorId { get; set; } 
        public string MajorName { get; set; } = string.Empty;
    }
}
