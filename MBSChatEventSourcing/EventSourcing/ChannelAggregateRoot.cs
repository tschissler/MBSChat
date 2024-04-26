namespace EventSourcing
{
    public class ChannelAggregateRoot
    {
        public ChatNachrichtGeschickt SendeNachricht(Nachricht nachricht)
        {
            if (nachricht.Inhalt == string.Empty)
                throw new ArgumentException(
                    "Die Nachricht darf nicht leer sein.");

            if(nachricht.Inhalt.Length > 1024)
				throw new ArgumentException(
                    "Die Nachricht darf nicht mehr als 1024 Zeichen haben.");

			return new ChatNachrichtGeschickt(nachricht);
        }
    }
}
