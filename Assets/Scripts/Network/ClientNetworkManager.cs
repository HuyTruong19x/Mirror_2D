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
        NetworkClient.Send(new ServerRoomMessage()
        {
            Operation = ServerRoomOperation.REMOVE,
        });
        NetworkClient.UnregisterHandler<ClientRoomMessage>();
        NetworkClient.UnregisterHandler<WaitingRoomMessage>();
    }

    public void OnStartClient()
    {
        NetworkClient.RegisterHandler<ClientRoomMessage>(OnClientMessage);
        NetworkClient.RegisterHandler<WaitingRoomMessage>(OnWaitingGameMessage);
    }

    public void OnStopClient()
    {

    }

    [ClientCallback]
    public void RequestCreateRoom()
    {
        NetworkClient.Send(new ServerRoomMessage()
        {
            Operation = ServerRoomOperation.CREATE,
            PlayerInfo = new PlayerInfo()
            {
                ID = Random.Range(0, 100000),
                Name = "User_" + Random.Range(0, 100000)
            }// TODO get player info from data
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
            RoomID = roomId,
            PlayerInfo = new PlayerInfo()
            {
                ID = Random.Range(0, 100000),
                Name = "User_" + Random.Range(0, 100000)
            }// TODO get player info from data
        });
    }

    private void OnClientMessage(ClientRoomMessage message)
    {
        EventDispatcher.Dispatch(ActionChannel.ROOM, message);
    }

    private void OnWaitingGameMessage(WaitingRoomMessage message)
    {
        EventDispatcher.Dispatch(ActionChannel.WAITING_ROOM, message);
    }
}
