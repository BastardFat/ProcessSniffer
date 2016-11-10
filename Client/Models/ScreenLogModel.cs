using System;

namespace Client.Models
{
    public class ScreenLogModel
    {
        public ScreenLogModel(DateTime time, EventTypes eventType, string processName)
        {
            Time = time;
            EventType = eventType;
            ProcessName = processName;
        }

        public DateTime Time { get; set; }
        public EventTypes EventType { get; set; }
        public string ProcessName { get; set; }
    }
}
