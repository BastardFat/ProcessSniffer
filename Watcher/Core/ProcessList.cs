using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Watcher.Core
{
    public class ProcessList
    {
        public ProcessList()
        {
            ActualProcessList = new List<Models.Process>();
        }

        public List<Models.Process> ActualProcessList { get; set; } 


        public void UpdateProcessList()
        {

            var processes = System.Diagnostics.Process.GetProcesses();

            foreach (var process in processes)
            {
                if (process.MainWindowHandle != IntPtr.Zero)
                {
                    try
                    {
                        ActualProcessList.Add(new Models.Process(process));
                    }
                    catch {
                        var p = process;
                    }
                }
            }
        }

    }
}
