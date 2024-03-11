using Mirror;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject _container;
    [SerializeField]
    private Button _actionButton;
    public Action OnActionClick;

    [SerializeField]
    private Button _useButton;
    [SerializeField]
    private Button _reportButton;

    public void SetActionInteract(bool canInteract)
    {
        _actionButton.interactable = canInteract;
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
        OnActionClick?.Invoke();
    }

    public void Report()
    {

    }

    public void Use()
    {

    }

    public void UpdateUI(RoleDataSO roleData)
    {
        _actionButton.gameObject.SetActive(roleData.Action != null);
        _actionButton.GetComponent<Image>().sprite = roleData.SpaceIcon;
    }    

    public void HideAction()
    {
        _actionButton.gameObject.SetActive(false);
        _reportButton.gameObject.SetActive(false);
    }

    public void Show()
    {
        _container.SetActive(true);
    }

    public void Hide()
    {
        _container.SetActive(false);
    }
}
