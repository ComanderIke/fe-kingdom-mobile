using Game.AI;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using Game.GameInput;
using Game.Manager;
using Game.Map;
using UnityEngine;

namespace Game.Mechanics.Commands
{
    public class UseSkillCommand : Command
    {
        private readonly Unit user;
        private readonly IAttackableTarget target;

        private Vector2Int castLocation;
        public UseSkillCommand(IBattleActor user, IAttackableTarget target, Vector2Int castLocation)
        {
            this.user = (Unit)user;
            this.target = target;
            this.castLocation = castLocation;
        }

        public override void Execute()
        {
            Debug.Log("EXECUTE USE SKILL ACTION");
            var activeSkillMixin = user.SkillManager.ActiveSkills[0].FirstActiveMixin;
            var gridSystem = ServiceProvider.Instance.GetSystem<GridSystem>();
            if (activeSkillMixin is PositionTargetSkillMixin ptsm)
            {
                Debug.Log(castLocation.x+" "+ castLocation.y);
                ptsm.Activate(user,gridSystem.Tiles, castLocation.x, castLocation.y);
            }

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