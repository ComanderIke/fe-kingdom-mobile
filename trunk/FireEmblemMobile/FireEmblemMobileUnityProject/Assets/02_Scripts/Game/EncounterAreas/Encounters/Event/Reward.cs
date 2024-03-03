using System;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Units.Skills.Base;
using Game.GameMechanics;
using UnityEngine.Serialization;

namespace Game.EncounterAreas.Encounters.Event
{
    [Serializable]
    public class Reward
    {
        [FormerlySerializedAs("item")] public List<ItemBP> itemBp;
        [FormerlySerializedAs("skill")] public SkillBp skillBp;
        public BlessingBP blessingBp;
        public int gold;
        public int experience;
        public int grace;
    }
}