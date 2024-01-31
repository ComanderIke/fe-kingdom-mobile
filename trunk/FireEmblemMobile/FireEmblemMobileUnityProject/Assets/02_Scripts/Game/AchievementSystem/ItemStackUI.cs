using System;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Players;
using UnityEngine;

public class ItemStackUI : MonoBehaviour
{
    public RectTransform StackPanel;
    public List<UIItemReceived> BackLog = new List<UIItemReceived>();

    public GameObject AchievementTemplate;
    [SerializeField] private int numberOnScreen = 3;

    private void Start()
    {
        Player.Instance.Party.Convoy.onItemReceived -= ShowItemReceived;
        Player.Instance.Party.Convoy.onItemReceived += ShowItemReceived;
        Player.Instance.Party.Storage.onItemReceived -= ShowItemReceived;
        Player.Instance.Party.Storage.onItemReceived += ShowItemReceived;
    }

    private void OnDestroy()
    {
        Player.Instance.Party.Convoy.onItemReceived -= ShowItemReceived;
        Player.Instance.Party.Storage.onItemReceived -= ShowItemReceived;
    }

    public void ShowItemReceived (Item item)
    {
        var Spawned = Instantiate(AchievementTemplate).GetComponent<UIItemReceived>();
        Spawned.Set(item, this);
        
        //If there is room on the screen
        if (StackPanel.childCount < numberOnScreen)
        {
            Spawned.transform.SetParent(StackPanel, false);
            Spawned.StartDeathTimer();
        }
        else
        {
            Spawned.gameObject.SetActive(false);
            BackLog.Add(Spawned);
        }
    }

    /// <summary>
    /// Add one achievement from the backlog to the screen
    /// </summary>
    public void CheckBackLog ()
    {
        if(BackLog.Count > 0)
        {
            BackLog[0].transform.SetParent(StackPanel, false);
            BackLog[0].gameObject.SetActive(true);
            BackLog[0].StartDeathTimer();
            BackLog.RemoveAt(0);
        }
    }
}