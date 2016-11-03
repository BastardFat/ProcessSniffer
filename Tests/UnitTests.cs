using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Watcher.Tools;
using System.Threading;
using Watcher.Core;

namespace Tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void CycleThreadTest()
        {
            var started = DateTime.Now;

            CycleThread ct = new CycleThread(() =>
            {
                Debug.WriteLine($"[{(DateTime.Now - started).TotalMilliseconds}] Doing some work...");
            }, 200);

            Thread.Sleep(2000);
            ct.StartCycle();
            Thread.Sleep(2000);
            ct.PauseCycle();
            Thread.Sleep(2000);
            ct.UnpauseCycle();
            Thread.Sleep(2000);
            ct.StopCycle();
            Thread.Sleep(2000);
            ct.ResetCycle();
            Thread.Sleep(2000);
            ct.StartCycle();
            Thread.Sleep(2000);
            ct.PauseCycle();
            Thread.Sleep(2000);
            ct.StopCycle();
            Thread.Sleep(2000);
        }
        [TestMethod]
        public void ProcessListTest()
        {
            ProcessList pl = new ProcessList();
            pl.UpdateProcessList();
        }

        [TestMethod]
        public void ProcessListEventsTest()
        {
            ProcessList pl = new ProcessList();

            pl.StartProcess += (proc) =>
            {
                Debug.WriteLine($"Started {proc.Name} [{proc.Pid}]");
            };
            pl.EndProcess += (proc) =>
            {
                Debug.WriteLine($"Ended {proc.Name} [{proc.Pid}]");
            };
            CycleThread ct = new CycleThread(() =>
            {
                pl.UpdateProcessList();
            }, 1000);
            ct.StartCycle();

            Thread.Sleep(10 * 1000);

        }

    }

}