using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIMeeting : MonoBehaviour
{
    [SerializeField]
    private GameObject _container;
    [SerializeField]
    private Transform _playerParent;
    [SerializeField]
    private UIMeetingPlayer _player;

    private void Awake()
    {
        _container.SetActive(false);
    }

    public void Show(List<Player> playes, Player deadPlayer)
    {
        foreach (var player in playes)
        {
            var pm = Instantiate(_player, _playerParent);
            pm.Setup(player);
            pm.gameObject.SetActive(true);
        }

        _container.SetActive(true);
    }

    public void Hide()
    {
        _container.SetActive(false);
        for (int i = 1; i < _playerParent.childCount; i++)
        {
            Destroy(_playerParent.GetChild(i).gameObject);
        }
    }
}
