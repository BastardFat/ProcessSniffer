using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using Watcher.Core;
using Watcher.Warners;
using Watcher.Warners.Criterias;

namespace Client.ViewModels
{
    class MainViewModel : ViewModelBase
    {

        public MainViewModel()
        {
            ProcWatcher = new ProcessWatcher(1000);
            LightWarner = new ProcessWarner(ProcWatcher, new ProcessSelectionCriteria(Watcher.Models.Enums.Criterias.ProcessFields.Path, Watcher.Models.Enums.Criterias.SelectionCriteriaMode.StartWith, "D:\\"));
            HardWarner = new ProcessWarner(ProcWatcher, new ProcessSelectionCriteria(Watcher.Models.Enums.Criterias.ProcessFields.Name, Watcher.Models.Enums.Criterias.SelectionCriteriaMode.Contains, "paint"));
            Killer = new ProcessKiller(ProcWatcher, new ProcessSelectionCriteria(Watcher.Models.Enums.Criterias.ProcessFields.Title, Watcher.Models.Enums.Criterias.SelectionCriteriaMode.StartWith, "Диалоги"));

            HardWarner.ProcessWarning += (proc) =>
            {
                addEventToLog("Message", proc);
                WarnUserAboutStart(proc);
            };
            LightWarner.ProcessWarning += (proc) =>
            {
                addEventToLog("Notification", proc);
                // TODO : BaloonPopup!
            };
            Killer.ProcessWarning += (proc) =>
            {
                addEventToLog("Kill", proc);
                WarnUserAboutKill(proc);
            };

            ProcWatcher.StartProcess += (proc) =>
            {
                addEventToLog("Start", proc);
                RaisePropertyChanged(nameof(ActualProcesses));
            };

            ProcWatcher.TitleChanged += (proc) => RaisePropertyChanged(nameof(ActualProcesses));

            ProcWatcher.EndProcess += (proc) =>
            {
                addEventToLog("End", proc);
                RaisePropertyChanged(nameof(ActualProcesses));
            };

        }

        

        public ProcessWatcher ProcWatcher;
        public ProcessWarner LightWarner;
        public ProcessWarner HardWarner;
        public ProcessKiller Killer;


        public List<Watcher.Models.Process> ActualProcesses
        {
            get { return ProcWatcher.GetActualProcessList(); }
        }


        private List<Tuple<DateTime, string, string>> log = new List<Tuple<DateTime, string, string>>();
        public List<Tuple<DateTime, string, string>> Log { get { return log.ToList(); } }


        private void addEventToLog(string eventType, Watcher.Models.Process proc)
        {
            log.Add(new Tuple<DateTime, string, string>(DateTime.Now, eventType, proc.Name));
            RaisePropertyChanged(nameof(Log));
        }


        private void WarnUserAboutStart(Watcher.Models.Process proc)
        {
            System.Windows.MessageBox.Show(
                $"Process {proc.Name} is not recommended by your network administrator",
                "Warning",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Warning);
        }

        private void WarnUserAboutKill(Watcher.Models.Process proc)
        {
            System.Windows.MessageBox.Show(
                $"Process {proc.Name} is not allowed by your network administrator and was terminated!",
                "Termnated",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Stop);
        }

    }
}
