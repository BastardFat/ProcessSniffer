using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Watcher.Tools;
using System.Threading;
using Watcher.Core;
using System.Collections.Generic;
using Watcher.Models.Enums;

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
            Thread.Sleep(400);
            var t = Process.GetProcessesByName("calc")[0];
            t.CloseMainWindow();
            Thread.Sleep(400);
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
                Thread.Sleep(400);

            }

            Assert.IsTrue(started && ended, $"Started : {started}, ended : {ended}");

        }


    }
}