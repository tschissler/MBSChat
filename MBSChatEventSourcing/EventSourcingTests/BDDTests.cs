using EventSourcing;
using FluentAssertions;

namespace EventSourcingTests;

[TestClass]
public class BDDTests
{
    [TestMethod]
    public void NeueNachrichtKannGesendetWerden()
    {
        var channel = new ChannelAggregateRoot();
        var nachricht = new Nachricht("Hallo Welt", new User("Teddy Tester"));
        var expectedResult = new ChatNachrichtGeschickt(nachricht);
            
        var chatNachrichtGeschickt = channel.SendeNachricht(nachricht);
            
        chatNachrichtGeschickt.Should().Be(expectedResult);
    }

    [TestMethod]
    public void LeereNachrichtKannNichtGesendetWerden()
    {
        var channel = new ChannelAggregateRoot();
        var nachricht = new Nachricht(string.Empty, new User("Rainer Zufall"));
            
        Action act = () => channel.SendeNachricht(nachricht);

        act.Should().Throw<ArgumentException>().WithMessage("*leer*");
    }

    [TestMethod]
    public void ZuLangeNachrichtKannNichtGesendetWerden()
    {
        var channel = new ChannelAggregateRoot();
        var nachricht = new Nachricht(new string('t', 1025), new User("Arno Amoebe"));

        Action act = () => channel.SendeNachricht(nachricht);

        act.Should().Throw<ArgumentException>().WithMessage("*mehr als*");
    }

    [TestMethod]
    public void UserKannNichtMehrAlsDreiNachrichtenInFolgeSchreiben()
    {
        var channel = new ChannelAggregateRoot();
        var user = new User("Arno Amoebe");

        channel.SendeNachricht(new Nachricht("a", user));
        channel.SendeNachricht(new Nachricht("b", user));
        channel.SendeNachricht(new Nachricht("c", user));
        Action act = () => channel.SendeNachricht(new Nachricht("d", user));

        act.Should().Throw<ArgumentException>().WithMessage("*Limit*");
    }

    [TestMethod]
    public void UserCounterWirdResettetWennUserWechselt()
    {
        var channel = new ChannelAggregateRoot();
        var user = new User("Arno Amoebe");
        var letzteNachricht = new Nachricht("d", user);
        var expectedResult = new ChatNachrichtGeschickt(letzteNachricht);

        channel.SendeNachricht(new Nachricht("a", user));
        channel.SendeNachricht(new Nachricht("b", user));
        channel.SendeNachricht(new Nachricht("c", user));
        channel.SendeNachricht(new Nachricht("Hi!", new User("Teddy Tester")));
        var chatNachrichtGeschickt = channel.SendeNachricht(letzteNachricht);
            
        chatNachrichtGeschickt.Should().Be(expectedResult);
    }
}