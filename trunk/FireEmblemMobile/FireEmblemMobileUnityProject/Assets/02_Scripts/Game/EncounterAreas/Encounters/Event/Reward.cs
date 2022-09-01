using System;
using Game.GameActors.Items;
using Game.GameActors.Units.Skills;
using LostGrace;

[Serializable]
public class Reward
{
    public Item item;
    public Skill skill;
    public Blessing Blessing;
    public int gold;
    public int smithingStones;
    public int experience;
}