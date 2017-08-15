using Assets.Scripts.Characters.Skills;
using Assets.Scripts.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Skills
{
    class EnemySkillState : SkillGameState
    {
        global::Character user, target;
        Skill skill;
        bool animation = false;
        public EnemySkillState(global::Character user, Skill skill, global::Character target)
        {
            this.user = user;
            this.skill = skill;
            this.target = target;
        }
        public override void enter()
        {
            EventCount = 0;
            SingleTargetSkill singleTargetSkill = (SingleTargetSkill)skill;
            user.gameObject.transform.rotation = Quaternion.AngleAxis(getRotationToPosition(user, target.gameObject.transform.position), Vector3.up);
            singleTargetSkill.Activate(user, target);
        }
        public override void exit()
        {
            GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().ActiveCharWait();
            GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().canEndTurn = true;
        }
        int EventCount = 0;
        public float getRotationToPosition(global::Character a, Vector3 b)
        {
            int xa = (int)a.gameObject.transform.localPosition.x;
            int za = (int)a.gameObject.transform.localPosition.z;
            int xb = (int)b.x;
            int zb = (int)b.z;
            int deltax = Mathf.Abs(xa - xb);
            int deltaz = Mathf.Abs(za - zb);
            float value = 0;
            if (xa > xb)
            {
                if (za > zb)
                {
                    if (deltax > deltaz)
                    {
                        value = 45 + 22.5f;
                    }
                    else if (deltax < deltaz)
                    {
                        value = 45 - 22.5f;
                    }
                    else
                    {
                        value = 225;
                    }

                }
                else if (za < zb)
                {

                    if (deltax > deltaz)
                    {
                        value = 315 - 22.5f;
                    }
                    else if (deltax < deltaz)
                    {
                        value = 315 + 22.5f;
                    }
                    else
                    {
                        value = 315;
                    }

                }
                else
                {
                    value = 270;
                }

            }
            else if (xa < xb)
            {
                if (za > zb)
                {

                    if (deltax > deltaz)
                    {


                        value = 225 - 22.5f;
                    }
                    else if (deltax < deltaz)
                    {
                        value = 225 - 22.5f;
                    }
                    else
                    {
                        value = 45;
                    }
                }
                else if (za < zb)
                {

                    if (deltax > deltaz)
                    {
                        value = 135 - 22.5f;
                    }
                    else if (deltax < deltaz)
                    {
                        value = 135 + 22.5f;
                    }
                    else
                    {
                        value = 135;
                    }
                }
                else
                {
                    value = 90;
                }

            }
            if (za > zb)
            {
                if (xa > xb)
                {
                    if (deltax > deltaz)
                    {
                        value = 225 + 22.5f;
                    }
                    else if (deltax < deltaz)
                    {
                        value = 225 - 22.5f;
                    }
                    else
                    {
                        value = 135;
                    }
                }
                else if (xa < xb)
                {

                    if (deltax > deltaz)
                    {

                        value = 135 - 22.5f;

                    }
                    else if (deltax < deltaz)
                    {
                        value = 135 + 22.5f;

                    }
                    else
                    {
                        value = 225;
                    }
                }
                else
                {
                    value = 180;
                }

            }
            else if (za < zb)
            {
                if (xa > xb)
                {
                    if (deltax > deltaz)
                    {
                        value = 315 - 22.5f;
                    }
                    else if (deltax < deltaz)
                    {
                        value = 315 + 22.5f;
                    }
                    else
                    {
                        value = 315;
                    }
                }
                else if (xa < xb)
                {

                    if (deltax > deltaz)
                    {
                        value = 45 + 22.5f;
                    }
                    else if (deltax < deltaz)
                    {
                        value = 45 - 22.5f;
                    }
                    else
                    {
                        value = 45;
                    }
                }
                else
                {
                    value = 0;
                }
            }
            return value;
        }
        public override void SkillAnimationEvent()
        {
          ((SingleTargetSkill)skill).Effect(user, target);

        }
        
        public override void update()
        {
            if (!skill.IsInAnimation())
            {
                GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().SwitchState(new GameplayState());
            }
        }
        
    }
}
