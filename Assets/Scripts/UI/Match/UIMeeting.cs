using System.Collections;
using System.Collections.Generic;
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
    private Button _skipButton;
    [SerializeField]
    private Text _voteResult;
    [SerializeField]
    private float _delayShowVoteCount = 0.5f;

    [Header("SkipVote")]
    [SerializeField]
    private Transform _skipVoteParent;
    [SerializeField]
    private GameObject _skipVote;
    private List<GameObject> _skipVotes = new();

    private Dictionary<string, UIMeetingPlayer> _uiMeetingPlayer = new();

    [SerializeField]
    private List<UIMeetingPlayer> _uiMeetingPlayers = new();

    private void Awake()
    {
        _container.SetActive(false);
    }

    public void Show(string raisePlayerId, List<Player> playes, Player deadPlayer)
    {
        foreach (var item in _uiMeetingPlayers)
        {
            item.Clear();
        }

        _uiMeetingPlayer.Clear();
        _skipVotes.Clear();
        _skipButton.interactable = true;

        for (int i = 1; i < _skipVoteParent.childCount; i++)
        {
            Destroy(_skipVoteParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < playes.Count; i++)
        {
            var pm = _uiMeetingPlayers[i];
            pm.Setup(raisePlayerId, playes[i], deadPlayer);
            pm.OnVoted = Vote;
            pm.gameObject.SetActive(true);
            pm.SetInteractable(true); // TODO only work in voting time
            _uiMeetingPlayer.Add(playes[i].PlayerInfo.ID, pm);
        }

        _container.SetActive(true);
        _votingObject.SetActive(true);
        _endVotingObject.SetActive(false);

        if (Player.Local.State == PlayerState.DEAD)
        {
            LockVote();
        }
    }

    private void Vote(string playerId)
    {
        Player.Local.Vote(playerId);
        LockVote();
    }

    private void LockVote()
    {
        _skipButton.interactable = false;
        foreach (var item in _uiMeetingPlayer.Values)
        {
            item.SetInteractable(false);
        }
    }

    public void UpdateVote(string playerIdVote, string playerIdTarget)
    {
        if (_uiMeetingPlayer.ContainsKey(playerIdVote))
        {
            _uiMeetingPlayer[playerIdVote].SetVoted(true);
            if (playerIdTarget != null && _uiMeetingPlayer.ContainsKey(playerIdTarget))
            {
                _uiMeetingPlayer[playerIdTarget].AddVoteCount();
            }
            else
            {
                _skipVotes.Add(Instantiate(_skipVote, _skipVoteParent));
            }
        }
    }

    public void Skip()
    {
        Player.Local.Vote(null);
        _skipButton.interactable = false;
    }

    public IEnumerator EndVote(string playerName)
    {
        _voteResult.text = string.IsNullOrEmpty(playerName) ? "Everyone now still live!" : playerName + " is going to the hell";
        yield return ShowResult();
    }

    private IEnumerator ShowResult()
    {
        yield return new WaitForSeconds(2);

        foreach (var item in _uiMeetingPlayer.Values)
        {
            item.SetVoted(false);
        }

        yield return new WaitForSeconds(2);

        foreach (var item in _uiMeetingPlayer.Values)
        {
            Debug.Log("Show vote count");
            yield return item.ShowVoteCount(_delayShowVoteCount);
        }

        foreach (var item in _skipVotes)
        {
            Debug.Log("Show skip voted");
            yield return new WaitForSeconds(_delayShowVoteCount);
            item.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(1);

        _endVotingObject.SetActive(true);
        _votingObject.SetActive(false);
        yield return new WaitForSeconds(3);
        _container.SetActive(false);
        //TODO show loading scene
    }
}
