using Microsoft.VisualStudio.TestTools.UnitTesting;
using Supay.Irc.Messages;

namespace Supay.Irc.Tests
{
  [TestClass]
  public class IrcxTests
  {
    [TestMethod]
    public void Whisper()
    {
      var raws = new[] {
        "WHISPER #example user1,user2 :Hello World"
      };

      var msg = MessageAssert.TypeAndRoundTrip<WhisperMessage>(raws[0]);
      Assert.AreEqual("#example", msg.Channel, "Channel");
      Assert.AreEqual("Hello World", msg.Text, "Text");
      Assert.AreEqual(2, msg.Targets.Count, "Targets Count");
      Assert.AreEqual("user1", msg.Targets[0], "Targets");
      Assert.AreEqual("user2", msg.Targets[1], "Targets");
    }

    [TestMethod]
    public void ChannelProperty()
    {
      var raws = new[] {
        "PROP #testing ONJOIN :Welcome to the stupidest channel ever.",
        "PROP #testing ONJOIN",
        "PROP #testing *"
      };

      var channels = new[] {
        "#testing",
        "#testing",
        "#testing"
      };
      var properties = new[] {
        "ONJOIN",
        "ONJOIN",
        ""
      };
      var values = new[] {
        "Welcome to the stupidest channel ever.",
        "",
        ""
      };

      for (var i = 0; i < raws.Length; i++)
      {
        var msg = MessageAssert.TypeAndRoundTrip<ChannelPropertyMessage>(raws[i]);
        Assert.AreEqual(channels[i], msg.Channel, "Channel");
        Assert.AreEqual(properties[i], msg.Prop, "Property");
        Assert.AreEqual(values[i], msg.NewValue, "New Value");
      }
    }

    [TestMethod]
    public void ChannelPropertyReply()
    {
      var raws = new[] {
        ":chat3 818 alfaproject #testing ONJOIN :Welcome to the stupidest channel ever.",
        ":chat3 818 alfaproject #testing GUID {}",
        ":chat3 818 alfaproject #testing CREATOR alfaproject[oqd@proxy38.safetypin.net]"
      };

      var channels = new[] {
        "#testing",
        "#testing",
        "#testing"
      };
      var properties = new[] {
        "ONJOIN",
        "GUID",
        "CREATOR"
      };
      var values = new[] {
        "Welcome to the stupidest channel ever.",
        "{}",
        "alfaproject[oqd@proxy38.safetypin.net]"
      };

      for (var i = 0; i < raws.Length; i++)
      {
        var msg = MessageAssert.TypeAndRoundTrip<ChannelPropertyReplyMessage>(raws[i]);
        Assert.AreEqual(channels[i], msg.Channel, "Channel");
        Assert.AreEqual(properties[i], msg.Prop, "Property");
        Assert.AreEqual(values[i], msg.Value, "New Value");
      }
    }

    [TestMethod]
    public void ChannelPropertyEndReply()
    {
      var raws = new[] {
        ":chat3 819 alfaproject #testing :End of properties"
      };

      var msg = MessageAssert.TypeAndRoundTrip<ChannelPropertyEndReplyMessage>(raws[0]);
      Assert.AreEqual("#testing", msg.Channel, "Channel");
    }

    [TestMethod]
    public void IsIrcx()
    {
      var raws = new[] {
        "ISIRCX",
        "IRCX"
      };

      MessageAssert.TypeAndRoundTrip<IsIrcxMessage>(raws[0]);

      MessageAssert.TypeAndRoundTrip<IrcxMessage>(raws[1]);
    }

    [TestMethod]
    public void IrcxReply()
    {
      var raws = new[] {
        ":server 800 you 0 1 ANON 512 *",
        ":server 800 you 1 1 ANON 512 *"
      };

      var msg = MessageAssert.TypeAndRoundTrip<IrcxReplyMessage>(raws[0]);
      Assert.IsFalse(msg.IsIrcxClientMode, "IsIrcxClientMode");
      Assert.AreEqual("1", msg.Version, "Version");
      Assert.AreEqual(1, msg.AuthenticationPackages.Count, "Authentication Packages Count");
      Assert.AreEqual("ANON", msg.AuthenticationPackages[0], "Authentication Packages");
      Assert.AreEqual(512, msg.MaximumMessageLength, "Message Length");
      Assert.AreEqual("*", msg.Tokens, "Tokens");

      msg = MessageAssert.TypeAndRoundTrip<IrcxReplyMessage>(raws[1]);
      Assert.IsTrue(msg.IsIrcxClientMode, "IsIrcxClientMode");
      Assert.AreEqual("1", msg.Version, "Version");
      Assert.AreEqual(1, msg.AuthenticationPackages.Count, "Authentication Packages Count");
      Assert.AreEqual("ANON", msg.AuthenticationPackages[0], "Authentication Packages");
      Assert.AreEqual(512, msg.MaximumMessageLength, "Message Length");
      Assert.AreEqual("*", msg.Tokens, "Tokens");
    }

    [TestMethod]
    public void Knock()
    {
      var raws = new[] {
        "KNOCK #testing"
      };

      var msg = MessageAssert.TypeAndRoundTrip<KnockMessage>(raws[0]);
      Assert.AreEqual("#testing", msg.Channel, "Channel");
    }

    [TestMethod]
    public void KnockRequest()
    {
      var raws = new[] {
        ":irc.foxlink.net 710 #_aLfa_2 #_aLfa_2 _aLfa_!_aLfa_@12-255-177-172.client.attbi.com :has asked for an invite."
      };

      var msg = MessageAssert.TypeAndRoundTrip<KnockRequestMessage>(raws[0]);
      Assert.AreEqual("#_aLfa_2", msg.Channel, "Channel");
      Assert.AreEqual("_aLfa_!_aLfa_@12-255-177-172.client.attbi.com", msg.Knocker.ToString(), "Knocker");
    }

    [TestMethod]
    public void KnockReply()
    {
      var raws = new[] {
        ":irc.foxlink.net 711 _aLfa_ #_aLfa_ :Your KNOCK has been delivered."
      };

      var msg = MessageAssert.TypeAndRoundTrip<KnockReplyMessage>(raws[0]);
      Assert.AreEqual("#_aLfa_", msg.Channel, "Channel");
    }
  }
}
