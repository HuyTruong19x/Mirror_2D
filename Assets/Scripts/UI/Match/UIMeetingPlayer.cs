using UnityEngine;
using UnityEngine.UI;

public class UIMeetingPlayer : MonoBehaviour
{
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Text _playerName;
    private Player _player;

    public void Setup(Player player)
    {
        _player = player;
        _button.interactable = true;
        _playerName.text = player.PlayerInfo.Name;
    }

    public void Vote()
    {
        _button.interactable = false;
        Player.Local.Vote(_player.PlayerInfo.ID);
    }
}
