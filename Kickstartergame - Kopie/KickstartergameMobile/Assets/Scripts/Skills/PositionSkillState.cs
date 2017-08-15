using Assets.Scripts.Characters.Skills;
using Assets.Scripts.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Skills
{
    class PositionSkillState : SkillGameState
    {
        global::Character user;
        Vector3 target;

        Skill skill;
        public PositionSkillState(global::Character user, Skill skill, Vector3 target)
        {
            this.user = user;
            this.skill = skill;
            this.target = target;
        }
        public override void enter()
        {
            PositionTargetSkill positionTargetSkill = (PositionTargetSkill)skill;
            user.gameObject.transform.rotation = Quaternion.AngleAxis(getRotationToPosition(user, target), Vector3.up);
            positionTargetSkill.Activate(user, new Vector3(target.x + 0.5f, target.y, target.z + 0.5f));
            GameObject.Find(MainScript.CURSOR_NAME).GetComponent<CursorScript>().wallcursor = false;
            GameObject.Find(MainScript.CURSOR_NAME).GetComponent<CursorScript>().lightningcursor = false;
        }
        public override void SkillAnimationEvent()
        {

        }
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
      
        public override void exit()
        {
			GameObject.Find (MainScript.MAIN_GAME_OBJ).GetComponent<MainScript> ().ActiveCharWait ();
			GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().canEndTurn = true;
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
