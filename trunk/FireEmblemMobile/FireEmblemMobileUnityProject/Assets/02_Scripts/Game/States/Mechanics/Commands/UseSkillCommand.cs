using System;
using Game.GameActors.InteractableGridObjects;
using Game.GameActors.Units;
using Game.GameActors.Units.Interfaces;
using Game.GameActors.Units.Skills.Active;
using Game.Grid;
using Game.Manager;
using UnityEngine;

namespace Game.States.Mechanics.Commands
{
    public class UseSkillCommand : Command
    {
        private readonly IAIAgent user;
        private readonly IAttackableTarget target;
        public static event Action<Unit> OnUseSkill;

        private Vector2Int castLocation;
        public UseSkillCommand(IAIAgent user, IAttackableTarget target, Vector2Int castLocation)
        {
            this.user = (Unit)user;
            this.target = target;
            this.castLocation = castLocation;
        }

        public override void Execute()
        {
            Debug.Log("EXECUTE USE SKILL ACTION");
            var skillToUse = user.AIComponent.AIBehaviour.GetSkillToUse();
            var activeSkillMixin = skillToUse.FirstActiveMixin;
            var gridSystem = ServiceProvider.Instance.GetSystem<GridSystem>();
            if (activeSkillMixin is PositionTargetSkillMixin ptsm)
            {
                Debug.Log(castLocation.x+" "+ castLocation.y);
                ptsm.Activate((Unit)user,gridSystem.Tiles, castLocation.x, castLocation.y);
            }
            else if (activeSkillMixin is SelfTargetSkillMixin stsm)
            {
                Debug.Log(castLocation.x+" "+ castLocation.y);
                stsm.Activate((Unit)user);
            }
            
            OnUseSkill?.Invoke((Unit)user);
            user.AIComponent.AIBehaviour.UsedSkill(skillToUse);
            IsFinished = true;
         //   MonoUtility.DelayFunction(()=>IsFinished=true, 2f);
          //  IsFinished = true;
            // GridGameManager.Instance.GameStateManager.BattleState.Start(user, target);
            // BattleSystem.OnBattleFinished -= BattleFinished;
            // BattleSystem.OnBattleFinished += BattleFinished;

        }

        // void BattleFinished(AttackResult result)
        // {
        //     BattleSystem.OnBattleFinished -= BattleFinished;
        //     IsFinished = true;
        // }
        public override void Undo()
        {
            Debug.LogWarning("Undo Skill should not be undoable!");
        }

        public override void Update()
        {
            // if ( GridGameManager.Instance.GameStateManager.BattleState.IsFinished)
            // {
            //     Debug.Log("BattleFinished");
            //     IsFinished = true;
            // }
        }
    }
}