using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Supay.Irc.Tests
{
  [TestClass]
  public class MaskTests
  {
    [TestMethod]
    public void ParseMask()
    {
      string raw = "foo!bar@zap.com";
      var mask = new User(raw);
      Assert.AreEqual("foo", mask.Nickname, "Nick");
      Assert.AreEqual("bar", mask.Username, "User");
      Assert.AreEqual("zap.com", mask.Host, "Host");

      raw = "foo!*@*.edu";
      mask = new User(raw);
      Assert.AreEqual("foo", mask.Nickname, "Nick");
      Assert.AreEqual("*", mask.Username, "User");
      Assert.AreEqual("*.edu", mask.Host, "Host");

      raw = "foo";
      mask = new User(raw);
      Assert.AreEqual("foo", mask.Nickname);
      Assert.IsNull(mask.Username);
      Assert.IsNull(mask.Host);

      raw = "@foo";
      mask = new User(raw);
      Assert.IsNull(mask.Nickname);
      Assert.IsNull(mask.Username);
      Assert.AreEqual("foo", mask.Host);
    }

    [TestMethod]
    public void MatchMask()
    {
      Assert.IsTrue(new User("foo!bar@zap.com").IsMatch(new User("foo!*@*.com")));
      Assert.IsFalse(new User("foo!bar@zap.com").IsMatch(new User("foo!*@*.edu")));
    }

    [TestMethod]
    public void RoundTrip()
    {
      var raw = "foo!bar@zap.com";
      var m = new User(raw);
      var parsed = m.ToString();
      Assert.AreEqual(raw, parsed);

      raw = "foo!*@*.edu";
      m = new User(raw);
      parsed = m.ToString();
      Assert.AreEqual(raw, parsed);

      raw = "foo@bar.net";
      m = new User(raw);
      parsed = m.ToString();
      Assert.AreEqual(raw, parsed);

      raw = "foo";
      m = new User(raw);
      parsed = m.ToString();
      Assert.AreEqual(raw, parsed);
    }
  }
}
