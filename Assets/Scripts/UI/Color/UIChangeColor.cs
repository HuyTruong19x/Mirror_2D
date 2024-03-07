using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChangeColor : MonoBehaviour
{
    [SerializeField]
    private VoidChannelEventSO _requestChangeColorEventSO;
    [SerializeField]
    private GameObject _container;
    [SerializeField]
    private List<ColorSO> _colors;
    [SerializeField]
    private Button _colorButton;
    [SerializeField]
    private Transform _colorGroup;

    private void OnEnable()
    {
        _requestChangeColorEventSO.AddListener(Show);
    }

    private void OnDisable()
    {
        _requestChangeColorEventSO.RemoveListener(Show);
    }

    private void Awake()
    {
        _container.SetActive(false);
        InitializeUI();
    }

    private void Show()
    {
        _container.SetActive(true);
    }    

    private void InitializeUI()
    {
        foreach (var color in _colors)
        {
            var btn = Instantiate(_colorButton, _colorGroup);
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(RequestChangeColor);

            btn.GetComponent<Image>().color = color.Color;

            btn.gameObject.SetActive(true);
        }
    }    

    private void RequestChangeColor()
    {

    }    
}
