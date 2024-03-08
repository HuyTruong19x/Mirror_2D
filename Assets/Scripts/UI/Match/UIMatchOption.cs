using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMatchOption : MonoBehaviour
{
    [SerializeField]
    private VoidChannelEventSO _requestRoomOptionEventSO;
    [SerializeField]
    private GameObject _lockBG;
    [SerializeField]
    private GameObject _container;
    [SerializeField]
    private MatchInfoSO _matchInfoSO;

    [Header("Discuss")]
    [SerializeField]
    private Slider _sliderDiscuss;
    [SerializeField]
    private InputField _inputDiscuss;

    [Header("Vote")]
    [SerializeField]
    private Slider _sliderVote;
    [SerializeField]
    private InputField _inputVote;

    private void OnEnable()
    {
        _requestRoomOptionEventSO.AddListener(Show);
        _matchInfoSO.OnMatchInfoChanged += OnMatchOptionChanged;
    }

    private void OnDisable()
    {
        _requestRoomOptionEventSO.RemoveListener(Show);
        _matchInfoSO.OnMatchInfoChanged -= OnMatchOptionChanged;
    }

    private void Awake()
    {
        _lockBG.SetActive(false);
        _container.SetActive(false);
    }

    private void Show()
    {
        _lockBG.SetActive(true);
        _container.SetActive(true);
    }

    private void OnMatchOptionChanged(MatchInfo matchInfo)
    {
        _sliderDiscuss.value = matchInfo.DiscussTime;
        _inputDiscuss.text = matchInfo.DiscussTime.ToString();

        _sliderVote.value = matchInfo.VoteTime;
        _inputVote.text = matchInfo.VoteTime.ToString();
    }    
}
