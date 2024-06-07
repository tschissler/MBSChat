using EventSourcing;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventSourcingTests;

[TestClass]
public class BDDTests
{
    [TestMethod]
    public void NeueNachrichtKannGesendetWerden()
    {
        var channel = new ChannelAggregateRoot();
        var nachricht = new Nachricht("Hallo Welt", new User("Teddy Tester"));
        var expectedResult = new ChatNachrichtGeschicktEvent(nachricht);

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
        var nachricht =
            new Nachricht(new string('t', 1025), new User("Arno Amoebe"));

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
        var expectedResult = new ChatNachrichtGeschicktEvent(letzteNachricht);

        channel.SendeNachricht(new Nachricht("a", user));
        channel.SendeNachricht(new Nachricht("b", user));
        channel.SendeNachricht(new Nachricht("c", user));
        channel.SendeNachricht(new Nachricht("Hi!", new User("Teddy Tester")));
        var chatNachrichtGeschickt = channel.SendeNachricht(letzteNachricht);

        chatNachrichtGeschickt.Should().Be(expectedResult);
    }

    [TestMethod]
    public void GesendeteNachrichtenKönnenAlleAusgelesenWerden()
    {
        var channel = new ChannelAggregateRoot();
        var user = new User("Arno Amoebe");

        channel.SendeNachricht(new Nachricht("a", user));
        channel.SendeNachricht(new Nachricht("b", user));
        channel.SendeNachricht(new Nachricht("c", user));

        var chatVerlauf = channel.GetChatVerlauf();

        chatVerlauf.Count().Should().Be(3);
        chatVerlauf.First().Inhalt.Should().Be("a");
        chatVerlauf.Skip(1).First().Inhalt.Should().Be("b");
        chatVerlauf.Skip(2).First().Inhalt.Should().Be("c");
    }


    [TestMethod]
    public void GesendeteNachrichtKannGelöschtWerden()
    {
        var channel = new ChannelAggregateRoot();
        var user = new User("Arno Amoebe");

        var nachricht = new Nachricht("a", user);
        channel.SendeNachricht(nachricht);
        channel.LöscheNachricht(nachricht);
        var chatVerlauf = channel.GetChatVerlauf();

        chatVerlauf.Count().Should().Be(0);
    }

    [TestMethod]
    public void ZweiNachrichtenGesendetEineNachrichtLöschenEineNachrichtÜbrig()
    {
        var channel = new ChannelAggregateRoot();
        var user = new User("Arno Amoebe");

        var nachrichtA = new Nachricht("a", user);
        var nachrichtB = new Nachricht("b", user);
        channel.SendeNachricht(nachrichtA);
        channel.SendeNachricht(nachrichtB);
        channel.LöscheNachricht(nachrichtB);
        var chatVerlauf = channel.GetChatVerlauf();

        chatVerlauf.Single().Inhalt.Should().Be("a");
    }

    [TestMethod]
    public void ZweiNachrichtenGesendetEineNachrichtBearbeitetÄnderungImChatverlauf()
    {
        var channel = new ChannelAggregateRoot();
        var user = new User("Arno Amoebe");

        var nachrichtA = new Nachricht("a", user);
        var nachrichtB = new Nachricht("b", user);
        channel.SendeNachricht(nachrichtA);
        channel.SendeNachricht(nachrichtB);
        channel.BearbeiteNachricht(nachrichtB, "c");
        var chatVerlauf = channel.GetChatVerlauf();

        chatVerlauf.First().Inhalt.Should().Be("a");
        chatVerlauf.Skip(1).First().Inhalt.Should().Be("c");
    }

    [TestMethod]
    public void ZweiNachrichtenGesendetEineNachrichtZweiMalBearbeitetÄnderungImChatverlauf()
    {
        var channel = new ChannelAggregateRoot();
        var user = new User("Arno Amoebe");

        var nachrichtA = new Nachricht("a", user);
        var nachrichtB = new Nachricht("b", user);
        channel.SendeNachricht(nachrichtA);
        channel.SendeNachricht(nachrichtB);
        channel.BearbeiteNachricht(nachrichtB, "c");
        channel.BearbeiteNachricht(nachrichtB, "d");
        var chatVerlauf = channel.GetChatVerlauf();

        chatVerlauf.First().Inhalt.Should().Be("a");
        chatVerlauf.Skip(1).First().Inhalt.Should().Be("d");
        chatVerlauf.Skip(1).First().Guid.Should().Be(nachrichtB.Guid);
    }

    [TestMethod]
    public void GelöschteNachrichtBearbeiten()
    {
        var channel = new ChannelAggregateRoot();
        var user = new User("Arno Amoebe");

        var nachrichtA = new Nachricht("a", user);
        var nachrichtB = new Nachricht("b", user);
        channel.SendeNachricht(nachrichtA);
        channel.LöscheNachricht(nachrichtA);
        channel.SendeNachricht(nachrichtB);

        Action act = () => channel.BearbeiteNachricht(nachrichtA, "c");
        act.Should().Throw<ArgumentException>().WithMessage("*existiert nicht*");
    }

    [TestMethod]
    public void NurExistierendeNachrichtKannBearbeitetWerden()
    {
        var channel = new ChannelAggregateRoot();
        var user = new User("Arno Amoebe");

        var nachrichtA = new Nachricht("a", user);

        Action act = () => channel.BearbeiteNachricht(nachrichtA, "c");
        act.Should().Throw<ArgumentException>().WithMessage("*existiert nicht*");
    }

    [TestMethod]
    public void NurExistierendeNachrichtKannBearbeitetWerden2()
    {
        var channel = new ChannelAggregateRoot();
        var user = new User("Arno Amoebe");
        var inhalt = "a";

        var nachrichtA = new Nachricht(inhalt, user);
        channel.SendeNachricht(nachrichtA);

        Action act = () => channel.BearbeiteNachricht(new Nachricht(inhalt, user), "c");
        act.Should().Throw<ArgumentException>().WithMessage("*existiert nicht*");
    }
}