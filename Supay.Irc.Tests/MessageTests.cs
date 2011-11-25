using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Supay.Irc.Messages;

namespace Supay.Irc.Tests
{
  [TestClass]
  public class MessageTests
  {
    [TestMethod]
    public void ParseDetermine()
    {
      var raws = new[] {
        ":foo!bar@zap.com PRIVMSG _aLfa_ :hello, how are you today?",
        ":foo!bar@zap.com NOTICE _aLfa_ :hello, how are you today?",
        ":foo!bar@zap.com PRIVMSG _aLfa_ :\x0001ACTION does some tests\x0001",
        ":foo!bar@zap.com 001 _aLfa_ :- Welcome To This IRC Server",
        ":foo!bar@zap.com NOTEXIST _aLfa_ :This Is Not In The RFC",
        "PRIVMSG _aLfa_ :hello, how are you today?",
        "NOTICE _aLfa_ :hello, how are you today?",
        "PRIVMSG _aLfa_ :\x0001ACTION does some tests\x0001",
        "001 _aLfa_ :- Welcome To This IRC Server",
        "NOTEXIST _aLfa_ :This Is Not In The RFC",
        ":irc.easynews.com 001 SupayBot :Welcome to the EFNet IRC via EasyNews SupayBot",
        ":irc.easynews.com 375 SupayBot :- irc.easynews.com funky MOTD, read slow! -",
        @":irc.easynews.com 353 _aLfa_ = #smack @_aLfa_",
        @":irc.easynews.com 366 _aLfa_ #smack :End of /NAMES list."
      };

      MessageAssert.TypeAndRoundTrip<ChatMessage>(raws[0]);
      MessageAssert.TypeAndRoundTrip<NoticeMessage>(raws[1]);
      MessageAssert.TypeAndRoundTrip<ActionRequestMessage>(raws[2]);
      MessageAssert.TypeAndRoundTrip<NumericMessage>(raws[3]);
      MessageAssert.TypeAndRoundTrip<GenericMessage>(raws[4]);
      MessageAssert.TypeAndRoundTrip<ChatMessage>(raws[5]);
      MessageAssert.TypeAndRoundTrip<NoticeMessage>(raws[6]);
      MessageAssert.TypeAndRoundTrip<ActionRequestMessage>(raws[7]);
      MessageAssert.TypeAndRoundTrip<NumericMessage>(raws[8]);
      MessageAssert.TypeAndRoundTrip<GenericMessage>(raws[9]);
      MessageAssert.TypeAndRoundTrip<WelcomeMessage>(raws[10]);
      MessageAssert.TypeAndRoundTrip<MotdStartReplyMessage>(raws[11]);
      MessageAssert.TypeAndRoundTrip<NamesReplyMessage>(raws[12]);
      MessageAssert.TypeAndRoundTrip<NamesEndReplyMessage>(raws[13]);
    }

    [TestMethod]
    public void UniqueReplyNumbers()
    {
      var foo = new NameValueCollection();
      foreach (var type in typeof(NumericMessage).Assembly.GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(NumericMessage))))
      {
        var msg = (NumericMessage) Activator.CreateInstance(type);
        var numeric = msg.InternalNumeric.ToString(CultureInfo.InvariantCulture);

        if (numeric != "-1")
        {
          if (foo[numeric] != null)
          {
            var otherType = foo[numeric];
            var thisType = type.FullName;
            Assert.Fail("Shared Numeric, {0}, for '{1}' and '{2}'.", numeric, thisType, otherType);
          }
          foo[numeric] = type.FullName;
        }
      }
    }

    [TestMethod]
    public void MessageSerialization()
    {
      if (typeof(IrcMessage).Assembly.GetTypes().Any(type => (type.IsAssignableFrom(typeof(IrcMessage)) || type.IsAssignableFrom(typeof(EventArgs))) && !type.IsSerializable))
      {
        Assert.Fail("All Messages and Message EventArgs need to be serializable.");
      }
    }

    [TestMethod]
    public void CtcpUtilParsing()
    {
      var raws = new[] {
        ":foo!bar@zap.com PRIVMSG _aLfa_ :\x0001ACTION does some tests\x0001",
        ":foo!bar@zap.com NOTICE _aLfa_ :\x0001VERSION SupayBot v0.2\x0001"
      };

      var extendedData = CtcpUtil.GetExtendedData(raws[0]);
      var internalCommand = CtcpUtil.GetInternalCommand(raws[0]);
      var transportCommand = CtcpUtil.GetTransportCommand(raws[0]);

      Assert.IsTrue(CtcpUtil.IsCtcpMessage(raws[0]), "Is Ctcp Message");
      Assert.IsTrue(CtcpUtil.IsRequestMessage(raws[0]), "Is Ctcp Request");
      Assert.AreEqual("PRIVMSG", transportCommand, "Transport Command");
      Assert.AreEqual("ACTION", internalCommand, "Internal Command");
      Assert.AreEqual("does some tests", extendedData, "Extended Data");

      extendedData = CtcpUtil.GetExtendedData(raws[1]);
      internalCommand = CtcpUtil.GetInternalCommand(raws[1]);
      transportCommand = CtcpUtil.GetTransportCommand(raws[1]);

      Assert.IsTrue(CtcpUtil.IsCtcpMessage(raws[1]), "Is Ctcp Message");
      Assert.IsTrue(CtcpUtil.IsReplyMessage(raws[1]), "Is Ctcp Reply");
      Assert.AreEqual("NOTICE", transportCommand, "Transport Command");
      Assert.AreEqual("VERSION", internalCommand, "Internal Command");
      Assert.AreEqual("SupayBot v0.2", extendedData, "Extended Data");
    }

    [TestMethod]
    public void MessageUtilParsing()
    {
      var raws = new[] {
        ":foo!bar@zap.com PRIVMSG _aLfa_ :hello, how are you today?",
        "NOTICE _aLfa_,#smack :hello! : !assdlkjdf",
        ":foo!bar@zap.com SILENCE"
      };

      for (var i = 0; i < raws.Length; i++)
      {
        var raw = raws[i];

        var prefix = MessageUtil.GetPrefix(raw);
        var command = MessageUtil.GetCommand(raw);
        var p = MessageUtil.GetParameters(raw);

        var expectedPrefix = string.Empty;
        var expectedCommand = string.Empty;
        var expectedPCount = -1;

        switch (i)
        {
          case 0:
            expectedPrefix = "foo!bar@zap.com";
            expectedCommand = "PRIVMSG";
            expectedPCount = 2;
            break;
          case 1:
            expectedPrefix = "";
            expectedCommand = "NOTICE";
            expectedPCount = 2;
            break;
          case 2:
            expectedPrefix = "foo!bar@zap.com";
            expectedCommand = "SILENCE";
            expectedPCount = 0;
            break;
        }

        Assert.AreEqual(expectedPrefix, prefix, "Prefix Test");
        Assert.AreEqual(expectedCommand, command, "Command Test");
        Assert.AreEqual(expectedPCount, p.Count, "Param Count Test");

        switch (i)
        {
          case 0:
            Assert.AreEqual("_aLfa_", p[0], "p target test");
            Assert.AreEqual("hello, how are you today?", p[1], "p text test");
            break;
          case 1:
            Assert.AreEqual("_aLfa_,#smack", p[0], "p target test");
            Assert.AreEqual("hello! : !assdlkjdf", p[1], "p text test");
            break;
        }
      }
    }

    #region Specific Messages

    [TestMethod]
    public void List()
    {
      var raws = new[] {
        ":irc.dkom.at LIST <10000,>499",
        ":irc.dkom.at LIST #foo"
      };

      var msg = MessageAssert.TypeAndRoundTrip<ListMessage>(raws[0]);
      Assert.AreEqual(499, msg.MinUsers);
      Assert.AreEqual(10000, msg.MaxUsers);

      msg = MessageAssert.TypeAndRoundTrip<ListMessage>(raws[1]);
      Assert.AreEqual("#foo", msg.Channels[0]);
    }

    [TestMethod]
    public void Wallchops()
    {
      var raws = new[] {
        "WALLCHOPS #foo :Test Message"
      };

      var msg = MessageAssert.TypeAndRoundTrip<WallchopsMessage>(raws[0]);
      Assert.AreEqual("#foo", msg.Channel);
      Assert.AreEqual("Test Message", msg.Text);
    }

    [TestMethod]
    public void Silence()
    {
      var raws = new[] {
        "SILENCE +*!*@*.aol.com",
        "SILENCE -foo",
        "SILENCE"
      };

      var users = new[] {
        "*!*@*.aol.com",
        "foo",
        string.Empty
      };
      var actions = new[] {
        ModeAction.Add,
        ModeAction.Remove,
        ModeAction.Add
      };

      for (var i = 0; i < raws.Length; i++)
      {
        var msg = MessageAssert.TypeAndRoundTrip<SilenceMessage>(raws[i]);
        Assert.AreEqual(users[i], msg.SilencedUser.ToString(), "SilencedUser");
        Assert.AreEqual(actions[i], msg.Action, "Silence Action");
      }
    }

    [TestMethod]
    public void SilenceReply()
    {
      var raws = new[] {
        ":broadway.ny.us.dal.net 271 alfaproject alfaproject *!*@*.aol.com"
      };

      var users = new[] {
        "*!*@*.aol.com"
      };
      var owners = new[] {
        "alfaproject"
      };

      for (var i = 0; i < raws.Length; i++)
      {
        var msg = MessageAssert.TypeAndRoundTrip<SilenceReplyMessage>(raws[i]);
        Assert.AreEqual(users[i], msg.SilencedUser.ToString(), "SilencedUser");
        Assert.AreEqual(owners[i], msg.SilenceListOwner, "Silence List Owner");
      }
    }

    [TestMethod]
    public void SilenceEndReply()
    {
      var raws = new[] {
        ":broadway.ny.us.dal.net 272 alfaproject :End of /SILENCE list."
      };

      MessageAssert.TypeAndRoundTrip<SilenceEndReplyMessage>(raws[0]);
    }

    [TestMethod]
    public void DccSend()
    {
      var raws = new[] {
        ":^Care|wrk!~carebear@212.4.54.137 PRIVMSG _aLfa_ :\x0001DCC SEND versions.txt 3557045897 1111 42740\x0001"
      };

      var msg = MessageAssert.TypeAndRoundTrip<DccSendRequestMessage>(raws[0]);
      Assert.AreEqual("212.4.54.137", msg.Address.ToString(), "Address");
      Assert.AreEqual("versions.txt", msg.FileName, "FileName");
      Assert.AreEqual("1111", msg.Port.ToString(CultureInfo.InvariantCulture), "Port");
      Assert.AreEqual("42740", msg.Size.ToString(CultureInfo.InvariantCulture), "Size");
      Assert.AreEqual("^Care|wrk!~carebear@212.4.54.137", msg.Sender.ToString(), "Sender");
      Assert.AreEqual("_aLfa_", msg.Target, "Target");
    }

    [TestMethod]
    public void DccChat()
    {
      var raws = new[] {
        ":JoaoDias!_aLfa_@12-255-177-172.client.attbi.com PRIVMSG _aLfa_ :\x0001DCC CHAT chat 218083756 2777\x0001"
      };

      var msg = MessageAssert.TypeAndRoundTrip<DccChatRequestMessage>(raws[0]);
      Assert.AreEqual("12.255.177.172", msg.Address.ToString(), "Address");
      Assert.AreEqual("2777", msg.Port.ToString(CultureInfo.InvariantCulture), "Port");
      Assert.AreEqual("JoaoDias!_aLfa_@12-255-177-172.client.attbi.com", msg.Sender.ToString(), "Sender");
      Assert.AreEqual("_aLfa_", msg.Target, "Target");
    }

    [TestMethod]
    public void NickInUse()
    {
      var raws = new[] {
        ":irc2.secsup.org 433 _aLfa_ COBOL :Nickname is already in use."
      };

      var msg = MessageAssert.TypeAndRoundTrip<NickInUseMessage>(raws[0]);
      Assert.AreEqual("COBOL", msg.Nick, "Nick");
    }

    [TestMethod]
    public void ErroneusNick()
    {
      var raws = new[] {
        ":irc2.secsup.org 432 _aLfa_ #foo :Erroneus Nickname"
      };

      var msg = MessageAssert.TypeAndRoundTrip<ErroneusNickMessage>(raws[0]);
      Assert.AreEqual("#foo", msg.Nick, "Nick");
    }

    [TestMethod]
    public void PingPong()
    {
      var raws = new[] {
        "PING GDN-7T4JZ11"
      };

      var msg = MessageAssert.TypeAndRoundTrip<PingMessage>(raws[0]);
      Assert.AreEqual("GDN-7T4JZ11", msg.Target, "Target");

      var pong = new PongMessage { Target = msg.Target };
      Assert.AreEqual("PONG GDN-7T4JZ11", pong.ToString(), "Pong Reply");
    }

    [TestMethod]
    public void ChannelModeMessage()
    {
      var raws = new[] {
        ":COBOL!~COBOL@ool-435015b7.dyn.optonline.net MODE #ms.net +o _aLfa_"
      };

      var msg = MessageAssert.TypeAndRoundTrip<ChannelModeMessage>(raws[0]);
      Assert.AreEqual("#ms.net", msg.Channel, "Channel");
      Assert.AreEqual("+o", msg.ModeChanges, "Mode Changes");
      Assert.AreEqual("_aLfa_", msg.ModeArguments[0], "Mode Changes");
    }

    [TestMethod]
    public void ServerInfoMessage()
    {
      var raws = new[] {
        ":irc2.secsup.org 004 _aLfa_ irc2.secsup.org 2.8/hybrid-6.3.1 oOiwszcrkfydnxb biklmnopstve"
      };

      var msg = MessageAssert.TypeAndRoundTrip<ServerInfoMessage>(raws[0]);
      Assert.AreEqual("irc2.secsup.org", msg.ServerName);
      Assert.AreEqual("2.8/hybrid-6.3.1", msg.Version);
      Assert.AreEqual("oOiwszcrkfydnxb", msg.UserModes);
      Assert.AreEqual("biklmnopstve", msg.ChannelModes);
    }

    [TestMethod]
    public void WelcomeMessage()
    {
      var raws = new[] {
        ":irc2.secsup.org 001 _aLfa_ :Welcome to the Internet Relay Network _aLfa_"
      };

      var msg = MessageAssert.TypeAndRoundTrip<WelcomeMessage>(raws[0]);
      Assert.AreEqual("Welcome to the Internet Relay Network _aLfa_", msg.Text, "Welcome Text");
    }

    [TestMethod]
    public void SupportMessage()
    {
      var raws = new[] {
        ":irc.easynews.com 005 _aLfa_ CHANLIMIT=#&:25 WALLCHOPS KNOCK EXCEPTS INVEX MODES=4 MAXCHANNELS=35 MAXBANS=25 MAXTARGETS=4 NICKLEN=9 TOPICLEN=120 KICKLEN=120 :are supported by this server",
        ":irc.foxlink.net 005 SupayBot STD=i-d STATUSMSG=@+ KNOCK EXCEPTS=e INVEX=I MODES=4 MAXCHANNELS=50 MAXLIST=beI:100 MAXTARGETS=7 NICKLEN=9 TOPICLEN=120 KICKLEN=120 :are supported by this server",
        ":irc.foxlink.net 005 SupayBot CHANTYPES=#& PREFIX=(ov)@+ CHANMODES=eIb,k,l,imnpst NETWORK=EFNet CASEMAPPING=rfc1459 CHARSET=ascii CALLERID ETRACE WALLCHOPS :are supported by this server"
      };

      for (var i = 0; i < raws.Length; i++)
      {
        var msg = MessageAssert.TypeAndRoundTrip<SupportMessage>(raws[i]);
        var support = new ServerSupport();
        support.LoadInfo(msg);

        switch (i)
        {
          case 0:
            Assert.AreEqual(12, msg.SupportedItems.Count, "SupportItems Count");
            Assert.IsNotNull(msg.SupportedItems["CHANLIMIT"], "CHANLIMIT");
            Assert.IsNotNull(msg.SupportedItems["WALLCHOPS"], "WALLCHOPS");
            Assert.IsNotNull(msg.SupportedItems["KNOCK"], "KNOCK");
            Assert.IsNotNull(msg.SupportedItems["EXCEPTS"], "EXCEPTS");
            Assert.IsNotNull(msg.SupportedItems["INVEX"], "INVEX");
            Assert.IsNotNull(msg.SupportedItems["MODES"], "MODES");
            Assert.IsNotNull(msg.SupportedItems["MAXCHANNELS"], "MAXCHANNELS");
            Assert.IsNotNull(msg.SupportedItems["MAXBANS"], "MAXBANS");
            Assert.IsNotNull(msg.SupportedItems["MAXTARGETS"], "MAXTARGETS");
            Assert.IsNotNull(msg.SupportedItems["NICKLEN"], "NICKLEN");
            Assert.IsNotNull(msg.SupportedItems["TOPICLEN"], "TOPICLEN");
            Assert.IsNotNull(msg.SupportedItems["KICKLEN"], "KICKLEN");
            Assert.AreEqual("#&:25", msg.SupportedItems["CHANLIMIT"], "CHANLIMIT");
            Assert.AreEqual("4", msg.SupportedItems["MODES"], "MODES");
            Assert.AreEqual("35", msg.SupportedItems["MAXCHANNELS"], "MAXCHANNELS");
            Assert.AreEqual("25", msg.SupportedItems["MAXBANS"], "MAXBANS");
            Assert.AreEqual("4", msg.SupportedItems["MAXTARGETS"], "MAXTARGETS");
            Assert.AreEqual("9", msg.SupportedItems["NICKLEN"], "NICKLEN");
            Assert.AreEqual("120", msg.SupportedItems["TOPICLEN"], "TOPICLEN");
            Assert.AreEqual("120", msg.SupportedItems["KICKLEN"], "KICKLEN");

            Assert.AreEqual(2, support.ChannelLimits.Count, "ChannelLimits");
            Assert.AreEqual(25, support.ChannelLimits["#"], "ChannelLimits");
            Assert.AreEqual(25, support.ChannelLimits["&"], "ChannelLimits");
            Assert.IsTrue(support.MessagesToOperators, "MessagesToOperators");
            Assert.IsTrue(support.Knock, "Knock");
            Assert.IsTrue(support.BanExceptions, "BanExceptions");
            Assert.IsTrue(support.InvitationExceptions, "InvitationExceptions");
            Assert.AreEqual(4, support.MaxModes, "MaxModes");
            Assert.AreEqual(35, support.MaxChannels, "MaxChannels");
            Assert.AreEqual(25, support.MaxBans, "MaxBans");
            Assert.AreEqual(4, support.MaxMessageTargets[""], "MaxMessageTargets");
            Assert.AreEqual(9, support.MaxNickLength, "MaxNickLength");
            Assert.AreEqual(120, support.MaxTopicLength, "MaxTopicLength");
            Assert.AreEqual(120, support.MaxKickCommentLength, "MaxKickCommentLength");

            break;

          case 1:
            Assert.AreEqual(12, msg.SupportedItems.Count, "SupportedItems Count");
            Assert.IsNotNull(msg.SupportedItems["STD"], "STD");
            Assert.IsNotNull(msg.SupportedItems["STATUSMSG"], "STATUSMSG");
            Assert.IsNotNull(msg.SupportedItems["KNOCK"], "KNOCK");
            Assert.IsNotNull(msg.SupportedItems["EXCEPTS"], "EXCEPTS");
            Assert.IsNotNull(msg.SupportedItems["INVEX"], "INVEX");
            Assert.IsNotNull(msg.SupportedItems["MODES"], "MODES");
            Assert.IsNotNull(msg.SupportedItems["MAXCHANNELS"], "MAXCHANNELS");
            Assert.IsNotNull(msg.SupportedItems["MAXLIST"], "MAXLIST");
            Assert.IsNotNull(msg.SupportedItems["MAXTARGETS"], "MAXTARGETS");
            Assert.IsNotNull(msg.SupportedItems["NICKLEN"], "NICKLEN");
            Assert.IsNotNull(msg.SupportedItems["TOPICLEN"], "TOPICLEN");
            Assert.IsNotNull(msg.SupportedItems["KICKLEN"], "KICKLEN");

            Assert.AreEqual("i-d", msg.SupportedItems["STD"], "STD");
            Assert.AreEqual("@+", msg.SupportedItems["STATUSMSG"], "STATUSMSG");
            Assert.AreEqual("e", msg.SupportedItems["EXCEPTS"], "EXCEPTS");
            Assert.AreEqual("I", msg.SupportedItems["INVEX"], "INVEX");
            Assert.AreEqual("4", msg.SupportedItems["MODES"], "MODES");
            Assert.AreEqual("50", msg.SupportedItems["MAXCHANNELS"], "MAXCHANNELS");
            Assert.AreEqual("beI:100", msg.SupportedItems["MAXLIST"], "MAXLIST");
            Assert.AreEqual("7", msg.SupportedItems["MAXTARGETS"], "MAXTARGETS");
            Assert.AreEqual("9", msg.SupportedItems["NICKLEN"], "NICKLEN");
            Assert.AreEqual("120", msg.SupportedItems["TOPICLEN"], "TOPICLEN");
            Assert.AreEqual("120", msg.SupportedItems["KICKLEN"], "KICKLEN");

            Assert.AreEqual("i-d", support.Standard, "Standard");
            Assert.AreEqual("@+", support.StatusMessages, "StatusMessages");
            Assert.IsTrue(support.Knock, "Knock");
            Assert.IsTrue(support.BanExceptions, "BanExceptions");
            Assert.IsTrue(support.InvitationExceptions, "InvitationExceptions");
            Assert.AreEqual(4, support.MaxModes, "MaxModes");
            Assert.AreEqual(50, support.MaxChannels, "MaxChannels");
            Assert.AreEqual(100, support.MaxBans, "MaxBans");
            Assert.AreEqual(100, support.MaxBanExceptions, "MaxBanExceptions");
            Assert.AreEqual(100, support.MaxInvitationExceptions, "MaxInvitationExceptions");
            Assert.AreEqual(7, support.MaxMessageTargets[""], "MaxMessageTargets");
            Assert.AreEqual(9, support.MaxNickLength, "MaxNickLength");
            Assert.AreEqual(120, support.MaxTopicLength, "MaxTopicLength");
            Assert.AreEqual(120, support.MaxKickCommentLength, "MaxKickCommentLength");

            break;

          case 2:
            Assert.AreEqual(9, msg.SupportedItems.Count, "SupportedItems Count");
            Assert.IsNotNull(msg.SupportedItems["CHANTYPES"], "CHANTYPES");
            Assert.IsNotNull(msg.SupportedItems["PREFIX"], "PREFIX");
            Assert.IsNotNull(msg.SupportedItems["CHANMODES"], "CHANMODES");
            Assert.IsNotNull(msg.SupportedItems["NETWORK"], "NETWORK");
            Assert.IsNotNull(msg.SupportedItems["CASEMAPPING"], "CASEMAPPING");
            Assert.IsNotNull(msg.SupportedItems["CHARSET"], "CHARSET");
            Assert.IsNotNull(msg.SupportedItems["CALLERID"], "CALLERID");
            Assert.IsNotNull(msg.SupportedItems["ETRACE"], "ETRACE");
            Assert.IsNotNull(msg.SupportedItems["WALLCHOPS"], "WALLCHOPS");

            Assert.AreEqual("#&", msg.SupportedItems["CHANTYPES"], "CHANTYPES");
            Assert.AreEqual("(ov)@+", msg.SupportedItems["PREFIX"], "PREFIX");
            Assert.AreEqual("eIb,k,l,imnpst", msg.SupportedItems["CHANMODES"], "CHANMODES");
            Assert.AreEqual("EFNet", msg.SupportedItems["NETWORK"], "NETWORK");
            Assert.AreEqual("rfc1459", msg.SupportedItems["CASEMAPPING"], "CASEMAPPING");
            Assert.AreEqual("ascii", msg.SupportedItems["CHARSET"], "CHARSET");

            Assert.IsTrue(support.CallerId, "CallerId");
            Assert.IsTrue(support.ETrace, "ETrace");
            Assert.IsTrue(support.MessagesToOperators, "MessagesToOperators");
            Assert.AreEqual(2, support.ChannelTypes.Count, "ChannelTypes");
            Assert.AreEqual("(ov)@+", support.ChannelStatuses, "ChannelStatuses");
            Assert.AreEqual(4, support.ModesWithParameters.Count, "ModesWithParameters");
            Assert.AreEqual(6, support.ModesWithoutParameters.Count, "ModesWithoutParameters");
            Assert.AreEqual(1, support.ModesWithParametersWhenSet.Count, "ModesWithParametersWhenSet");
            Assert.AreEqual("EFNet", support.NetworkName, "NetworkName");
            Assert.AreEqual("rfc1459", support.CaseMapping, "CaseMapping");
            Assert.AreEqual("ascii", support.CharacterSet, "CharacterSet");

            break;
        }
      }
    }

    [TestMethod]
    public void PartMessage()
    {
      var raws = new[] {
        ":atchr!~antichron@pcp407739pcs.waldrf01.md.comcast.net PART #ms.net",
        ":atchr!~antichron@pcp407739pcs.waldrf01.md.comcast.net PART #ms.net,#mscorlib :You solved all my problems."
      };

      var channels = new[] {
        new[] { "#ms.net" },
        new[] { "#ms.net", "#mscorlib" }
      };
      var reasons = new[] {
        "",
        "You solved all my problems."
      };

      for (var i = 0; i < raws.Length; i++)
      {
        var msg = MessageAssert.TypeAndRoundTrip<PartMessage>(raws[i]);
        Assert.AreEqual(reasons[i], msg.Reason, "Reason");
        for (var j = 0; j < msg.Channels.Count; j++)
        {
          Assert.AreEqual(channels[i][j], msg.Channels[j], "Channel");
        }
      }
    }

    [TestMethod]
    public void TimeRequestMessage()
    {
      var raws = new[] {
        ":liKwid|ii!iridium@kernel.hacking.no PRIVMSG _aLfa_ \x0001TIME\x0001"
      };

      var msg = MessageAssert.TypeAndRoundTrip<TimeRequestMessage>(raws[0]);
      Assert.AreEqual("_aLfa_", msg.Target);
    }

    [TestMethod]
    public void JoinMessage()
    {
      var raws = new[] {
        ":_aLfa_!_aLfa_@12-255-177-172.client.attbi.com JOIN #ms.net",
        ":_aLfa_!_aLfa_@12-255-177-172.client.attbi.com JOIN #ms.net,#mscorlib",
        ":_aLfa_!_aLfa_@12-255-177-172.client.attbi.com JOIN #ms.net myKey",
        ":_aLfa_!_aLfa_@12-255-177-172.client.attbi.com JOIN #ms.net,#mscorlib myKey,myOtherKey",
        ":_aLfa_ JOIN #foo"
      };

      var channels = new[] {
        new[] { "#ms.net" },
        new[] { "#ms.net", "#mscorlib" },
        new[] { "#ms.net" },
        new[] { "#ms.net", "#mscorlib" },
        new[] { "#foo" }
      };
      var keys = new[] {
        new string[] { },
        new string[] { },
        new[] { "myKey" },
        new[] { "myKey", "myOtherKey" },
        new string[] { }
      };

      for (var i = 0; i < raws.Length; i++)
      {
        var msg = MessageAssert.TypeAndRoundTrip<JoinMessage>(raws[i]);
        for (var j = 0; j < msg.Channels.Count; j++)
        {
          Assert.AreEqual(channels[i][j], msg.Channels[j], "Channels");
        }
        for (var k = 0; k < msg.Keys.Count; k++)
        {
          Assert.AreEqual(keys[i][k], msg.Keys[k], "Keys");
        }
      }
    }

    [TestMethod]
    public void YourHostIsMessageTest()
    {
      var raws = new[] {
        ":irc.easynews.com 002 SupayBot :Your host is irc.easynews.com[irc.easynews.com/6667], running version ircd-ratbox-1.1rc1"
      };

      var msg = MessageAssert.TypeAndRoundTrip<YourHostMessage>(raws[0]);
      Assert.AreEqual("irc.easynews.com[irc.easynews.com/6667]", msg.ServerName, "ServerName");
      Assert.AreEqual("ircd-ratbox-1.1rc1", msg.Version, "Version");
    }

    [TestMethod]
    public void ServerCreatedMessageTest()
    {
      var raws = new[] {
        ":irc.easynews.com 003 SupayBot :This server was created Thu Dec 26 2002 at 15:52:56 MST"
      };

      var msg = MessageAssert.TypeAndRoundTrip<ServerCreatedMessage>(raws[0]);
      Assert.AreEqual("Thu Dec 26 2002 at 15:52:56 MST", msg.CreatedDate, "CreatedDate");
    }

    [TestMethod]
    public void LusersReplyMessageTest()
    {
      var raws = new[] {
        ":irc.easynews.com 251 SupayBot :There are 7012 users and 112533 invisible on 50 servers"
      };

      var msg = MessageAssert.TypeAndRoundTrip<LusersReplyMessage>(raws[0]);
      Assert.AreEqual(7012, msg.UserCount, "UserCount");
      Assert.AreEqual(112533, msg.InvisibleCount, "InvisibleCount");
      Assert.AreEqual(50, msg.ServerCount, "ServerCount");
    }

    [TestMethod]
    public void LusersOpReplyMessageTest()
    {
      var raws = new[] {
        ":irc.easynews.com 252 SupayBot 350 :POWER RANGER IRC Operators online"
      };

      var msg = MessageAssert.TypeAndRoundTrip<LusersOpReplyMessage>(raws[0]);
      Assert.AreEqual(350, msg.OpCount, "OpCount");
      Assert.AreEqual("POWER RANGER IRC Operators online", msg.Info, "Info");
    }

    [TestMethod]
    public void LusersChannelsReplyMessageTest()
    {
      var raws = new[] {
        ":irc.easynews.com 254 SupayBot 42533 :channels formed"
      };

      var msg = MessageAssert.TypeAndRoundTrip<LusersChannelsReplyMessage>(raws[0]);
      Assert.AreEqual(42533, msg.ChannelCount, "ChannelCount");
    }

    [TestMethod]
    public void LusersMeReplyMessageTest()
    {
      var raws = new[] {
        ":irc.easynews.com 255 SupayBot :I have 9638 clients and 1 servers"
      };

      var msg = MessageAssert.TypeAndRoundTrip<LusersMeReplyMessage>(raws[0]);
      Assert.AreEqual(9638, msg.ClientCount, "ClientCount");
      Assert.AreEqual(1, msg.ServerCount, "ServerCount");
    }

    [TestMethod]
    public void LocalUsersReplyMessageTest()
    {
      var raws = new[] {
        ":irc.easynews.com 265 SupayBot :Current local users: 9638 Max: 13035",
        ":irc.ptptech.com 265 _aLfa_ 606 610 :Current local users 606, max 610",
        ":helix.fl.us.SwiftIRC.net 265 SupayBot :Current Local Users: 6074  Max: 16258"
      };

      var msg = MessageAssert.TypeAndRoundTrip<LocalUsersReplyMessage>(raws[0]);
      Assert.AreEqual(9638, msg.UserCount, "UserCount");
      Assert.AreEqual(13035, msg.UserLimit, "UserLimit");

      // the second one will not be a valid round trip.
      msg = IrcMessageFactory.Parse(raws[1]) as LocalUsersReplyMessage;
      Assert.IsNotNull(msg);
      Assert.AreEqual(606, msg.UserCount, "UserCount");
      Assert.AreEqual(610, msg.UserLimit, "UserLimit");

      // the third one will also not be a valid round trip.
      msg = IrcMessageFactory.Parse(raws[2]) as LocalUsersReplyMessage;
      Assert.IsNotNull(msg);
      Assert.AreEqual(6074, msg.UserCount, "UserCount");
      Assert.AreEqual(16258, msg.UserLimit, "UserLimit");
    }

    [TestMethod]
    public void GlobalUsersReplyMessageTest()
    {
      var raws = new[] {
        ":irc.easynews.com 266 SupayBot :Current global users: 119545 Max: 131548",
        ":irc.ptptech.com 266 _aLfa_ 84236 87756 :Current global users 84236, max 87756",
        ":helix.fl.us.SwiftIRC.net 266 SupayBot :Current Global Users: 6074  Max: 16258"
      };

      var msg = MessageAssert.TypeAndRoundTrip<GlobalUsersReplyMessage>(raws[0]);
      Assert.AreEqual(119545, msg.UserCount, "UserCount");
      Assert.AreEqual(131548, msg.UserLimit, "UserLimit");

      // the second one will not be a valid round trip.
      msg = IrcMessageFactory.Parse(raws[1]) as GlobalUsersReplyMessage;
      Assert.IsNotNull(msg);
      Assert.AreEqual(84236, msg.UserCount, "UserCount");
      Assert.AreEqual(87756, msg.UserLimit, "UserLimit");

      // the third one will not be a valid round trip.
      msg = IrcMessageFactory.Parse(raws[2]) as GlobalUsersReplyMessage;
      Assert.IsNotNull(msg);
      Assert.AreEqual(6074, msg.UserCount, "UserCount");
      Assert.AreEqual(16258, msg.UserLimit, "UserLimit");
    }

    [TestMethod]
    public void TopicSetReplyMessageTest()
    {
      var raws = new[] {
        ":irc.easynews.com 333 SupayBot #ms.net ^Care|wrk!~carebear@212.4.54.137 104403576"
      };

      var msg = MessageAssert.TypeAndRoundTrip<TopicSetReplyMessage>(raws[0]);
      Assert.AreEqual("#ms.net", msg.Channel, "Channel");
      Assert.AreEqual(MessageUtil.ConvertFromUnixTime(104403576), msg.TimeSet, "TimeSet");
      Assert.AreEqual("^Care|wrk!~carebear@212.4.54.137", msg.User.ToString(), "User");
    }

    [TestMethod]
    public void StatsReplyMessageTest()
    {
      var raws = new[] {
        ":irc.easynews.com 250 SupayBot :Highest connection count: 13036 (13035 clients) (1042354 connections received)"
      };

      var msg = MessageAssert.TypeAndRoundTrip<StatsReplyMessage>(raws[0]);
      Assert.AreEqual("Highest connection count: 13036 (13035 clients) (1042354 connections received)", msg.Stats, "Stats");
    }

    [TestMethod]
    public void ChatMessageRoundTrip()
    {
      var raws = new[] {
        ":foo!bar@zap.com PRIVMSG _aLfa_ :hello, how are you today?"
      };

      var msg = MessageAssert.TypeAndRoundTrip<ChatMessage>(raws[0]);
      Assert.AreEqual("foo!bar@zap.com", msg.Sender.ToString(), "Sender");
      Assert.AreEqual("_aLfa_", msg.Targets[0], "Target");
      Assert.AreEqual("hello, how are you today?", msg.Text, "Text");
    }

    [TestMethod]
    public void NoticeMessageRoundTrip()
    {
      var raws = new[] {
        ":foo!bar@zap.com NOTICE _aLfa_,#smack :hello, how are you today?"
      };

      var msg = MessageAssert.TypeAndRoundTrip<NoticeMessage>(raws[0]);
      Assert.AreEqual("foo!bar@zap.com", msg.Sender.ToString(), "Sender");
      Assert.AreEqual("_aLfa_", msg.Targets[0], "Target1");
      Assert.AreEqual("#smack", msg.Targets[1], "Target2");
      Assert.AreEqual("hello, how are you today?", msg.Text, "Text");
    }

    [TestMethod]
    public void UniqueIdMessageTest()
    {
      var raws = new[] {
        ":irc.inter.net.il 042 SupayBot SupayBot 5ILABFZUY :your unique ID",
        ":irc.inter.net.il 042 SupayBot SupayBot 5ILABFZW7 :your unique ID",
        ":us.ircnet.org 042 SupayBot SupayBot 0PNUACUZI :your unique ID"
      };

      for (var i = 0; i < raws.Length; i++)
      {
        var msg = MessageAssert.TypeAndRoundTrip<UniqueIdMessage>(raws[i]);
        switch (i)
        {
          case 0:
            Assert.AreEqual("5ILABFZUY", msg.UniqueId, false, CultureInfo.InvariantCulture, "UniqueId");
            break;
          case 1:
            Assert.AreEqual("5ILABFZW7", msg.UniqueId, false, CultureInfo.InvariantCulture, "UniqueId");
            break;
          case 2:
            Assert.AreEqual("0PNUACUZI", msg.UniqueId, false, CultureInfo.InvariantCulture, "UniqueId");
            break;
        }
      }
    }

    #endregion
  }
}
