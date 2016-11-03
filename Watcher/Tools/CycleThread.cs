using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Watcher.Models.Enums;

namespace Watcher.Tools
{
    public class CycleThread : IDisposable
    {
        public CycleThread(Action cycleBody, int refreshPeriod = 1000)
        {
            CycleBody = cycleBody;
            RefreshPeriod = refreshPeriod;
            State = CycleThreadStates.Stoped;
            ResetCycle();
        }

        public int RefreshPeriod { get; set; }
        public CycleThreadStates State { get; private set; }

        public void StartCycle()
        {
            if (State == CycleThreadStates.Ready)
            {
                MainTask.Start();
            }
        }

        public void ResetCycle()
        {
            if (State == CycleThreadStates.Stoped)
            {
                StopCycle();
                Stoping = false;
                MainTask = new Task(TaskBody);
                State = CycleThreadStates.Ready;
            }
        }

        public void StopCycle()
        {
            if (State == CycleThreadStates.Paused || State == CycleThreadStates.Running)
            {
                Stoping = true;
                MainTask?.Wait();
            }
        }

        public void PauseCycle()
        {
            if (State == CycleThreadStates.Running)
                Pausing = true;
        }

        public void UnpauseCycle()
        {
            if (State == CycleThreadStates.Paused)
                Pausing = false;
        }


        private Task MainTask;
        private Action CycleBody;
        private volatile bool Stoping = false;
        private volatile bool Pausing = false;
        


        private void TaskBody()
        {
            while(!Stoping)
            {
                State = CycleThreadStates.Running;
                Thread.Sleep(RefreshPeriod);
                CycleBody?.Invoke();
                while (Pausing && !Stoping) State = CycleThreadStates.Paused;
                State = CycleThreadStates.Running;
            }
            State = CycleThreadStates.Stoped;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    StopCycle();
                    MainTask?.Dispose();
                }
                disposedValue = true;
            }
        }

        ~CycleThread()
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
