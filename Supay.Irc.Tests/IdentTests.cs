using System.IO;
using System.Net.Sockets;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Supay.Irc.Network;

namespace Supay.Irc.Tests
{
    [TestClass]
    public class IdentTests
    {
        [TestMethod]
        public void IdentTest()
        {
            var identService = Ident.Service.Start(true);
            Assert.AreEqual(ConnectionStatus.Connecting, Ident.Service.Status);

            using (var client = new TcpClient("localhost", 113))
            {
                var stream = client.GetStream();

                using (var writer = new StreamWriter(stream))
                {
                    writer.WriteLine("test");
                    writer.Flush();

                    using (var reader = new StreamReader(stream))
                    {
                        var reply = reader.ReadLine();
                        Assert.AreEqual("test : USERID : UNIX : Supay", reply);
                    }
                }
            }

            identService.Wait();
            Assert.AreEqual(ConnectionStatus.Disconnected, Ident.Service.Status);
        }
    }
}
