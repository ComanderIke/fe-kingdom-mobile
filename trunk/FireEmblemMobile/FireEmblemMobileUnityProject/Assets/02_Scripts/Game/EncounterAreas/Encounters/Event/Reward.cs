using System;
using Game.GameActors.Items;
using Game.GameActors.Units.Skills;
using LostGrace;
using UnityEngine.Serialization;

[Serializable]
public class Reward
{
    [FormerlySerializedAs("item")] public ItemBP itemBp;
    [FormerlySerializedAs("skill")] public SkillBP skillBp;
    public Blessing Blessing;
    public int gold;
    public int smithingStones;
    public int experience;
}