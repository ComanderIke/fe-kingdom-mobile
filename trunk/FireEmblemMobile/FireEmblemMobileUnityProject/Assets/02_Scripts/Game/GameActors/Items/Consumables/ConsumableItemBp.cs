﻿using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Skills;
using Game.WorldMapStuff.Model;

namespace Game.GameActors.Items
{
    [Serializable]
    public abstract class ConsumableItemBp:ItemBP{
        public ItemTarget target;
        public SkillBp skill;
    }
}