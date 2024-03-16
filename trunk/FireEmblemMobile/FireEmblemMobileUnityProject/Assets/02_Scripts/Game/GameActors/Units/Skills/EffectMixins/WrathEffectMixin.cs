using System.Collections.Generic;
using System.Net;
using Game.AI;
using Game.AI.DecisionMaking;
using Game.GameActors.Units.Skills.Active;
using Game.GameActors.Units.Skills.Base;
using Game.Grid;
using Game.Manager;
using Game.States.Mechanics.Commands;
using Game.Systems;
using GameCamera;
using UnityEngine;

namespace Game.GameActors.Units.Skills.EffectMixins
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/Wrath", fileName = "WrathSkillEffect")]
    public class WrathEffectMixin : UnitTargetSkillEffectMixin
    {
        private UnitActionSystem unitActionSystem;
        private GridSystem gridSystem;
        private CameraSystem cameraSystem;
        private Unit target;
        private Unit performer;
        private Vector2Int attackLocation;
     
        public override void Activate(Unit performer, Unit caster, int level)
        {
            if (unitActionSystem == null)
                unitActionSystem = ServiceProvider.Instance.GetSystem<UnitActionSystem>();
            if (gridSystem == null)
                gridSystem = ServiceProvider.Instance.GetSystem<GridSystem>();
            if (cameraSystem == null)
                cameraSystem = GameObject.FindObjectOfType<CameraSystem>();
            //Block Player Input during this.
            //Change TO AI State but only with the one unit and without phase transition maybe dont change state?
            MyDebug.LogTODO("BLOCK Input do a pre playerphase State where all buffs /debuffs and start of turn skills happen.");
            performer.AIComponent.MovementOptions=gridSystem.GridLogic.GetMoveLocations(performer);
            var attackTargets = DecisionMaker.GetAttackTargets(gridSystem.GridLogic, performer);
            if (attackTargets.Count == 0)
            {
                MyDebug.LogTest("No Attack Targets for Wrath Proc");
                return;
            }
                
            var rng = Random.Range(0, attackTargets.Count);
            var randomTarget = attackTargets[rng];
            var randomAttackPos = randomTarget.AttackableTiles[Random.Range(0,randomTarget.AttackableTiles.Count)];
            MyDebug.LogTODO("Problems with Attackable Targets that are not units?");
            target = (Unit)randomTarget.Target;
            attackLocation = randomAttackPos;
            this.performer = performer;
            //Change back to Player state but units stays waiting
            unitActionSystem.AddCommand(new MoveCharacterCommand(performer, randomAttackPos));
            unitActionSystem.AddCommand(new AttackCommand(performer, target));
            unitActionSystem.AddCommand(new WaitCommand(performer));
            UnitActionSystem.OnAllCommandsFinished += UnitActionsFinished;
            cameraSystem.GetMixin<FocusCameraMixin>().SetTargets(performer.GameTransformManager.GameObject, AISystem.cameraPerformerTime);
            gridSystem.cursor.SetCurrentTile(performer.GridComponent.Tile);
            FocusCameraMixin.OnArrived += CameraOnUnit;

        }
        void UnitActionsFinished()
        {
            UnitActionSystem.OnAllCommandsFinished -= UnitActionsFinished;
            MyDebug.Log("Finished Reenable Input");
           // GridGameManager.Instance.GameStateManager.SwitchState( GridGameManager.Instance.GameStateManager.EnemyPhaseState);
        }
        void CameraOnUnit()
        {
            
            FocusCameraMixin.OnArrived -= CameraOnUnit;
            var cameraTarget = target != null
                ? target.GameTransformManager.GameObject
                : performer.GameTransformManager.GameObject;
            cameraSystem.GetMixin<FocusCameraMixin>().SetTargets(cameraTarget, AISystem.cameraTargetTime, true);
            gridSystem.cursor.SetCurrentTile(gridSystem.GetTileFromVector2(attackLocation));
            unitActionSystem.ExecuteActions();
            
        }

        public override void Deactivate(Unit user, Unit caster, int skillLevel)
        {
            
        }

        public override List<EffectDescription> GetEffectDescription(Unit caster,int level)
        {
            return new List<EffectDescription>();
            return new List<EffectDescription>()
            {
                new EffectDescription("Automatically Attacks an enemy nearby.","","")
            };
        }
        
    }
}