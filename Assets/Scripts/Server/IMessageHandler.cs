using Mirror;

public interface IMessageHandler
{
    public void Handle(NetworkConnectionToClient conn, NetworkMessage message);
}

public abstract class MessageHandler<T> : IMessageHandler where T : NetworkMessage
{
    public void Handle(NetworkConnectionToClient conn, NetworkMessage msg)
    {
        Handle(conn, (T)msg);
    }

    public abstract void Handle(NetworkConnectionToClient conn, T message);
}