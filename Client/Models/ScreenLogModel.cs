using System;

namespace Client.Models
{
    public class ScreenLogModel
    {
        public DateTime Time { get; set; }
        public EventTypes EventType { get; set; }
        public string ProcessName { get; set; }
    }
}
