using Assets.Scripts.Characters.Skills;
using Assets.Scripts.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    [System.Serializable]
    class SkillState : GameState
    {
        global::Character skillUser;
        global::Character skillTarget;
        Skill skill;
        int SelectedSkillTarget;
        Vector3 positionTarget;
        public SkillState(global::Character skillUser, Skill skill, global::Character skillTarget,Vector3 pos)
        {
            this.skill = skill;
            this.skillUser = skillUser;
            this.skillTarget = skillTarget;
            this.positionTarget = pos;
        }
        SkillGameState gameState;
        public override void enter()
        {
            if (skill.target == SkillTarget.Ally)
            {
                gameState = new AllySkillState(skillUser, skill, skillTarget, new GameplayState());
            }
            if (skill.target == SkillTarget.Enemy)
            {
                gameState= new EnemySkillState( skillUser, skill, skillTarget);
            }

            if (skill.target == SkillTarget.Position)
            {
                gameState = new PositionSkillState(skillUser, skill, positionTarget);
            }
            if (skill.target == SkillTarget.None)
            {
                gameState = new SelfSkillState(skillUser, skill, new GameplayState());
            }
            gameState.enter();
        }
        public void SkillAnimationEvent()
        {
            gameState.SkillAnimationEvent();
        }
        public override void exit()
        {
            gameState.exit();
        }

        public override void update()
        {
            gameState.update();
        }
        public void switchState(SkillGameState gameState)
        {
            gameState.exit();
            gameState.enter();
        }
    }
}
