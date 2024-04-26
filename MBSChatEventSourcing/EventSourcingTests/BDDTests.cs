#nullable enable
using EventSourcing;
using FluentAssertions;

namespace EventSourcingTests
{
    [TestClass]
    public class BDDTests
    {
        [TestMethod]
        public void NeueNachrichtKannGesendetWerden()
        {
            var channel = new ChannelAggregateRoot();
            var nachricht = new Nachricht("Hallo Welt");
            var expectedResult = new ChatNachrichtGeschickt(nachricht);
            
            var chatNachrichtGeschickt = channel.SendeNachricht(nachricht);
            
            chatNachrichtGeschickt.Should().Be(expectedResult);
        }


        [TestMethod]
        public void LeereNachrichtKannNichtGesendetWerden()
        {
            var channel = new ChannelAggregateRoot();
            var nachricht = new Nachricht("");
            var expectedResult = new ChatNachrichtGeschickt(nachricht);
            
            Action act = () => channel.SendeNachricht(nachricht);

            act.Should().Throw<ArgumentException>().WithMessage("*leer*");
        }
    }
}