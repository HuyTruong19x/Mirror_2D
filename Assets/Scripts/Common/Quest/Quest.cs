using System;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public Action OnFinish;

    [SerializeField]
    private int _id;
    public int ID => _id;

    [SerializeField]
    private string _name;

    [SerializeField]
    private GameObject _container;

    private void Awake()
    {
        _container.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerUI>();

        if(player != null )
        {
            player.SetQuestAction(this);
        }    
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerUI>();

        if (player != null)
        {
            player.SetQuestAction(null);
        }
    }

    public void Show()
    {
        _container.SetActive(true);
    }    

    public void Finish()
    {
        OnFinish?.Invoke();
        gameObject.SetActive(false);
    }    
}
