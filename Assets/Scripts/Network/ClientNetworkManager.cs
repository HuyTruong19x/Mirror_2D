using Mirror;
using Mirror.Examples.MultipleMatch;
using System.Security.Cryptography;
using UnityEngine;

public class ClientNetworkManager : MonoBehaviour
{
    public string MatchID;
    [SerializeField]
    private MatchInfoSO _localMatchInfo;

    public void OnClientConnect()
    {

    }

    public void OnClientDisconnect()
    {
    }

    public void OnStartClient()
    {
        NetworkClient.RegisterHandler<ClientMatchMessage>(OnClientMessage);
        NetworkClient.RegisterHandler<ClientMatchInfoMessage>(OnMatchInfoChangeMessage);
    }

    public void OnStopClient()
    {
        NetworkClient.UnregisterHandler<ClientMatchMessage>();
    }

    [ClientCallback]
    public void RequestCreateRoom()
    {
        var playerInfo = new PlayerInfo()
        {
            ID = "ID_" + Random.Range(0, 100000),
            Name = "User_" + Random.Range(0, 100000)
        };
        NetworkClient.Send(new ServerMatchMessage()
        {
            Operation = MatchOperation.CREATE,
            MatchInfo = new MatchInfo()
            {
                HostName = playerInfo.Name,
                Mode = "Random",
                Map = "Map_0",
                MaxPlayer = 16,
                RaiseTime = 15,
                DiscussTime = 180,
                VoteTime = 30
            },
            PlayerInfo = playerInfo// TODO get player info from data
        });
    }

    [ClientCallback]
    public void RequestRoomList()
    {
        NetworkClient.Send(new ServerMatchMessage()
        {
            Operation = MatchOperation.LIST,
        });
    }

    [ClientCallback]
    public void RequestJoinRoom(string roomId)
    {
        var playerInfo = new PlayerInfo()
        {
            ID = "ID_" + Random.Range(0, 100000),
            Name = "User_" + Random.Range(0, 100000)
        };
        NetworkClient.Send(new ServerMatchMessage()
        {
            Operation = MatchOperation.JOIN,
            MatchID = roomId,
            PlayerInfo = playerInfo// TODO get player info from data
        });
    }

    [ClientCallback]
    public void RequestQuickJoin()
    {
        var playerInfo = new PlayerInfo()
        {
            ID = "ID_" + Random.Range(0, 100000),
            Name = "User_" + Random.Range(0, 100000)
        };
        NetworkClient.Send(new ServerMatchMessage()
        {
            Operation = MatchOperation.QUICK_JOIN,
            PlayerInfo = playerInfo// TODO get player info from data
        });
    }

    [ClientCallback]
    private void OnClientMessage(ClientMatchMessage message)
    {
        EventDispatcher.Dispatch(MessageCode.MATCH, message);
    }

    [ClientCallback]
    private void OnMatchInfoChangeMessage(ClientMatchInfoMessage message)
    {
        _localMatchInfo.SetInfo(message.Info);
    }
}
