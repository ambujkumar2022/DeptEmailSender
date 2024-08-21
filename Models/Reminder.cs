using System;

namespace DeptEmailSender.Models
{
    public class Reminder
    {
        public int RemId { get; set; }
        public string RemTitle { get; set; }
        public DateTime ReminderDateTime { get; set; } 
        public bool IsEmailSent { get; set; }
    }
}
