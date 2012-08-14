using Microsoft.VisualStudio.TestTools.UnitTesting;
using Supay.Irc.Messages;

namespace Supay.Irc.Tests
{
    [TestClass]
    public class WatchTests
    {
        [TestMethod]
        public void Watch600Test()
        {
            var raws = new[] {
                ":hotspeed.sg.as.dal.net 600 _aLfa_ ImNotHere * * 0 :logged offline"
            };

            var msg = MessageAssert.TypeAndRoundTrip<WatchedUserNowOfflineMessage>(raws[0]);
            Assert.AreEqual(msg.WatchedUser.Nickname, "ImNotHere");
            Assert.AreEqual(msg.WatchedUser.Username, "*");
            Assert.AreEqual(msg.WatchedUser.Host, "*");
            Assert.AreEqual(MessageUtil.ConvertToUnixTime(msg.TimeOfChange), 0);
        }

        [TestMethod]
        public void Watch601Test()
        {
            var raws = new[] {
                ":hotspeed.sg.as.dal.net 601 _aLfa_ _aLfa_ _aLfa_ c-24-8-243-76.client.comcast.net 1082383302 :logged online"
            };

            var msg = MessageAssert.TypeAndRoundTrip<WatchedUserNowOnlineMessage>(raws[0]);
            Assert.AreEqual(msg.WatchedUser.Nickname, "_aLfa_");
            Assert.AreEqual(msg.WatchedUser.Username, "_aLfa_");
            Assert.AreEqual(msg.WatchedUser.Host, "c-24-8-243-76.client.comcast.net");
            Assert.AreEqual(MessageUtil.ConvertToUnixTime(msg.TimeOfChange), 1082383302);
        }

        [TestMethod]
        public void Watch602Test()
        {
            var raws = new[] {
                ":hotspeed.sg.as.dal.net 602 _aLfa_ alfa * * 0 :stopped watching",
                ":hotspeed.sg.as.dal.net 602 _aLfa_ _aLfa_ _aLfa_ c-24-8-243-76.client.comcast.net 1082383544 :stopped watching",
                ":hotspeed.sg.as.dal.net 602 _aLfa_ _aLfa_ _aLfa_ c-24-8-243-76.client.comcast.net 1082383572 :stopped watching"
            };

            var msg = MessageAssert.TypeAndRoundTrip<WatchStoppedMessage>(raws[0]);
            Assert.AreEqual(msg.WatchedUser.Nickname, "alfa");
            Assert.AreEqual(msg.WatchedUser.Username, "*");
            Assert.AreEqual(msg.WatchedUser.Host, "*");
            Assert.AreEqual(MessageUtil.ConvertToUnixTime(msg.TimeOfChange), 0);

            msg = MessageAssert.TypeAndRoundTrip<WatchStoppedMessage>(raws[1]);
            Assert.AreEqual(msg.WatchedUser.Nickname, "_aLfa_");
            Assert.AreEqual(msg.WatchedUser.Username, "_aLfa_");
            Assert.AreEqual(msg.WatchedUser.Host, "c-24-8-243-76.client.comcast.net");
            Assert.AreEqual(MessageUtil.ConvertToUnixTime(msg.TimeOfChange), 1082383544);

            msg = MessageAssert.TypeAndRoundTrip<WatchStoppedMessage>(raws[2]);
            Assert.AreEqual(msg.WatchedUser.Nickname, "_aLfa_");
            Assert.AreEqual(msg.WatchedUser.Username, "_aLfa_");
            Assert.AreEqual(msg.WatchedUser.Host, "c-24-8-243-76.client.comcast.net");
            Assert.AreEqual(MessageUtil.ConvertToUnixTime(msg.TimeOfChange), 1082383572);
        }

        [TestMethod]
        public void Watch603Test()
        {
            var raws = new[] {
                ":hotspeed.sg.as.dal.net 603 _aLfa_ :You have 0 and are on 0 WATCH entries",
                ":hotspeed.sg.as.dal.net 603 _aLfa_ :You have 1 and are on 1 WATCH entries"
            };

            var msg = MessageAssert.TypeAndRoundTrip<WatchStatusReplyMessage>(raws[0]);
            Assert.AreEqual(msg.WatchListCount, 0);
            Assert.AreEqual(msg.UsersWatchingYou, 0);

            msg = MessageAssert.TypeAndRoundTrip<WatchStatusReplyMessage>(raws[1]);
            Assert.AreEqual(msg.WatchListCount, 1);
            Assert.AreEqual(msg.UsersWatchingYou, 1);
        }

        [TestMethod]
        public void Watch604Test()
        {
            var raws = new[] {
                ":hotspeed.sg.as.dal.net 604 _aLfa_ _aLfa_ _aLfa_ c-24-8-243-76.client.comcast.net 1082383302 :is online"
            };

            var msg = MessageAssert.TypeAndRoundTrip<WatchedUserIsOnlineMessage>(raws[0]);
            Assert.AreEqual(msg.WatchedUser.Nickname, "_aLfa_");
            Assert.AreEqual(msg.WatchedUser.Username, "_aLfa_");
            Assert.AreEqual(msg.WatchedUser.Host, "c-24-8-243-76.client.comcast.net");
            Assert.AreEqual(MessageUtil.ConvertToUnixTime(msg.TimeOfChange), 1082383302);
        }

        [TestMethod]
        public void Watch605Test()
        {
            var raws = new[] {
                ":hotspeed.sg.as.dal.net 605 _aLfa_ ImNotHere * * 0 :is offline"
            };

            var msg = MessageAssert.TypeAndRoundTrip<WatchedUserIsOfflineMessage>(raws[0]);
            Assert.AreEqual(msg.WatchedUser.Nickname, "ImNotHere");
            Assert.AreEqual(msg.WatchedUser.Username, "*");
            Assert.AreEqual(msg.WatchedUser.Host, "*");
        }

        [TestMethod]
        public void Watch606Test()
        {
            var raws = new[] {
                ":mesra.kl.my.dal.net 606 _aLfa_ :supaybot2 supaybot"
            };

            var msg = MessageAssert.TypeAndRoundTrip<WatchStatusNicksReplyMessage>(raws[0]);
            Assert.AreEqual(2, msg.Nicks.Count);
            Assert.AreEqual("supaybot2", msg.Nicks[0]);
            Assert.AreEqual("supaybot", msg.Nicks[1]);
        }

        [TestMethod]
        public void Watch607Test()
        {
            var raws = new[] {
                ":hotspeed.sg.as.dal.net 607 _aLfa_ :End of WATCH l",
                ":hotspeed.sg.as.dal.net 607 _aLfa_ :End of WATCH L",
                ":hotspeed.sg.as.dal.net 607 _aLfa_ :End of WATCH s"
            };

            var msg = MessageAssert.TypeAndRoundTrip<WatchListEndReplyMessage>(raws[0]);
            Assert.AreEqual(msg.ListType, "l");

            msg = MessageAssert.TypeAndRoundTrip<WatchListEndReplyMessage>(raws[1]);
            Assert.AreEqual(msg.ListType, "L");

            msg = MessageAssert.TypeAndRoundTrip<WatchListEndReplyMessage>(raws[2]);
            Assert.AreEqual(msg.ListType, "s");
        }

        [TestMethod]
        public void WatchClearTest()
        {
            var raws = new[] {
                ":hotspeed.sg.as.dal.net WATCH C"
            };

            MessageAssert.TypeAndRoundTrip<WatchListClearMessage>(raws[0]);
        }

        [TestMethod]
        public void WatchStatsTest()
        {
            var raws = new[] {
                ":hotspeed.sg.as.dal.net WATCH S"
            };

            MessageAssert.TypeAndRoundTrip<WatchStatusRequestMessage>(raws[0]);
        }

        [TestMethod]
        public void WatchListTest()
        {
            var raws = new[] {
                ":hotspeed.sg.as.dal.net WATCH l",
                ":hotspeed.sg.as.dal.net WATCH L"
            };

            var msg = MessageAssert.TypeAndRoundTrip<WatchListRequestMessage>(raws[0]);
            Assert.IsTrue(msg.OnlineOnly);

            msg = MessageAssert.TypeAndRoundTrip<WatchListRequestMessage>(raws[1]);
            Assert.IsFalse(msg.OnlineOnly);
        }

        [TestMethod]
        public void WatchEditorTest()
        {
            var raws = new[] {
                ":hotspeed.sg.as.dal.net WATCH +_aLfa_ +foo",
                ":hotspeed.sg.as.dal.net WATCH +ImNotHere",
                ":hotspeed.sg.as.dal.net WATCH -alfa"
            };

            var msg = MessageAssert.TypeAndRoundTrip<WatchListEditorMessage>(raws[0]);
            Assert.AreEqual(2, msg.AddedNicks.Count);
            Assert.AreEqual(0, msg.RemovedNicks.Count);
            Assert.AreEqual("_aLfa_", msg.AddedNicks[0]);
            Assert.AreEqual("foo", msg.AddedNicks[1]);

            msg = MessageAssert.TypeAndRoundTrip<WatchListEditorMessage>(raws[1]);
            Assert.AreEqual(1, msg.AddedNicks.Count);
            Assert.AreEqual(0, msg.RemovedNicks.Count);
            Assert.AreEqual("ImNotHere", msg.AddedNicks[0]);

            msg = MessageAssert.TypeAndRoundTrip<WatchListEditorMessage>(raws[2]);
            Assert.AreEqual(0, msg.AddedNicks.Count);
            Assert.AreEqual(1, msg.RemovedNicks.Count);
            Assert.AreEqual("alfa", msg.RemovedNicks[0]);
        }
    }
}
