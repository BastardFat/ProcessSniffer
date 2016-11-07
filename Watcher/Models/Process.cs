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
            realProcess = process;
            Name = process.ProcessName;
            Pid = process.Id;
            Path = process.MainModule.FileName;
            Title = process.MainWindowTitle;
            StartTime = process.StartTime;
        }

        public string Name { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
        public int Pid { get; set; }
        public DateTime StartTime { get; set; }


        // TODO: Consider the possibility of getting rid of this thing
        public System.Diagnostics.Process RealProcess
        {
            get { return realProcess; }
        }
        private System.Diagnostics.Process realProcess;

        public override string ToString()
        {
            return Name;
        }

    }
}
