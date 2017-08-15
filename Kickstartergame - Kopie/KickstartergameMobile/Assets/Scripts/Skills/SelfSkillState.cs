using Assets.Scripts.Characters.Skills;
using Assets.Scripts.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Skills
{
    class SelfSkillState : SkillGameState
    {
        global::Character user;
        Skill skill;
        GameState targetState;
        public SelfSkillState(global::Character user, Skill skill, GameState targetState)
        {
            this.user = user;
            this.skill = skill;
            this.targetState = targetState;
        }
        public override void enter()
        {
            SelfTargetSkill singleTargetSkill = (SelfTargetSkill)skill;
            singleTargetSkill.Activate(user);
        }

        public override void exit()
        {
        }
        public override void SkillAnimationEvent()
        {

        }
        public override void update()
        {
            if (!skill.IsInAnimation())
            {
                GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().SwitchState(targetState);
            }
        }
    }
}
