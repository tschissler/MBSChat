namespace EventSourcing;

public interface IEvent { 
    Nachricht Nachricht { get; }
}

public record ChatNachrichtGeschicktEvent(Nachricht Nachricht) : IEvent;
public record ChatNachrichtGel�schtEvent(Nachricht Nachricht) : IEvent;
public record ChatNachrichtBearbeitetEvent(
    Nachricht Nachricht, string NeuerInhalt) : IEvent;