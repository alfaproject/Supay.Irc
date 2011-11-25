using Microsoft.VisualStudio.TestTools.UnitTesting;
using Supay.Irc.Messages;

namespace Supay.Irc.Tests
{
  [TestClass]
  public class ChannelScopedTests
  {
    [TestMethod]
    public void ChannelScopedChatTest()
    {
      var raws = new[] {
        "CPRIVMSG _aLfa_ #foo :Fake Message"
      };

      var msg = MessageAssert.TypeAndRoundTrip<ChannelScopedChatMessage>(raws[0]);
      Assert.AreEqual("_aLfa_", msg.Target);
      Assert.AreEqual("#foo", msg.Channel);
      Assert.AreEqual("Fake Message", msg.Text);
    }

    [TestMethod]
    public void ChannelScopedNoticeTest()
    {
      var raws = new[] {
        "CNOTICE _aLfa_ #foo :Fake Message"
      };

      var msg = MessageAssert.TypeAndRoundTrip<ChannelScopedNoticeMessage>(raws[0]);
      Assert.AreEqual("_aLfa_", msg.Target);
      Assert.AreEqual("#foo", msg.Channel);
      Assert.AreEqual("Fake Message", msg.Text);
    }
  }
}
