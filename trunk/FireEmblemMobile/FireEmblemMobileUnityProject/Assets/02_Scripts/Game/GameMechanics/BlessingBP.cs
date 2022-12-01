﻿using Game.GameActors.Units.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Blessing", fileName = "Blessing")]
    public class BlessingBP : ScriptableObject
    {
        [FormerlySerializedAs("skill")] public SkillBP skillBp;
        public string effectDescription;
        public string name;
        public string Description;
        public int tier=3;

        public Blessing Create()
        {
            return new Blessing(skillBp.Create(), effectDescription, name, Description, tier);
        }

    }
}