using UnityEngine;

public class WaitingMapObject : MonoBehaviour
{
    [SerializeField]
    private bool _isNeedHost;
    private bool _canAction = false;
    [SerializeField]
    private VoidChannelEventSO _channel;

    public void OnMouseDown()
    {
        if(_canAction)
        {
            Debug.Log("Request action");
            _channel.Raise();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();

        if(player != null && (_isNeedHost && player.IsHost || !_isNeedHost))
        {
            _canAction = true;
        }    
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _canAction = false;
    }
}
