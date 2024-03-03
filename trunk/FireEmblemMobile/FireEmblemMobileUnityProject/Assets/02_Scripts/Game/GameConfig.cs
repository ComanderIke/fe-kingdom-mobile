using System;
using System.Collections.Generic;
using Game.GameActors.Items.Consumables;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using Game.GameResources;
using GameEngine;
using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameConfig/Config", fileName = "GameConfig")]
    public class GameConfig : SingletonScriptableObject<GameConfig>
    {
        public GameConfigProfile ConfigProfile;
        public static Attributes BonusStartAttributes;
        public static Attributes BonusAttributeGrowths;

        public  List<Unit> GetPlayerUnits()
        {
            var units =ConfigProfile.GetUnits();
            InitializePlayerUnits(units);
            return units;
        }

        public void InitializePlayerUnits(List<Unit> units)
        {
            foreach (var unit in units)
            {
                if(BonusStartAttributes!=null)
                    unit.Stats.BaseAttributes += BonusStartAttributes;
                if (BonusAttributeGrowths != null)
                {
                    unit.Stats.BaseGrowths += BonusAttributeGrowths;
                }
                if (Player.Instance.Flags.StrongestAttributeIncrease)
                {
                    int strongest = 0;
                    int strongestIndex = 0;
                    var statArray = unit.Stats.BaseAttributes.AsArray();
                    for(int i=0; i < statArray.Length; i++)
                    {
                        if(i==4)//Skip MaxHp
                            continue;
                        if (statArray[i] > strongest)
                        {
                            strongest = statArray[i];
                            strongestIndex = i;
                        }
                            
                    }
                    unit.Stats.BaseAttributes.IncreaseAttribute(1, (AttributeType)strongestIndex);
                }

                if (Player.Instance.Flags.WeakestAttributeIncrease)
                {
                    int weakest = 99;
                    int weakestIndex = 0;
                    var statArray = unit.Stats.BaseAttributes.AsArray();
                    for(int i=0; i < statArray.Length; i++)
                    {
                        if(i==4)//Skip MaxHp
                            continue;
                        if (statArray[i] < weakest)
                        {
                            weakest = statArray[i];
                            weakestIndex = i;
                        }
                            
                    }
                    unit.Stats.BaseAttributes.IncreaseAttribute(1, (AttributeType)weakestIndex);
                }
            }
        }
       
    }
}