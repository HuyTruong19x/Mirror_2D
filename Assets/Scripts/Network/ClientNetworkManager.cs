using Mirror;
using UnityEngine;

public class ClientNetworkManager : MonoBehaviour
{
    public string RoomID;

    public void OnClientConnect()
    {

    }

    public void OnClientDisconnect()
    {

    }

    public void OnStartClient()
    {
        NetworkClient.RegisterHandler<ClientRoomMessage>(OnClientMessage);
        NetworkClient.RegisterHandler<GameMessage>(OnGameMessage);
    }

    public void OnStopClient()
    {

    }

    [ClientCallback]
    public void RequestCreateRoom()
    {
        NetworkClient.Send(new ServerRoomMessage()
        {
            Operation = ServerRoomOperation.CREATE
        });
    }

    [ClientCallback]
    public void RequestRoomList()
    {
        NetworkClient.Send(new ServerRoomMessage()
        {
            Operation = ServerRoomOperation.LIST,
        });
    }

    [ClientCallback]
    public void RequestJoinRoom(string roomId)
    {
        NetworkClient.Send(new ServerRoomMessage()
        {
            Operation = ServerRoomOperation.JOIN,
            RoomID = roomId
        });
    }

    private void OnClientMessage(ClientRoomMessage message)
    {
        EventDispatcher.Dispatch(ActionChannel.ROOM, message);
    }

    private void OnGameMessage(GameMessage message)
    {
        EventDispatcher.Dispatch(ActionChannel.GAME, message);
    }
}
