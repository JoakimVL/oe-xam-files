using System;
using System.Collections.Generic;
using System.Text;

namespace FilesExercise.Domain.Models
{
    public class UserSettings
    {
        public string UserName { get; set; }
        public bool ReceiveWeeklyStats { get; set; }

        public string Email { get; set; }
    }
}
