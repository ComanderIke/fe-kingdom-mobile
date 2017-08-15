using Assets.Scripts.Characters.Skills;
using Assets.Scripts.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Skills
{
    class AllySkillState : SkillGameState
    {
        global::Character user, target;
        Skill skill;
        GameState targetState;
        public AllySkillState(global::Character user, Skill skill, global::Character target, GameState targetState)
        {
            this.user = user;
            this.skill = skill;
            this.target = target;
            this.targetState = targetState;
        }
        public override void enter()
        {
            SingleTargetSkill singleTargetSkill = (SingleTargetSkill)skill;
            singleTargetSkill.Activate(user, target);
        }

        public override void exit()
        {
            GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().ActiveCharWait();
            GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().canEndTurn = true;
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
