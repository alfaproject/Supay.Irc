using Microsoft.VisualStudio.TestTools.UnitTesting;
using Supay.Irc.Messages;

namespace Supay.Irc.Tests
{
  [TestClass]
  public class IgnoreTests
  {
    [TestMethod]
    public void SilenceTest()
    {
      var raws = new[] {
        ":mesra.kl.my.dal.net SILENCE +acidjnk!*@*",
        ":_aLfa_!_aLfa_@c-24-9-165-248.hsd1.co.comcast.net SILENCE +aMike!*@*",
        ":mesra.kl.my.dal.net SILENCE"
      };

      var msg = MessageAssert.TypeAndRoundTrip<SilenceMessage>(raws[0]);
      Assert.AreEqual(ModeAction.Add, msg.Action);
      Assert.AreEqual("acidjnk", msg.SilencedUser.Nickname);

      msg = MessageAssert.TypeAndRoundTrip<SilenceMessage>(raws[1]);
      Assert.AreEqual(ModeAction.Add, msg.Action);
      Assert.AreEqual("aMike", msg.SilencedUser.Nickname);

      msg = MessageAssert.TypeAndRoundTrip<SilenceMessage>(raws[2]);
      Assert.AreEqual("", msg.SilencedUser.Nickname);
    }

    [TestMethod]
    public void Silence271Test()
    {
      var raws = new[] {
        ":mesra.kl.my.dal.net 271 _aLfa_ _aLfa_ acidjnk!*@*"
      };

      var msg = MessageAssert.TypeAndRoundTrip<SilenceReplyMessage>(raws[0]);
      Assert.AreEqual("_aLfa_", msg.SilenceListOwner);
      Assert.AreEqual("acidjnk", msg.SilencedUser.Nickname);
    }

    [TestMethod]
    public void Silence272Test()
    {
      var raws = new[] {
        ":mesra.kl.my.dal.net 272 _aLfa_ :End of /SILENCE list."
      };

      MessageAssert.TypeAndRoundTrip<SilenceEndReplyMessage>(raws[0]);
    }

    [TestMethod]
    public void Silence511Test()
    {
      var raws = new[] {
        ":mesra.kl.my.dal.net 511 _aLfa_ acidjnk!*@* :Your silence list is full"
      };

      var msg = MessageAssert.TypeAndRoundTrip<SilenceListFullMessage>(raws[0]);
      Assert.AreEqual("acidjnk", msg.SilenceMask.Nickname);
    }

    [TestMethod]
    public void AcceptListEditorTest()
    {
      var raws = new[] {
        ":irc.pte.hu ACCEPT azure",
        ":irc.pte.hu ACCEPT -BBS",
        ":irc.pte.hu ACCEPT -BBS,azure"
      };

      var msg = MessageAssert.TypeAndRoundTrip<AcceptListEditorMessage>(raws[0]);
      Assert.AreEqual(1, msg.AddedNicks.Count);
      Assert.AreEqual(0, msg.RemovedNicks.Count);
      Assert.AreEqual("azure", msg.AddedNicks[0]);

      msg = MessageAssert.TypeAndRoundTrip<AcceptListEditorMessage>(raws[1]);
      Assert.AreEqual(0, msg.AddedNicks.Count);
      Assert.AreEqual(1, msg.RemovedNicks.Count);
      Assert.AreEqual("BBS", msg.RemovedNicks[0]);

      msg = MessageAssert.TypeAndRoundTrip<AcceptListEditorMessage>(raws[2]);
      Assert.AreEqual(1, msg.AddedNicks.Count);
      Assert.AreEqual(1, msg.RemovedNicks.Count);
      Assert.AreEqual("azure", msg.AddedNicks[0]);
      Assert.AreEqual("BBS", msg.RemovedNicks[0]);
    }

    [TestMethod]
    public void AcceptListRequestTest()
    {
      var raws = new[] {
        ":irc.pte.hu ACCEPT *"
      };

      MessageAssert.TypeAndRoundTrip<AcceptListRequestMessage>(raws[0]);
    }

    [TestMethod]
    public void AcceptListReplyTest()
    {
      var raws = new[] {
        ":irc.pte.hu 281 _aLfa_ azure bbs"
      };

      var msg = MessageAssert.TypeAndRoundTrip<AcceptListReplyMessage>(raws[0]);
      Assert.AreEqual(2, msg.Nicks.Count);
      Assert.AreEqual("azure", msg.Nicks[0]);
      Assert.AreEqual("bbs", msg.Nicks[1]);
    }

    [TestMethod]
    public void AcceptListEndReplyTest()
    {
      var raws = new[] {
        ":irc.pte.hu 282 _aLfa_ :End of /ACCEPT list."
      };

      MessageAssert.TypeAndRoundTrip<AcceptListEndReplyMessage>(raws[0]);
    }

    [TestMethod]
    public void Accept458Test()
    {
      var raws = new[] {
        ":irc.pte.hu 458 _aLfa_ BBS :is not on your accept list"
      };

      var msg = MessageAssert.TypeAndRoundTrip<AcceptDoesNotExistMessage>(raws[0]);
      Assert.AreEqual("BBS", msg.Nick);
    }

    [TestMethod]
    public void Accept457Test()
    {
      var raws = new[] {
        ":irc.pte.hu 457 _aLfa_ azure :is already on your accept list"
      };

      var msg = MessageAssert.TypeAndRoundTrip<AcceptAlreadyExistsMessage>(raws[0]);
      Assert.AreEqual("azure", msg.Nick);
    }

    [TestMethod]
    public void Accept456Test()
    {
      var raws = new[] {
        ":irc.server 456 client :Accept list is full"
      };

      MessageAssert.TypeAndRoundTrip<AcceptListFullMessage>(raws[0]);
    }
  }
}
