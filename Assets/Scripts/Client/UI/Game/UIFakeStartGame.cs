using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFakeStartGame : MonoBehaviour
{
    [SerializeField]
    private GameObject _container;
    [SerializeField]
    private float _disappearedTime;
    [SerializeField]
    private Text _roleName;

    private void Awake()
    {
        _container.SetActive(false);
    }

    public void Show(Player localPlayer, List<Player> players)
    {
        SetupUI(localPlayer, players);
        _container.SetActive(true);
        StartCoroutine(DelayToDisappear(_disappearedTime));
    }   

    private void SetupUI(Player localPlayer, List<Player> players)
    {
        _roleName.text = localPlayer.Role.Name;
        _roleName.color = localPlayer.Role.Type == RoleType.NONE ? Color.green : Color.red;
    }    
    
    private IEnumerator DelayToDisappear(float time)
    {
        yield return new WaitForSeconds(time);
        _container.SetActive(false);
    }    
}
