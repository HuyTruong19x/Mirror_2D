using Mirror;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    [SerializeField]
    private Button _actionButton;
    public Action OnClick;

    [SerializeField]
    private Button _useButton;
    [SerializeField]
    private Button _reportButton;

    public void SetActionInteract(bool canInteract)
    {
        _actionButton.interactable = canInteract;
    }  

    public void ShowAction()
    {
        _actionButton.gameObject.SetActive(true);
    }

    public void HideAction()
    {
        _actionButton.gameObject.SetActive(false);
    }

    public void SetReportInteract(bool canInteract)
    {
        _reportButton.interactable = canInteract;
    }

    public void SetUseInteract(bool canInteract)
    {
        _useButton.interactable = canInteract;
    }

    public void Action()
    {
        OnClick?.Invoke();
    }

    public void Report()
    {

    }

    public void Use()
    {

    }
}
