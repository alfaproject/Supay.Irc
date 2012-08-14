using Microsoft.VisualStudio.TestTools.UnitTesting;
using Supay.Irc.Messages;
using Supay.Irc.Messages.Modes;

namespace Supay.Irc.Tests
{
    [TestClass]
    public class ModeTests
    {
        [TestMethod]
        public void ParseOpMode()
        {
            const string raw = ":COBOL!~COBOL@ool-435015b7.dyn.optonline.net MODE #ms.net +o _aLfa_";

            IrcMessage ircMsg = IrcMessageFactory.Parse(raw);
            Assert.IsInstanceOfType(ircMsg, typeof(ChannelModeMessage), "Parses Part");

            var msg = (ChannelModeMessage) ircMsg;
            var creator = new ChannelModesCreator();
            creator.Parse(msg);

            Assert.AreEqual(1, creator.Modes.Count, "Mode Count");
            Assert.IsInstanceOfType(creator.Modes[0], typeof(OperatorMode), "Mode Type");

            var mode = (OperatorMode) creator.Modes[0];
            Assert.AreEqual(ModeAction.Add, mode.Action, "Mode Action");
            Assert.AreEqual("_aLfa_", mode.Nick, "Mode Nick");

            creator.ApplyTo(msg);

            Assert.AreEqual(raw, msg.ToString(), "Round Trip");
        }

        [TestMethod]
        public void ParseMultiOpMode()
        {
            const string raw = ":COBOL!~COBOL@ool-435015b7.dyn.optonline.net MODE #ms.net +oo _aLfa_ bob";

            IrcMessage ircMsg = IrcMessageFactory.Parse(raw);
            Assert.IsInstanceOfType(ircMsg, typeof(ChannelModeMessage), "Parses MODE");

            var msg = (ChannelModeMessage) ircMsg;
            var creator = new ChannelModesCreator();
            creator.Parse(msg);

            Assert.AreEqual(2, creator.Modes.Count, "Mode Count");
            Assert.IsInstanceOfType(creator.Modes[0], typeof(OperatorMode), "Mode Type");
            Assert.IsInstanceOfType(creator.Modes[1], typeof(OperatorMode), "Mode Type");

            var mode = (OperatorMode) creator.Modes[0];
            Assert.AreEqual(ModeAction.Add, mode.Action, "Mode Action");
            Assert.AreEqual("_aLfa_", mode.Nick, "Mode Nick");

            mode = (OperatorMode) creator.Modes[1];
            Assert.AreEqual(ModeAction.Add, mode.Action, "Mode Action");
            Assert.AreEqual("bob", mode.Nick, "Mode Nick");

            creator.ApplyTo(msg);

            Assert.AreEqual(raw, msg.ToString(), "Round Trip");
        }
    }
}
