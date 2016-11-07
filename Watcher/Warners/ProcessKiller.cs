using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watcher.Core;
using Watcher.Models;
using Watcher.Warners.Criterias;

namespace Watcher.Warners
{
    public class ProcessKiller : ProcessWarner
    {
        public ProcessKiller(ProcessWatcher watcher) : base(watcher)
        {
            CloseTimeout = DefaultCloseTimeout;
        }
        public ProcessKiller(ProcessWatcher watcher, SelectionCriteria criteria) : base(watcher, criteria)
        {
            CloseTimeout = DefaultCloseTimeout;
        }
        public ProcessKiller(ProcessWatcher watcher, int closeTimeout) : base(watcher)
        {
            CloseTimeout = closeTimeout;
        }
        public ProcessKiller(ProcessWatcher watcher, SelectionCriteria criteria, int closeTimeout) : base(watcher, criteria)
        {
            CloseTimeout = closeTimeout;
        }


        public int CloseTimeout { get; }

        protected override void StartProcessHandler(Process process)
        {

            if (WarningCriteria?.CheckCriteria(process) == false)
                return;

            WarnAboutProcess(process);


            var processForKilling = process.FindByPID();

            Task killTask = new Task(() =>
            {
                if (processForKilling.CloseMainWindow())
                {
                    processForKilling.WaitForExit(CloseTimeout);
                    if (processForKilling.HasExited) return;
                }
                processForKilling.Kill();
            });

            killTask.Start();


        }



        private const int DefaultCloseTimeout = 3000;

    }
}
