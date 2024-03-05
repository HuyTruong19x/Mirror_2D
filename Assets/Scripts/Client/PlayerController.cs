using Mirror;
using System;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public Guid GameId;
    private Rigidbody2D _rid2D;

    [ServerCallback]
    private void OnDestroy()
    {
        Disconnected(connectionToClient);
    }

    private void Start()
    {
        _rid2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            var x = Input.GetAxisRaw("Horizontal");
            var y = Input.GetAxisRaw("Vertical");
            _rid2D.velocity = new Vector2(x, y) * 5;
        }
    }

    [ClientRpc]
    public void StartGame(Vector3 position)
    {
        transform.position = position;
    }

    [ServerCallback]
    public void Disconnected(NetworkConnectionToClient conn)
    {
        RoomManager.Instance.LeaveRoom(conn);
        GameManager.Instance.RemovePlayerFromGame(GameId, this);
    }
}
