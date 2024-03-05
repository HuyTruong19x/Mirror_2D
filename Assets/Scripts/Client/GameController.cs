using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetworkClient.Send(new GameMessage()
        {
            doing = 1
        });
    }
}
