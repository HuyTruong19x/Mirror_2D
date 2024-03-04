using Mirror;

public struct GameMessage : NetworkMessage
{
    public ActionChannel Channel;
    public Data Message;
}

public class Data
{

}

public enum ActionChannel : byte
{
    NONE = 0,
    ROOM = 1,
    GAME = 2,
}
