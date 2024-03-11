using System;
using UnityEngine;
using UnityEngine.UI;

public class UIMeetingPlayer : MonoBehaviour
{
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Text _playerName;
    [SerializeField]
    private GameObject _voted;

    private Player _player;

    public Action<string> OnVoted;

    public void Setup(Player player)
    {
        _voted.SetActive(false);
        _player = player;
        _playerName.text = player.PlayerInfo.Name;
    }

    public void SetInteractable(bool isAble)
    {
        _button.interactable = isAble;
    }    

    public void Vote()
    {
        OnVoted?.Invoke(_player.PlayerInfo.ID);
    }

    public void SetVoted(bool isVoted)
    {
        _voted.SetActive(isVoted);
    }
}
