using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDead : MonoBehaviour
{
    [SerializeField]
    private GameObject _container;
    [SerializeField]
    private float _motionTime = 1f;

    public void Show()
    {
        StartCoroutine(Dead());
    }

    private IEnumerator Dead()
    {
        _container.SetActive(true);
        yield return new WaitForSeconds(_motionTime);
        _container.SetActive(false);
    }
}
