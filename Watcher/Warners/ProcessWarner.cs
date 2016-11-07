using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watcher.Core;
using Watcher.Warners.Criterias;

namespace Watcher.Warners
{
    public class ProcessWarner
    {
        public ProcessWarner(ProcessWatcher watcher)
        {
            _watcher = watcher;

            _watcher.StartProcess += StartProcessHandler;

        }
        public ProcessWarner(ProcessWatcher watcher, SelectionCriteria criteria) : this(watcher)
        {
            WarningCriteria = criteria;
        }

        

        public event Action<Models.Process> ProcessWarning = delegate { };
        public event Action<SelectionCriteria> SelectionCriteriaChanged = delegate { };

        protected virtual void StartProcessHandler(Models.Process process)
        {
            if (WarningCriteria?.CheckCriteria(process) == true) WarnAboutProcess(process);
        }

        protected void WarnAboutProcess(Models.Process process)
        {
            ProcessWarning?.Invoke(process);
        }


        protected ProcessWatcher _watcher;

        protected SelectionCriteria warningCriteria;
        public SelectionCriteria WarningCriteria
        {
            get
            {
                return warningCriteria;
            }
            set
            {
                if (warningCriteria?.ToString() != value?.ToString())
                {
                    warningCriteria = value;
                    SelectionCriteriaChanged?.Invoke(value);
                }
            }
        }
    }
}
