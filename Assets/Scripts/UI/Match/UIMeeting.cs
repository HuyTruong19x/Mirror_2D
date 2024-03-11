using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMeeting : MonoBehaviour
{
    [SerializeField]
    private GameObject _container;
    [SerializeField]
    private GameObject _votingObject;
    [SerializeField] 
    private GameObject _endVotingObject;
    [SerializeField]
    private Transform _playerParent;
    [SerializeField]
    private UIMeetingPlayer _player;
    [SerializeField]
    private Button _skipButton;
    [SerializeField]
    private Text _voteResult;

    private Dictionary<string, UIMeetingPlayer> _uiMeetingPlayer = new();

    private void Awake()
    {
        _container.SetActive(false);
    }

    public void Show(List<Player> playes, Player deadPlayer)
    {
        _uiMeetingPlayer.Clear();

        foreach (var player in playes)
        {
            var pm = Instantiate(_player, _playerParent);
            pm.Setup(player);
            pm.OnVoted = Vote;
            pm.gameObject.SetActive(true);
            pm.SetInteractable(true); // TODO only work in voting time
            _uiMeetingPlayer.Add(player.PlayerInfo.ID, pm);
        }

        _container.SetActive(true);
        _votingObject.SetActive(true);
        _endVotingObject.SetActive(false);
    }

    public void Hide()
    {
        _container.SetActive(false);
        for (int i = 1; i < _playerParent.childCount; i++)
        {
            Destroy(_playerParent.GetChild(i).gameObject);
        }
    }

    private void Vote(string playerId)
    {
        _skipButton.interactable = false;
        Player.Local.Vote(playerId);
        foreach (var item in _uiMeetingPlayer.Values)
        {
            item.SetInteractable(false);
        }
    }    

    public void UpdateVote(string playerId)
    {
        if(_uiMeetingPlayer.ContainsKey(playerId))
        {
            _uiMeetingPlayer[playerId].SetVoted(true);
        }    
    }    

    public void Skip()
    {
        Player.Local.Vote(null);
        _skipButton.interactable = false;
    }    

    public void EndVote(string playerName)
    {
        _voteResult.text = string.IsNullOrEmpty(playerName) ? "Everyone now still live!" : playerName + " is going to the hell";
        StartCoroutine(ShowResult());
    }   
    
    private IEnumerator ShowResult()
    {
        _endVotingObject.SetActive(true);
        _votingObject.SetActive(false);
        yield return new WaitForSeconds(3);
        _container.SetActive(false);
    }    
}
