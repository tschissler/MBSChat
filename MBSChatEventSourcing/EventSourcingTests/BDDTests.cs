#nullable enable
using EventSourcing;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            
            Action act = () => channel.SendeNachricht(nachricht);

            act.Should().Throw<ArgumentException>().WithMessage("*leer*");
        }

        [TestMethod]
        public void ZuLangeNachrichtKannNichtGesendetWerden()
        {
			var channel = new ChannelAggregateRoot();
			var nachricht = new Nachricht(new string('t', 1025));

			Action act = () => channel.SendeNachricht(nachricht);

			act.Should().Throw<ArgumentException>().WithMessage("*mehr als*");
		}
    }
}