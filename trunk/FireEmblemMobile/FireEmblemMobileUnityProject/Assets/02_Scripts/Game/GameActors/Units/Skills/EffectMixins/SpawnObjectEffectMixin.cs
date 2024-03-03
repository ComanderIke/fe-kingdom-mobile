using System.Collections.Generic;
using Game.GameActors.Units.Skills.Active;
using Game.GameActors.Units.Skills.Base;
using Game.Grid.Tiles;
using UnityEngine;

namespace Game.GameActors.Units.Skills.EffectMixins
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/SpawnObject", fileName = "SpawnObjectSkillEffect")]
    public class SpawnObjectEffectMixin : TileTargetSkillEffectMixin
    {
        public GameObject spawnedObject;
        
        

       
        public override List<EffectDescription> GetEffectDescription(Unit caster,int level)
        {
            var list = new List<EffectDescription>();
            list.Add((new EffectDescription("Creates: ","Iceblock", "Iceblock")));
       
            return list;
        }

        public override void Activate(Tile target, int level)
        {
            var go = Instantiate(spawnedObject, target.GetTransform().position ,
                Quaternion.identity,target.GetTransform());
        }
    }
}