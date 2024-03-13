using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEndGame : MonoBehaviour
{
    [SerializeField]
    private GameObject _container;

    private void Awake()
    {
        Hide();
    }

    public void Show()
    {
        _container.SetActive(true);
        Invoke(nameof(Hide), 2);
        Player.Local.NewGame();
    }

    public void Hide()
    {
        _container.SetActive(false);
    }
}
