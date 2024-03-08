using Mirror;
using UnityEngine;

public class PlayerCamera : NetworkBehaviour
{
    private Camera _mainCam;

    void Awake()
    {
        _mainCam = Camera.main;
    }

    public override void OnStartLocalPlayer()
    {
        if (_mainCam != null)
        {
            _mainCam.transform.SetParent(transform);
        }
        else
            Debug.LogWarning("PlayerCamera: Could not find a camera in scene with 'MainCamera' tag.");
    }

    public override void OnStopLocalPlayer()
    {
        if (_mainCam != null)
        {
            _mainCam.transform.SetParent(null);
        }
    }

    public void UpdateViewLayer(LayerMask layerMask)
    {
        if (_mainCam != null)
        {
            _mainCam.cullingMask = layerMask;
        }
    }
}
