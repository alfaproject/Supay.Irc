using Microsoft.VisualStudio.TestTools.UnitTesting;
using Supay.Irc.Messages;

namespace Supay.Irc.Tests
{
    [TestClass]
    public class MonitorTests
    {
        [TestMethod]
        public void Monitor730Test()
        {
            var raws = new[] {
                ":irc.example.com 730 _aLfa_ _aLfa_!_aLfa_@comcast.net,supaybot!supaybot@example.com"
            };

            var msg = MessageAssert.TypeAndRoundTrip<MonitoredUserOnlineMessage>(raws[0]);
            Assert.AreEqual(2, msg.Users.Count);
            Assert.IsTrue(msg.Users.ContainsKey("_aLfa_"));
            Assert.IsTrue(msg.Users.ContainsKey("supaybot"));
        }

        [TestMethod]
        public void Monitor731Test()
        {
            var raws = new[] {
                ":irc.example.com 731 _aLfa_ supaybot,chanserv"
            };

            var msg = MessageAssert.TypeAndRoundTrip<MonitoredUserOfflineMessage>(raws[0]);
            Assert.AreEqual(2, msg.Nicks.Count);
            Assert.IsTrue(msg.Nicks.Contains("supaybot"));
            Assert.IsTrue(msg.Nicks.Contains("chanserv"));
        }

        [TestMethod]
        public void Monitor732Test()
        {
            var raws = new[] {
                ":irc.example.com 732 _aLfa_ supaybot,chanserv"
            };

            var msg = MessageAssert.TypeAndRoundTrip<MonitorListReplyMessage>(raws[0]);
            Assert.AreEqual(2, msg.Nicks.Count);
            Assert.IsTrue(msg.Nicks.Contains("supaybot"));
            Assert.IsTrue(msg.Nicks.Contains("chanserv"));
        }

        [TestMethod]
        public void Monitor733Test()
        {
            var raws = new[] {
                ":irc.example.com 733 _aLfa_ :End of MONITOR list"
            };

            MessageAssert.TypeAndRoundTrip<MonitorListEndReplyMessage>(raws[0]);
        }

        [TestMethod]
        public void Monitor734Test()
        {
            var raws = new[] {
                ":irc.example.com 734 _aLfa_ 100 supaybot,chanserv :Monitor list is full."
            };

            var msg = MessageAssert.TypeAndRoundTrip<MonitorListFullMessage>(raws[0]);
            Assert.AreEqual(2, msg.Nicks.Count);
            Assert.IsTrue(msg.Nicks.Contains("supaybot"));
            Assert.IsTrue(msg.Nicks.Contains("chanserv"));
            Assert.AreEqual(100, msg.Limit);
        }

        [TestMethod]
        public void MonitorClearTest()
        {
            var raws = new[] {
                ":hotspeed.sg.as.dal.net MONITOR C"
            };

            MessageAssert.TypeAndRoundTrip<MonitorListClearMessage>(raws[0]);
        }

        [TestMethod]
        public void WatchStatsTest()
        {
            var raws = new[] {
                ":hotspeed.sg.as.dal.net MONITOR S"
            };

            MessageAssert.TypeAndRoundTrip<MonitorStatusRequestMessage>(raws[0]);
        }

        [TestMethod]
        public void WatchListTest()
        {
            var raws = new[] {
                ":hotspeed.sg.as.dal.net MONITOR L"
            };

            MessageAssert.TypeAndRoundTrip<MonitorListRequestMessage>(raws[0]);
        }

        [TestMethod]
        public void MonitorAddTest()
        {
            var raws = new[] {
                ":hotspeed.sg.as.dal.net MONITOR + _aLfa_,foo",
                ":hotspeed.sg.as.dal.net MONITOR + ImNotHere"
            };

            var msg = MessageAssert.TypeAndRoundTrip<MonitorAddUsersMessage>(raws[0]);
            Assert.AreEqual(2, msg.Nicks.Count);
            Assert.IsTrue(msg.Nicks.Contains("_aLfa_"));
            Assert.IsTrue(msg.Nicks.Contains("foo"));

            msg = MessageAssert.TypeAndRoundTrip<MonitorAddUsersMessage>(raws[1]);
            Assert.AreEqual(1, msg.Nicks.Count);
            Assert.IsTrue(msg.Nicks.Contains("ImNotHere"));
        }

        [TestMethod]
        public void MonitorRemoveTest()
        {
            var raws = new[] {
                ":hotspeed.sg.as.dal.net MONITOR - _aLfa_"
            };

            var msg = MessageAssert.TypeAndRoundTrip<MonitorRemoveUsersMessage>(raws[0]);
            Assert.AreEqual(1, msg.Nicks.Count);
            Assert.IsTrue(msg.Nicks.Contains("_aLfa_"));
        }
    }
}
