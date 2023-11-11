using System.Collections.Generic;
using Game.Grid;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
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