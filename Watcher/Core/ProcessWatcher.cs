using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Watcher.Tools;

namespace Watcher.Core
{
    public class ProcessWatcher : IDisposable
    {

        public ProcessWatcher(int refreshPeriod = 1000)
        {
            processList = new ProcessList();
            cycleThread = new CycleThread(
                () => 
                processList.UpdateProcessList()
                , refreshPeriod
                );

            cycleThread.StartCycle();
        }

        private ProcessList processList;
        private CycleThread cycleThread;

        public event Action<Models.Process> EndProcess
        {
            add { processList.EndProcess += value; }
            remove { processList.EndProcess -= value; }
        }

        public event Action<Models.Process> StartProcess
        {
            add { processList.StartProcess += value; }
            remove { processList.StartProcess -= value; }
        }

        public event Action<Models.Process> TitleChanged
        {
            add { processList.TitleChanged += value; }
            remove { processList.TitleChanged -= value; }
        }


        public List<Models.Process> GetActualProcessList() => processList.ActualProcessList;

        #region IDisposable Support
        private bool disposedValue = false;
        public bool Disposed { get { return disposedValue; } }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    cycleThread?.Dispose();
                }

                disposedValue = true;
            }
        }

        ~ProcessWatcher()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
