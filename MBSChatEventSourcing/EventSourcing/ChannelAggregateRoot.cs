namespace EventSourcing;

public record NachrichtenCounter(User User, int AnzahlNachrichtenInFolge);

public class ChannelAggregateRoot
{
    private NachrichtenCounter? _nachrichtenCounter;
    
    public ChatNachrichtGeschickt SendeNachricht(Nachricht nachricht)
    {
        if (nachricht.Inhalt == string.Empty)
            throw new ArgumentException(
                "Die Nachricht darf nicht leer sein.");

        if(nachricht.Inhalt.Length > 1024)
            throw new ArgumentException(
                "Die Nachricht darf nicht mehr als 1024 Zeichen haben.");

        if (_nachrichtenCounter is null || _nachrichtenCounter.User != nachricht.User)
        {
            _nachrichtenCounter = new NachrichtenCounter(nachricht.User, 1);
        }
        else
        {
            if (_nachrichtenCounter.AnzahlNachrichtenInFolge > 2)
                throw new ArgumentException("Limit von 3 Nachrichten pro User erreicht");
            
            _nachrichtenCounter = _nachrichtenCounter with
            {
                AnzahlNachrichtenInFolge = _nachrichtenCounter.AnzahlNachrichtenInFolge + 1
            };
        }
        
        return new ChatNachrichtGeschickt(nachricht);
    }
}