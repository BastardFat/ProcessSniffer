using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Watcher.Models
{
    public class Process
    {
        public Process(System.Diagnostics.Process process)
        {
            Name = process.ProcessName;
            Pid = process.Id;
            Path = process.MainModule.FileName;
            Title = process.MainWindowTitle;
            StartTime = process.StartTime;
        }

        public string Name { get; }
        public string Path { get; }
        public string Title { get; }
        public int Pid { get; }
        public DateTime StartTime { get; }


        public System.Diagnostics.Process FindByPID() =>
            System.Diagnostics.Process.GetProcessById(Pid);


        public override string ToString()
        {
            return Name;
        }

    }
}
