﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade", fileName = "MetaUpgrade1")]
public class MetaUpgradeBP: ScriptableObject
{
    public string label;
    public int maxLevel;
    public int[] costToLevel;
    public string Description;
    public Sprite icon;
    public int xPosInTree=0;
    public int yPosInTree=0;
    public int requiredFlameLevel = 0;
    public bool toggle = false;
    public List<MetaUpgradeMixin> mixins;
}