using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preload : MonoBehaviour
{
    [SerializeField]
    private GameObject _serverText;

    // Start is called before the first frame update
    void Start()
    {
        if(Application.isBatchMode || NetworkManager.singleton.editorAutoStart)
        {
            Debug.Log("====== Game Server Starting ======");
            _serverText.SetActive(true);
        }
        else
        {
            Debug.Log("====== Game Client Starting ======");
            NetworkManager.singleton.StartClient();
            SceneManager.LoadScene(1);
        }    
    }
}
