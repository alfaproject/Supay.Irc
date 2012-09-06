using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Supay.Irc.Messages;

namespace Supay.Irc.Tests
{
    [TestClass]
    public class UtilityTests
    {
        [TestMethod]
        public void CtcpEscape()
        {
            var data = "foo bar zap!";
            var escaped = CtcpUtil.Escape(data);
            var unescaped = CtcpUtil.Unescape(escaped);
            Assert.AreEqual("foo bar zap!", escaped, "ctcp escaped");
            Assert.AreEqual(data, unescaped, "ctcp escape round trip");

            data = "foo\r\nbar\r\nzap!";
            escaped = CtcpUtil.Escape(data);
            unescaped = CtcpUtil.Unescape(escaped);
            Assert.AreEqual("foo\x0014r\x0014nbar\x0014r\x0014nzap!", escaped, "ctcp escaped");
            Assert.AreEqual(data, unescaped, "ctcp escape round trip");
        }

        [TestMethod]
        public void EnsureValidChannelName()
        {
            var defaultSupport = new ServerSupport();

            Assert.AreEqual("#foo", MessageUtil.EnsureValidChannelName("#foo", defaultSupport));
            Assert.AreEqual("#foo", MessageUtil.EnsureValidChannelName("foo", defaultSupport));
            Assert.AreEqual("#", MessageUtil.EnsureValidChannelName("#", defaultSupport));
            Assert.AreEqual("#irc", MessageUtil.EnsureValidChannelName("", defaultSupport));
        }

        [TestMethod]
        public void GetPrefix()
        {
            Assert.AreEqual("^Care|wrk!~carebear@212.4.54.137", MessageUtil.GetPrefix(":^Care|wrk!~carebear@212.4.54.137 PRIVMSG _aLfa_ :\x0001DCC SEND versions.txt 3557045897 1111 42740\x0001"));
            Assert.AreEqual("JoaoDias!_aLfa_@12-255-177-172.client.attbi.com", MessageUtil.GetPrefix(":JoaoDias!_aLfa_@12-255-177-172.client.attbi.com PRIVMSG _aLfa_ :\x0001DCC CHAT chat 218083756 2777\x0001"));
            Assert.AreEqual("irc2.secsup.org", MessageUtil.GetPrefix(":irc2.secsup.org 433 _aLfa_ COBOL :Nickname is already in use."));
            Assert.AreEqual("", MessageUtil.GetPrefix("PING GDN-7T4JZ11"));
            Assert.AreEqual("COBOL!~COBOL@ool-435015b7.dyn.optonline.net", MessageUtil.GetPrefix(":COBOL!~COBOL@ool-435015b7.dyn.optonline.net MODE #ms.net +o _aLfa_"));
            Assert.AreEqual("irc.easynews.com", MessageUtil.GetPrefix(":irc.easynews.com 005 _aLfa_ WALLCHOPS KNOCK EXCEPTS INVEX MODES=4 MAXCHANNELS=35 MAXBANS=25 MAXTARGETS=4 NICKLEN=9 TOPICLEN=120 KICKLEN=120 :are supported by this server"));
        }

        [TestMethod]
        public void GetCommand()
        {
            Assert.AreEqual("PRIVMSG", MessageUtil.GetCommand(":^Care|wrk!~carebear@212.4.54.137 PRIVMSG _aLfa_ :\x0001DCC SEND versions.txt 3557045897 1111 42740\x0001"));
            Assert.AreEqual("432", MessageUtil.GetCommand(":irc2.secsup.org 432 _aLfa_ #foo :Erroneus Nickname"));
            Assert.AreEqual("433", MessageUtil.GetCommand(":irc2.secsup.org 433 _aLfa_ COBOL :Nickname is already in use."));
            Assert.AreEqual("PING", MessageUtil.GetCommand("PING GDN-7T4JZ11"));
            Assert.AreEqual("MODE", MessageUtil.GetCommand(":COBOL!~COBOL@ool-435015b7.dyn.optonline.net MODE #ms.net +o _aLfa_"));
            Assert.AreEqual("005", MessageUtil.GetCommand(":irc.easynews.com 005 _aLfa_ WALLCHOPS KNOCK EXCEPTS INVEX MODES=4 MAXCHANNELS=35 MAXBANS=25 MAXTARGETS=4 NICKLEN=9 TOPICLEN=120 KICKLEN=120 :are supported by this server"));
        }

        [TestMethod]
        public void GetParameters()
        {
            string input = "COMMAND";
            var p = MessageUtil.GetParameters(input);
            Assert.AreEqual(0, p.Count);

            input = "COMMAND param1";
            p = MessageUtil.GetParameters(input);
            Assert.AreEqual(1, p.Count);
            Assert.AreEqual("param1", p[0]);

            input = "COMMAND param1 param2";
            p = MessageUtil.GetParameters(input);
            Assert.AreEqual(2, p.Count);
            Assert.AreEqual("param1", p[0]);
            Assert.AreEqual("param2", p[1]);

            input = ":nick!user@host.com COMMAND";
            p = MessageUtil.GetParameters(input);
            Assert.AreEqual(0, p.Count);

            input = ":nick!user@host.com COMMAND param1";
            p = MessageUtil.GetParameters(input);
            Assert.AreEqual(1, p.Count);
            Assert.AreEqual("param1", p[0]);

            input = ":nick!user@host.com COMMAND param1 param2";
            p = MessageUtil.GetParameters(input);
            Assert.AreEqual(2, p.Count);
            Assert.AreEqual("param1", p[0]);
            Assert.AreEqual("param2", p[1]);
        }

        [TestMethod]
        public void GetLastParameter()
        {
            Assert.AreEqual("\x0001DCC SEND versions.txt 3557045897 1111 42740\x0001", MessageUtil.GetParameters(":^Care|wrk!~carebear@212.4.54.137 PRIVMSG _aLfa_ :\x0001DCC SEND versions.txt 3557045897 1111 42740\x0001").Last());
            Assert.AreEqual("Erroneus Nickname", MessageUtil.GetParameters(":irc2.secsup.org 432 _aLfa_ #foo :Erroneus Nickname").Last());
            Assert.AreEqual("Nickname is already in use.", MessageUtil.GetParameters(":irc2.secsup.org 433 _aLfa_ COBOL :Nickname is already in use.").Last());
            Assert.AreEqual("GDN-7T4JZ11", MessageUtil.GetParameters("PING GDN-7T4JZ11").Last());
            Assert.AreEqual("_aLfa_", MessageUtil.GetParameters(":COBOL!~COBOL@ool-435015b7.dyn.optonline.net MODE #ms.net +o _aLfa_").Last());
            Assert.AreEqual("are supported by this server", MessageUtil.GetParameters(":irc.easynews.com 005 _aLfa_ WALLCHOPS KNOCK EXCEPTS INVEX MODES=4 MAXCHANNELS=35 MAXBANS=25 MAXTARGETS=4 NICKLEN=9 TOPICLEN=120 KICKLEN=120 :are supported by this server").Last());
        }

        [TestMethod]
        public void StringBetweenStrings()
        {
            Assert.AreEqual("bar", MessageUtil.StringBetweenStrings("foobarzap", "foo", "zap"));
            Assert.AreEqual("foo", MessageUtil.StringBetweenStrings("foobarzap", "", "barzap"));
            Assert.AreEqual("zap", MessageUtil.StringBetweenStrings("foobarzap", "foobar", ""));
            Assert.AreEqual("", MessageUtil.StringBetweenStrings("foobarzap", "foobarzap", ""));
            Assert.AreEqual("", MessageUtil.StringBetweenStrings("foobarzap", "", "foobarzap"));
            Assert.AreEqual("foobarzap", MessageUtil.StringBetweenStrings("foobarzap", "q", "t"));
            Assert.AreEqual("zap", MessageUtil.StringBetweenStrings("foobarzap", "foobar", "t"));
            Assert.AreEqual("foo", MessageUtil.StringBetweenStrings("foobarzap", "t", "barzap"));
        }

        [TestMethod]
        public void UnixTime()
        {
            var dateTime = new DateTime(2003, 12, 31, 13, 23, 45, DateTimeKind.Utc);
            const int unixTimeStamp = 1072877025;

            Assert.AreEqual(unixTimeStamp, MessageUtil.ConvertToUnixTime(dateTime));
            Assert.AreEqual(dateTime, MessageUtil.ConvertFromUnixTime(unixTimeStamp));
            Assert.AreEqual(dateTime, MessageUtil.ConvertFromUnixTime(MessageUtil.ConvertToUnixTime(dateTime)));
        }
    }
}
