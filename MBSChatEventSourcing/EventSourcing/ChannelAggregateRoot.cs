
namespace EventSourcing;

public record NachrichtenCounter(User User, int AnzahlNachrichtenInFolge);

public class ChannelAggregateRoot
{
    private NachrichtenCounter? _nachrichtenCounter;

    private List<IEvent> store = new();

    public IEnumerable<Nachricht> GetChatVerlauf()
    {
        List<Nachricht> chatverlauf = new();

        foreach (var @event in store)
        {
            switch (@event)
            {
                case ChatNachrichtGeschicktEvent:
                    chatverlauf.Add(@event.Nachricht);
                    break;
                case ChatNachrichtGelöschtEvent:
                    chatverlauf.Remove(@event.Nachricht);
                    break;
                case ChatNachrichtBearbeitetEvent bearbeitetEvent:
                    BearbeiteChatNachricht(chatverlauf, @event, bearbeitetEvent);
                    break;
            }
        }

        return chatverlauf;
    }

    private static void BearbeiteChatNachricht(
        List<Nachricht> chatverlauf,
        IEvent @event,
        ChatNachrichtBearbeitetEvent bearbeitetEvent)
    {
        var alteNachricht = chatverlauf.Single(e => e.Guid == @event.Nachricht.Guid);
        var position = chatverlauf.IndexOf(alteNachricht);

        chatverlauf[position] =
            @event.Nachricht with
            {
                Inhalt = bearbeitetEvent.NeuerInhalt
            };
    }

    public ChatNachrichtGeschicktEvent SendeNachricht(Nachricht nachricht)
    {
        if (nachricht.Inhalt == string.Empty)
            throw new ArgumentException(
                "Die Nachricht darf nicht leer sein.");

        if (nachricht.Inhalt.Length > 1024)
            throw new ArgumentException(
                "Die Nachricht darf nicht mehr als 1024 Zeichen haben.");

        if (_nachrichtenCounter is null ||
            _nachrichtenCounter.User != nachricht.User)
        {
            _nachrichtenCounter = new NachrichtenCounter(nachricht.User, 1);
        }
        else
        {
            if (_nachrichtenCounter.AnzahlNachrichtenInFolge > 2)
                throw new ArgumentException(
                    "Limit von 3 Nachrichten pro User erreicht");

            _nachrichtenCounter = _nachrichtenCounter with
            {
                AnzahlNachrichtenInFolge =
                _nachrichtenCounter.AnzahlNachrichtenInFolge + 1
            };
        }

        var chatNachricht = new ChatNachrichtGeschicktEvent(nachricht);
        store.Add(chatNachricht);

        return chatNachricht;
    }

    public void LöscheNachricht(Nachricht nachricht)
    {
        store.Add(new ChatNachrichtGelöschtEvent(nachricht));
    }

    public void BearbeiteNachricht(Nachricht nachricht, string neuerInhalt)
    {
        if (GetChatVerlauf().All(n => n.Guid != nachricht.Guid))
            throw new ArgumentException("Die Nachricht existiert nicht.");

        store.Add(new ChatNachrichtBearbeitetEvent(nachricht, neuerInhalt));
    }
}