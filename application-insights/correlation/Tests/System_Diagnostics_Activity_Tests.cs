using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace Tests
{
    [TestClass]
    public class System_Diagnostics_Activity_Tests
    {
        [TestMethod]
        public void CreateActivityAndCheckingCurrentActivityWithoutStarting_WillBeNull()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity act = new Activity("myop");

            act.SetParentId("abcd");
            string actualActivityId = act.Id;
            
            Assert.IsNull(Activity.Current);
            
        }
        [TestMethod]
        public void CreateActivityAndCheckingCurrentActivityWithStarting_WillNotBeNull()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity act = new Activity("myop");

            act.SetParentId("abcd");
            string actualActivityId = act.Id;
            act.Start();
            Assert.IsNull(Activity.Current);
            act.Stop();
        }
        [TestMethod]
        public void CreateActivityAndCheckingCurrentActivityAfterStartAndStop_WillBeNull()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity act = new Activity("myop");

            act.SetParentId("abcd");
            string actualActivityId = act.Id;
            act.Start();
            act.Stop();

            Assert.IsNull(Activity.Current);
        }
        [TestMethod]
        public void SettingTheParentId_()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity act = new Activity("myop");
            
            act.SetParentId("abcd");
            string actualActivityId = act.Id;
            act.Start();
            Assert.IsTrue( act.Id.Contains("abcd"));
            act.Stop();
        }
    }
}
