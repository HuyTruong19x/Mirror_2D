using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    [SerializeField]
    private GameObject _container;
    [SerializeField]
    private Slider _questBar;

    private void Awake()
    {
        Hide();
    }

    public void Show()
    {
        _container.SetActive(true);
    }    

    public void Hide()
    {
        _container.SetActive(false);
    }

    public void InitQuest(int total)
    {
        _questBar.value = 0;
        _questBar.maxValue = total;
    }    

    public void FinishQuest(int count)
    {
        _questBar.value += count;
    }    
}
