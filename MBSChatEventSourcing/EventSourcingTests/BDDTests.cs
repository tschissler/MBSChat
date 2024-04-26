using EventSourcing;

namespace EventSourcingTests
{
    [TestClass]
    public class BDDTests
    {
        [TestMethod]
        public void NeueNachrichtKannGesendetUndWiederGelesenWerden()
        {
            // Arrange
            var channel = new ChannelAggregateRoot();
            var nachricht = new Nachricht("Hallo Welt");

            // Act
            channel.SendeNachricht(nachricht);

            // Assert
            Assert.AreEqual(1, channel.Nachrichten.Count);
            Assert.AreEqual(nachricht, channel.Nachrichten[0]);
        }
    }
}