using Assets.Scripts.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerManager
{
    public List<Army> Players { get; set; }
    public Army ActivePlayer { get; set; }
    private int activePlayerNumber;
    public int ActivePlayerNumber
    {
        get
        {
            return activePlayerNumber;
        }
        set
        {
            if (value >= Players.Count)
                activePlayerNumber = 0;
            else
                activePlayerNumber = value;
            ActivePlayer = Players[activePlayerNumber];
        }
    }

    public PlayerManager()
    {
        Players = new List<Army>();
        PlayerConfig transform = GameObject.FindObjectOfType<PlayerConfig>();
        foreach (Army p in transform.armys)
        {
            Players.Add(p);
            p.Init();
        }
    }
    public Army GetRealPlayer()
    {
        return Players[0];
    }
}

