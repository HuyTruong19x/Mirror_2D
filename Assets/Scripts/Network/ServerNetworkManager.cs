using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerNetworkManager : MonoBehaviour
{
    public void OnServerReady(NetworkConnectionToClient conn)
    {

    }    

    public IEnumerator OnServerDisconnect(NetworkConnectionToClient conn)
    {
        yield break;
    }    

    public void OnStartServer()
    {

    }   
    
    public void OnStopServer()
    {

    }    
}
