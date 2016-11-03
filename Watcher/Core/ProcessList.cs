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

        public event Action<Models.Process> StartProcess = delegate { };
        public event Action<Models.Process> EndProcess = delegate { };


        public void UpdateProcessList()
        {
            var newList = GetProcessList().ToList();
            foreach (var newProc in newList)
            {
                if (!ActualProcessList.Any((oldProc) => oldProc.Pid == newProc.Pid))
                    StartProcess?.Invoke(newProc);
            }
            foreach (var oldProc in ActualProcessList)
            {
                if (!newList.Any((newProc) => oldProc.Pid == newProc.Pid))
                    EndProcess?.Invoke(oldProc);
            }
            ActualProcessList = newList;
        }

        private IEnumerable<Models.Process> GetProcessList()
        {
            var processes = System.Diagnostics.Process.GetProcesses();

            foreach (var process in processes)
            {
                if (process.MainWindowHandle != IntPtr.Zero)
                {
                    Models.Process t = null;
                    try
                    {
                        t = new Models.Process(process);
                    }
                    catch { }
                    if (t != null) yield return t;
                }
            }
        }

    }
}
