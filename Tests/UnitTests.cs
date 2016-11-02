using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Watcher.Tools;
using System.Threading;

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
                Console.WriteLine($"[{(DateTime.Now - started).TotalMilliseconds}] Doing some work...");
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
    }
}
