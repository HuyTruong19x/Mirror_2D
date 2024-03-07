using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFakeStartGame : MonoBehaviour
{
    [SerializeField]
    private GameObject _container;
    [SerializeField]
    private VoidChannelEventSO _starGameEventSO;
    [SerializeField]
    private float _disappearedTime;

    private void OnEnable()
    {
        _starGameEventSO.AddListener(Show);
    }

    private void OnDisable()
    {
        _starGameEventSO?.RemoveListener(Show);
    }

    private void Awake()
    {
        _container.SetActive(false);
    }

    public void Show()
    {
        _container.SetActive(true);
        StartCoroutine(DelayToDisappear(_disappearedTime));
    }   
    
    private IEnumerator DelayToDisappear(float time)
    {
        yield return new WaitForSeconds(time);
        _container.SetActive(false);
    }    
}
