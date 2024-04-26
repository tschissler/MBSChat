namespace EventSourcing
{
    public class ChannelAggregateRoot
    {
        public ChatNachrichtGeschickt SendeNachricht(Nachricht nachricht)
        {
            if (nachricht.Inhalt == string.Empty)
                throw new ArgumentException("Die Nachricht darf nicht leer sein.");

            return new ChatNachrichtGeschickt(nachricht);
        }
    }
}
