using Mirror;
using Mirror.Examples.MultipleMatch;
using System.Security.Cryptography;
using UnityEngine;

public class ClientNetworkManager : MonoBehaviour
{
    [SerializeField]
    private MatchInfoSO _localMatchInfo;
    [SerializeField]
    private LocalPlayerDataSO _playerData;

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

    #region REQUEST

    [ClientCallback]
    public void RequestCreateRoom()
    {
        _localMatchInfo.Info = new MatchInfo()
        {
            HostName = _playerData.Data.Name + "'s Room",
            Mode = "Random",
            Map = "Map_0",
            MaxPlayer = 16,
            RaiseTime = 15,
            DiscussTime = 180,
            VoteTime = 30
        };

        NetworkClient.Send(new ServerMatchMessage()
        {
            Operation = MatchOperation.CREATE,
            MatchInfo = _localMatchInfo.Info,
            PlayerInfo = _playerData.Data
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
    public void RequestJoinRoom(MatchInfo matchInfo)
    {
        if(string.IsNullOrEmpty(matchInfo.ID))
        {
            Debug.Log("Please selected room before joining");
            return;
        }    

        _localMatchInfo.Info = matchInfo;

        NetworkClient.Send(new ServerMatchMessage()
        {
            Operation = MatchOperation.JOIN,
            MatchID = matchInfo.ID,
            PlayerInfo = _playerData.Data
        });
    }

    [ClientCallback]
    public void RequestQuickJoin()
    {
        NetworkClient.Send(new ServerMatchMessage()
        {
            Operation = MatchOperation.QUICK_JOIN,
            PlayerInfo = _playerData.Data
        });
    }

    [ClientCallback]
    public void RequestLoadedGame()
    {
        NetworkClient.Send(new ServerMatchMessage()
        {
            Operation = MatchOperation.LOADED_GAME_SCENE
        });
    }

    [ClientCallback]    
    public void RequestStartGame()
    {
        NetworkClient.Send(new ServerMatchMessage()
        {
            Operation = MatchOperation.START_GAME
        });
    }

    [ClientCallback]   
    public void RequestLeaveGame()
    {
        NetworkClient.Send(new ServerMatchMessage()
        {
            Operation = MatchOperation.LEAVE
        });
    }

    #endregion

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

    [ClientCallback]
    public void SetMatchID(string matchID)
    {
        _localMatchInfo.Info.ID = matchID;
    }    
}
