using Mirror;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    [SerializeField]
    private Button _actionButton;
    private Action OnClick;

    public void SetInteract(bool canInteract)
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

    public void Action()
    {
        OnClick?.Invoke();
    }
}
