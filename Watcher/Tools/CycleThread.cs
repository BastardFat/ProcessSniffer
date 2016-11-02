using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Watcher.Models.Enums;

namespace Watcher.Tools
{
    public class CycleThread
    {
        public CycleThread(Action cycleBody, int refreshPeriod = 1000)
        {
            CycleBody = cycleBody;
            RefreshPeriod = refreshPeriod;
            State = CycleThreadStates.Stoped;
            ResetCycle();
        }


        private Task MainTask;
        public int RefreshPeriod { get; set; }

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

        private Action CycleBody;
        private volatile bool Stoping = false;
        private volatile bool Pausing = false;
        public CycleThreadStates State { get; private set; }


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

    }
}
