using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMeetingPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject _container;
    [SerializeField]
    private GameObject _deadLayout;
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Text _playerName;
    [SerializeField]
    private GameObject _voted;
    [SerializeField]
    private GameObject _killed;
    [SerializeField]
    private GameObject _found;
    [SerializeField]
    private Transform _voteCountParent;
    [SerializeField]
    private GameObject _voteCount;
    [SerializeField]
    private GameObject _speaker;


    private List<GameObject> _votes = new();
    private Player _player;

    public Action<string> OnVoted;

    public void Clear()
    {
        _container.SetActive(false);
    }    

    public void Setup(string raisePlayerId, Player player, Player deadPlayer)
    {
        _votes.Clear();

        for (int i = 1; i < _voteCountParent.childCount; i++)
        {
            Destroy(_voteCountParent.GetChild(i).gameObject);
        }

        _voted.SetActive(false);
        _player = player;
        _playerName.text = player.PlayerInfo.Name;

        var isDead = _player.State == PlayerState.DEAD;
        var isFound = deadPlayer != null ? _player.PlayerInfo.ID == deadPlayer.PlayerInfo.ID : false;
        _killed.SetActive(isDead);
        _deadLayout.SetActive(isDead);
        _found.SetActive(isFound);
        _speaker.SetActive(_player.PlayerInfo.ID == raisePlayerId);

        _container.SetActive(true);
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

    public void AddVoteCount()
    {
        _votes.Add(Instantiate(_voteCount, _voteCountParent));
    }    

    public IEnumerator ShowVoteCount(float delay)
    {
        Debug.Log(gameObject.name + " has " + _votes.Count);
        for (int i = 0; i < _votes.Count; i++)
        {
            Debug.Log(gameObject.name + " Show vote at index " + i);
            _votes[i].SetActive(true);
            yield return new WaitForSeconds(delay);
        }
    }    
}
