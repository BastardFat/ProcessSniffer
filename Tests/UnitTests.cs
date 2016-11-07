using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Watcher.Tools;
using System.Threading;
using Watcher.Core;
using System.Collections.Generic;
using Watcher.Models.Enums;
using Watcher.Warners.Criterias;
using Watcher.Models.Enums.Criterias;

namespace Tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        [TestCategory(nameof(CycleThread))]
        public void CycleThreadTest()
        {

            var checklist = new Dictionary<int, CycleThreadStates>();

            checklist.Add(100, CycleThreadStates.Ready);
            checklist.Add(600, CycleThreadStates.Running);
            checklist.Add(1100, CycleThreadStates.Paused);
            checklist.Add(1600, CycleThreadStates.Running);
            checklist.Add(2100, CycleThreadStates.Stoped);
            checklist.Add(2600, CycleThreadStates.Ready);
            checklist.Add(3100, CycleThreadStates.Running);


            var started = DateTime.Now;
            CycleThread ct = new CycleThread(() =>
            {
                Debug.WriteLine("Working so hard...");
            }, 100);

            CycleThread checker = new CycleThread(() =>
            {
                var currentTimestamp = (DateTime.Now - started).TotalMilliseconds;
                foreach (var check in checklist)
                {
                    if (currentTimestamp >= check.Key && currentTimestamp <= check.Key + 300)
                    {
                        Debug.WriteLine($"currentTimestamp={currentTimestamp}; real={ct.State}; expected={check.Value};");
                        Assert.AreEqual(ct.State, check.Value);
                    }
                }
            }, 100);
            checker.StartCycle();
            Thread.Sleep(500);
            ct.StartCycle();
            Thread.Sleep(500);
            ct.PauseCycle();
            Thread.Sleep(500);
            ct.UnpauseCycle();
            Thread.Sleep(500);
            ct.StopCycle();
            Thread.Sleep(500);
            ct.ResetCycle();
            Thread.Sleep(500);
            ct.StartCycle();
            Thread.Sleep(500);
            ct.PauseCycle();
            Thread.Sleep(500);
            ct.StopCycle();
            Thread.Sleep(500);
            checker.Dispose();
            Assert.AreEqual(ct.State, CycleThreadStates.Stoped);

        }

        [TestMethod]
        [TestCategory(nameof(ProcessList))]
        public void ProcessListTest()
        {
            ProcessList pl = new ProcessList();
            pl.UpdateProcessList();
            Assert.IsTrue(pl.ActualProcessList.Count > 0);
        }

        [TestMethod]
        [TestCategory(nameof(ProcessList))]
        public void ProcessListEventsTest()
        {
            ProcessList pl = new ProcessList();
            bool started = false;
            bool ended = false;
            pl.StartProcess += (p) =>
            {
                if (p.Name.ToLower().StartsWith("calc"))
                {
                    started = true;
                }
            };
            pl.EndProcess += (p) =>
            {
                if (p.Name.ToLower().StartsWith("calc"))
                {
                    ended = true;
                }
            };

            CycleThread ct = new CycleThread(() =>
            {
                pl.UpdateProcessList();
            }, 200);
            ct.StartCycle();

            Thread.Sleep(400);
            Process.Start("calc.exe");
            Thread.Sleep(1000);
            var t = Process.GetProcessesByName("calc")[0];
            t.CloseMainWindow();
            t.WaitForExit();
            Thread.Sleep(1000);
            ct.Dispose();
            Assert.IsTrue(started && ended, $"Started : {started}, ended : {ended}");
        }

        [TestMethod]
        [TestCategory(nameof(ProcessWatcher))]
        public void ProcessWatcherEventsTest()
        {
            bool started = false;
            bool ended = false;
            using (ProcessWatcher w = new ProcessWatcher(200))
            {
                w.StartProcess += (p) =>
                {
                    if (p.Name.ToLower().StartsWith("calc"))
                    {
                        started = true;
                    }
                };
                w.EndProcess += (p) =>
                {
                    if (p.Name.ToLower().StartsWith("calc"))
                    {
                        ended = true;
                    }
                };

                Thread.Sleep(400);
                Process.Start("calc.exe");
                Thread.Sleep(400);
                var t = Process.GetProcessesByName("calc")[0];
                t.CloseMainWindow();
                t.WaitForExit();
                Thread.Sleep(400);
            }

            Assert.IsTrue(started && ended, $"Started : {started}, ended : {ended}");

        }

        [TestMethod]
        [TestCategory(nameof(SelectionCriteria))]
        public void ProcessSelectionCriteriaTest()
        {
            foreach (var proc in Process.GetProcessesByName("calc"))
            {
                proc.CloseMainWindow();
            }


            Process.Start("calc.exe");
            Thread.Sleep(200);
            
            SelectionCriteria criteria = new ProcessSelectionCriteria(ProcessFields.Name, SelectionCriteriaMode.StartWith, "calc");

            ProcessList pl = new ProcessList();
            pl.UpdateProcessList();


            int checkNumber = 0;
            foreach (var proc in pl.ActualProcessList)
            {
                if (criteria.CheckCriteria(proc)) checkNumber++;
            }
            Assert.AreEqual(checkNumber, 1);
                            
            var t = Process.GetProcessesByName("calc")[0];
            t.CloseMainWindow();
            t.WaitForExit();
            pl.UpdateProcessList();
            checkNumber = 0;
            foreach (var proc in pl.ActualProcessList)
            {
                if (criteria.CheckCriteria(proc)) checkNumber++;
            }
            Assert.AreEqual(checkNumber, 0);
        }

        [TestMethod]
        [TestCategory(nameof(SelectionCriteria))]
        public void SelectionCriteriaSerializationTest()
        {
            SelectionCriteria criteria = new OrSelectionCriteria
            (
                new ProcessSelectionCriteria(ProcessFields.Path, SelectionCriteriaMode.StartWith, "D:"),
                new AndSelectionCriteria
                (
                    new ProcessSelectionCriteria(ProcessFields.Title, SelectionCriteriaMode.Contains, "'\t\n\'\"\\\\"),
                    new NotSelectionCriteria(new ProcessSelectionCriteria(ProcessFields.Name, SelectionCriteriaMode.Equal, "{{")),
                    new OrSelectionCriteria
                    (
                        new ProcessSelectionCriteria(ProcessFields.Path, SelectionCriteriaMode.StartWith, "D:\\"),
                        new NotSelectionCriteria(new ProcessSelectionCriteria(ProcessFields.Path, SelectionCriteriaMode.StartWith, "    } \n\t  \\n\\t"))
                    )
                )
            );

            var newcriteria = SelectionCriteria.CreateFromJson(criteria.ToJson());



            Assert.AreEqual(criteria.ToString(), newcriteria.ToString());
        }

    }
}