using System.Collections.Generic;
using Game.GameActors.Units.Skills.Active;
using Game.GameActors.Units.Skills.Base;
using Game.Manager;
using Game.Utility;
using UnityEngine;

namespace Game.GameActors.Units.Skills.EffectMixins
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/SmokeScreen", fileName = "SmokeScreenSkillEffect")]
    public class SmokeScreenSkillEffectMixin : SelfTargetSkillEffectMixin
    {
        [SerializeField] private GameObject vfx;
        public override List<EffectDescription> GetEffectDescription(Unit caster, int level)
        {
            return new List<EffectDescription>()
            {
                new EffectDescription("Retreat from Battle","","")
            };
        }

        public override void Activate(Unit target, int level)
        {
            MonoUtility.DelayFunction(() => Instantiate(vfx,
                Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, .1f)),
                Quaternion.identity), 1.2f);
            MonoUtility.DelayFunction(()=>
            {
                GameSceneController.Instance.LoadEncounterAreaAfterBattle(false);
            },2.8f);
            
        }

        public override void Deactivate(Unit user, int skillLevel)
        {
            
        }
    }
}