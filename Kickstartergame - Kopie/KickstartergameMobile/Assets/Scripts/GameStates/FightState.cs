using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Classes;
using Assets.Scripts.Characters.Skills;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Battle;
using AssemblyCSharp;

namespace Assets.Scripts.GameStates
{
    class FightState : GameState
    {
        private const float DOUBLE_ATTACK_THRESHOLD = 1.3f;
		private const float ATTACK_BREAK = 0.4f;
        private const String ATTACK_ANIMATION = "Attack1";
        private const String ATTACK_ANIMATION2 = "Attack2";
        public const String FIGHT_PANEL = "FightPanel";
        private const String ATTACK_MISS_TEXT = "missed!";
        private const String ATTACK_CRIT_TEXT = "Crit: ";
        private global::Character activeCharacter;
        private AttackTarget defender;
        private global::Character fightCharacter;
        Vector3 cameraStartPosition;
        Quaternion cameraStartRotation;
        bool flag;
        bool flag2;
        int cnt = 0;
        bool attack2 = false;
        bool fightRotationSetup;
        bool isFightingAgainstWall = false;
        bool flag3 = false;
		bool gotExp=false;
        private GameState targetState;
        private List<Attack> attackOrderList;
        public FightState(global::Character attacker, AttackTarget defender, GameState targetState)
        {
            this.activeCharacter = attacker;
            this.targetState = targetState;
            this.defender = defender;
        }
        public void SetOtherCharsVisibility( bool isVisible)
        {
            MainScript m = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>();
            foreach(global::Character c in m.characterList)
            {
                if(c != activeCharacter && c!= defender.character)
                {
                    
                    foreach(SkinnedMeshRenderer sm in c.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
                    {
                        sm.enabled = isVisible;
                    }
                    foreach (MeshRenderer sm in c.gameObject.GetComponentsInChildren<MeshRenderer>())
                    {
                        sm.enabled = isVisible;
                    }
                }
            }
        }
        public override void enter()
        {
            activeCharacter.hasAttacked = true;
            MouseManager.active = false;
            //SetOtherCharsVisibility(false);
            cameraStartPosition = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().myCamera.transform.position;
            cameraStartRotation = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().myCamera.transform.rotation;
            GameObject.Find(MainScript.CURSOR_NAME).GetComponent<MeshRenderer>().enabled = false;
            flag = false;
            flag2 = false;
			gotExp = false;
            if (defender.character == null)
            {
                //call FightWallState or something
                isFightingAgainstWall = true;
                flag3 = false;

            }
            else
            {
                
                fightCharacter = defender.character;
                attackOrderList = new List<Attack>();
                SetUpFight();
               
            }



        }
        private void MoveCamera()
        {

            float offset = -3;
            Vector3 forward = activeCharacter.gameObject.transform.forward;
            //Vector3 targetPosition = new Vector3(activeCharacter.gameObject.transform.position.x - (forward.x* offset) , activeCharacter.gameObject.transform.position.y+1.5f, activeCharacter.gameObject.transform.position.z - (forward.z * offset));
            Vector3 centerPos = GetCenterPosition(activeCharacter, fightCharacter);
            Quaternion charrotation = activeCharacter.gameObject.transform.rotation;
            Vector3 right = activeCharacter.gameObject.transform.right;
            Vector3 targetPosition = new Vector3(centerPos.x - (right.x * offset), centerPos.y + 1.5f, centerPos.z - (right.z * offset));
            
            charrotation.SetLookRotation(-right, activeCharacter.gameObject.transform.up);
            MoveCameraTo(targetPosition, charrotation);
           // charrotation.SetAxisAngle(activeCharacter.gameObject.transform.up, Mathf.Deg2Rad*270);
            
        }
        private void MoveCameraTo(Vector3 targetPosition, Quaternion targetRotation)
        {
            float smooth = 3;
            Vector3 pos = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().myCamera.transform.position;
            Quaternion rot = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().myCamera.transform.rotation;
            
            GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().myCamera.transform.position = Vector3.Lerp(pos, targetPosition, Time.deltaTime * smooth);
            GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().myCamera.transform.rotation = Quaternion.Lerp(rot, targetRotation, Time.deltaTime * smooth);
        }
        public Vector3 GetCenterPosition(global::Character a, global::Character b)
        {
            Vector3 ret = new Vector3();
            Vector3 apos = a.gameObject.transform.position;
            Vector3 bpos = b.gameObject.transform.position;
            float deltax = (apos.x - bpos.x)/2;
            float deltay = (apos.y - bpos.y)/2;
            float deltaz = (apos.z - bpos.z)/2;
            ret = new Vector3(apos.x - deltax, apos.y - deltay, apos.z - deltaz);
            //ret = apos;
            return ret;
        }

        bool DoesAttackHit(Character attacker, Character defender)
        {
           return attacker.GetHitAgainstTarget(defender) <= UnityEngine.Random.Range(1, 101);
        }
        void SetUpCamera()
        {
            CombatCamera cc =MainScript.GetInstance().combatCamera;
            cc.target = activeCharacter.gameObject;
            cc.transform.SetParent(activeCharacter.gameObject.transform);
            cc.gameObject.SetActive(true);
           // Debug.Log(GameObject.Find("CombatCam").name);
            Debug.Log("Active");
            MainScript.GetInstance().myCamera.gameObject.SetActive(false);
            MainScript.GetInstance().FightCanvas.gameObject.SetActive(true);

        }
        void SetUpFight()
        {
            SetUpCamera();
            GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().gridScript.HideMovement();
            fightRotationSetup = true;
            int deltaPosX = (int)(fightCharacter.gameObject.transform.position.x - activeCharacter.gameObject.transform.position.x);
            int deltaPosZ = (int)(fightCharacter.gameObject.transform.position.z - activeCharacter.gameObject.transform.position.z);
            int deltaPos = Mathf.Abs(deltaPosX) + Mathf.Abs(deltaPosZ);
            //if (activeCharacter.doubleAttack)
            //    attackOrderList.Add(true);
            bool isRunAttack = false;
            bool isFollowUp = true;
            if (activeCharacter.OldPosition.x == activeCharacter.x && activeCharacter.OldPosition.y == activeCharacter.z)
            {
                Debug.Log("Normal Attack");
                Debug.Log("NormalKillAttack");
                Debug.Log("BlockNoDamage");
                Debug.Log("Unsheath Sword when not in fight");
                Debug.Log("Idle Animation with unsheathed Sword");
            }
            else
            {
                Debug.Log(activeCharacter.OldPosition);
                Debug.Log("Run Attack");
                Debug.Log("RunKillAttack");
                isRunAttack = true;
            }
            attackOrderList.Add(new Attack(activeCharacter, activeCharacter.GetDamageAgainstTarget(fightCharacter), activeCharacter.GetHitAgainstTarget(fightCharacter),isRunAttack,false,DoesAttackHit(activeCharacter, fightCharacter),activeCharacter.CanKillTarget(fightCharacter),false));
            if (fightCharacter.charclass.AttackRanges.Contains(Mathf.Abs(deltaPos)))
            {
                attackOrderList.Add(new Attack(fightCharacter, fightCharacter.GetDamageAgainstTarget(activeCharacter), fightCharacter.GetHitAgainstTarget(activeCharacter), false, true, DoesAttackHit(fightCharacter, activeCharacter), fightCharacter.CanKillTarget(activeCharacter),false));
                //if (fightCharacter.doubleAttack)
                //    attackOrderList.Add(false);
                if (fightCharacter.CanDoubleAttack(activeCharacter))
                {
                    //if (fightCharacter.doubleAttack)
                    //    attackOrderList.Add(false);
                    attackOrderList.Add(new Attack(fightCharacter, fightCharacter.GetDamageAgainstTarget(activeCharacter), fightCharacter.GetHitAgainstTarget(activeCharacter), false, true, DoesAttackHit(fightCharacter, activeCharacter), fightCharacter.CanKillTarget(activeCharacter), true));
                }
                isFollowUp = false;
                //fightCharacter.doubleAttack = false;
            }
            if (activeCharacter.CanDoubleAttack(fightCharacter))
            {
                //if (activeCharacter.doubleAttack)
                //    attackOrderList.Add(true);
                attackOrderList.Add(new Attack(activeCharacter, activeCharacter.GetDamageAgainstTarget(fightCharacter), activeCharacter.GetHitAgainstTarget(fightCharacter), false, false, DoesAttackHit(activeCharacter, fightCharacter), activeCharacter.CanKillTarget(fightCharacter), isFollowUp));
            }

            //activeCharacter.doubleAttack = false;
        }
        public void AttackAnimationEvent()
        {
            if (isFightingAgainstWall)
            {
                //AttackWall(activeCharacter, defender.wall);
            }
            else
            {
				if (cnt >= attackOrderList.Count)
					return;
                if (!attackOrderList[cnt].isCounterAttack)
                    Attack(activeCharacter, fightCharacter);
                else
                    Attack(fightCharacter, activeCharacter);
            }
        }
		public void DodgeEvent()
		{
			if (!attackOrderList [cnt].isCounterAttack) {
				int hitA = activeCharacter.GetHitAgainstTarget (fightCharacter);
				rng = UnityEngine.Random.Range (1, 101);
				if (rng > hitA) {

					fightCharacter.gameObject.GetComponent<CharacterScript> ().PlayDogueAnimation ();
				}
			} else {
				int hitA = fightCharacter.GetHitAgainstTarget(activeCharacter);
				rng = UnityEngine.Random.Range(1, 101);
				if (rng > hitA) {

					activeCharacter.gameObject.GetComponent<CharacterScript> ().PlayDogueAnimation ();
				}
			}
		}
       
        public override void exit()
        {
            MouseManager.active = true;
            SetOtherCharsVisibility(true);
            GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().ActiveCharWait();
            CombatCamera cc = MainScript.GetInstance().combatCamera;
            MainScript.GetInstance().FightCanvas.gameObject.SetActive(false);
            cc.gameObject.SetActive(false);
            MainScript.GetInstance().myCamera.gameObject.SetActive(true);
        }
        bool endfight = false;
        float cameraTime = 0;
        public override void update()
        {
            ContinueFightAnimation();
			if (endfight) {
				if (!gotExp) {
					//activeCharacter.GetExpForBattle (fightCharacter);
					//fightCharacter.GetExpForBattle (activeCharacter);
					gotExp = true;
				}
                GameObject.Find (MainScript.MAIN_GAME_OBJ).GetComponent<MainScript> ().SwitchState (targetState);
            }
            else
            {
                //MoveCamera();
            }

        }
		private int rng = 0;
        void Attack(global::Character attacker, Character defender)
        {
			cnt++;
            int hitA = attacker.GetHitAgainstTarget(defender);
            if (rng <= hitA)
            {
                rng = UnityEngine.Random.Range(1, 101);
               
                if (attacker.EquipedWeapon.weaponType.type != WeaponType.Magic)
                {
                    defender.InflictDamage(attacker.GetDamage(true), attacker);
                }
                else
                {
                    defender.inflictMagicDamage(attacker.GetDamage(true), attacker);
                }
            }
            else
            {
                FightText t = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponentInChildren<FightText>();
                Vector3 textPosition = new Vector3(defender.gameObject.transform.position.x, 1, defender.gameObject.transform.position.z);
                string text = "";
                FightTextType type;
				//defender.PlayDogueAnimation ();
                text = ATTACK_MISS_TEXT;
                type = FightTextType.Missed;
                t.setText(textPosition, text, type, attacker, defender);
            }
        }

        float getRotation(global::Character a, AttackTarget b)
        {
            int xa = (int)a.gameObject.transform.localPosition.x;
            int za = (int)a.gameObject.transform.localPosition.z;
            int xb=0;
            int zb=0;
            if (b.character != null)
            {
                xb = (int)b.character.gameObject.transform.localPosition.x;
                zb = (int)b.character.gameObject.transform.localPosition.z;
            }
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
                        value = 45;
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
                        value = 225;
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
                        value = 225;
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
                        value = 135;
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
        void ContinueFightAnimation()
        {
            if (fightRotationSetup)
            {
                fightRotationSetup = false;
                activeCharacter.rotation = getRotation(activeCharacter, defender);
                activeCharacter.gameObject.transform.rotation = Quaternion.AngleAxis(activeCharacter.rotation, Vector3.up);
                fightCharacter.rotation = activeCharacter.rotation - 180;

                if (fightCharacter.rotation < 0)
                    fightCharacter.rotation = 360 + fightCharacter.rotation;
                fightCharacter.gameObject.transform.rotation = Quaternion.AngleAxis(fightCharacter.rotation, Vector3.up);
            }
            if (cnt >= attackOrderList.Count)
            {
                if (activeCharacter.isAlive && !fightCharacter.isAlive)
                {

                }
                if (!activeCharacter.isAlive && fightCharacter.isAlive)
                {

                }
                endfight = true;
                
            }
            else if (!activeCharacter.isAlive || !fightCharacter.isAlive && !flag && !flag2)
            {
                if (activeCharacter.isAlive)
                {

                }
                if (fightCharacter.isAlive)
                {
					
                }
                endfight = true;
            }
            else if (!attackOrderList[cnt].isCounterAttack)
            {

				if (AttackAnimation(activeCharacter)&&attacktime >= ATTACK_BREAK)
                {
					flag = false;
					flag2 = false;
                    //cnt++;
                }
            }
            else
            {

				if (AttackAnimation(fightCharacter)&&attacktime >= ATTACK_BREAK)
                {
					flag = false;
					flag2 = false;
                   // cnt++;
                }
            }
        }
		private float attacktime=0;
        bool AttackAnimation(global::Character a)
        {
            CharacterScript cs = a.gameObject.GetComponentInChildren<CharacterScript>();
            if (!attack2)
            {
                if (!flag && cs.animator.GetCurrentAnimatorStateInfo(0).IsName(ATTACK_ANIMATION))
                {
                   
                    flag = true;

                    return false;
                }
            }
            else if (!flag && cs.animator.GetCurrentAnimatorStateInfo(0).IsName(ATTACK_ANIMATION2))
            {
             
                flag = true;

                return false;
            }
            if (!attack2)
            {
                if (flag && !cs.animator.GetCurrentAnimatorStateInfo(0).IsName(ATTACK_ANIMATION))
                {
                   
					attacktime += Time.deltaTime;
                    return true;
                }
            }
            else if (flag && !cs.animator.GetCurrentAnimatorStateInfo(0).IsName(ATTACK_ANIMATION2))
            {
                
                attacktime += Time.deltaTime;
                return true;
            }
            if (!flag && !flag2)
            {
                flag2 = true;
				if (a.characterClassType == CharacterClassType.Rogue) {
					int deltaPos;
					if (a == activeCharacter) {
						deltaPos = GetDeltaPos (a, fightCharacter);
					} else
						deltaPos = GetDeltaPos (activeCharacter, a);
					if (deltaPos == 1) {
						attack2 = true;
						cs.setAttacking2 ();
					} else {
						attack2 = false;
						cs.setAttacking ();
					}
				} else if (a.characterClassType == CharacterClassType.Hellebardier) {
					int x = 0;
					for (int i=0; i<=cnt; i++) {
						if(attackOrderList[cnt].isCounterAttack==attackOrderList[i].isCounterAttack)
							x++;
					}
					if (x == 2) {
						attack2 = true;
						cs.setAttacking2 ();
					} else {
						attack2 = false;
						cs.setAttacking ();
					}
				}
                else
                {
                    attack2 = false;
                    cs.setAttacking();
                }
            }
            return false;
        }
     
        private int GetDeltaPos(global::Character a, global::Character fightCharacter)
        {
            int delta;
            delta = (int)(Math.Abs(a.GetPositionOnGrid().x - fightCharacter.GetPositionOnGrid().x) + Math.Abs(a.GetPositionOnGrid().y - fightCharacter.GetPositionOnGrid().y));
            return delta;
        }
    }
}
