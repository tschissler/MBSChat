namespace EventSourcing;

public record Nachricht(string Inhalt, User User)
{
    public Guid Guid { get; } = Guid.NewGuid();

    public void Deconstruct(out string Inhalt, out User User, out Guid Guid)
    {
        Inhalt = this.Inhalt;
        User = this.User;
        Guid = this.Guid;
    }
}