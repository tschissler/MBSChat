namespace MBSChatStandAlone;

public class MessageService
{
    public List<string> Messages = new();

    public void AddMessage(string aMessage)
    {
        Messages.Add(aMessage);
        Notify?.Invoke();
	}

    public event Action? Notify;
}
