using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
using Assets.Scripts.Items;

namespace Assets.Scripts.GameStates
{
    public class GameplayState : GameState
    {
        MainScript mainScript;
        public Player activePlayer;
        bool attackRangesOn = false;
        int CharacterNumber = 0;

        float cameraSpeed = 3f;
        bool isAnimation = false;

        public override void enter()
        {
            mainScript = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>();
            activePlayer = MainScript.players[MainScript.ActivePlayerNumber];
        }

        public override void exit()
        {

        }

        public override void update()
        {
            CheckGameOver();
        }

        
        public void CheckGameOver()
        {
            /*Player loser = null;
            foreach (Player p in MainScript.players)
            {
                bool allCharactersDead = true;
                foreach (global::Character c in p.getCharacters())
                {
                    if (c.isAlive)
                    {
                        allCharactersDead = false;
                        break;
                    }
                }
                if (allCharactersDead)
                {
                    loser = p;
                    break;
                }
            }
            if (loser != null)
            {
                Debug.Log("GameOver!");
            }*/
        }
        
        private void RemoveDeadCharacters()
        {
            foreach (Player player in MainScript.players)
            {
                foreach (global::Character character in player.getCharacters())
                {
                    if (character.isAlive == false)
                    {
                        Vector2 pos = character.GetPositionOnGrid();
                        if (mainScript.gridScript.fields[(int)pos.x, (int)pos.y].character == character)
                            mainScript.gridScript.fields[(int)pos.x, (int)pos.y].character = null;
                    }
                }
            }

        }

        
        void StartTurn()
        {
            if (!activePlayer.isPlayerControlled)
            {
                mainScript.SwitchState(new AIState(activePlayer));
                GameObject go = GameObject.Instantiate(mainScript.AITurnAnimation, new Vector3(), Quaternion.identity)as GameObject;
                go.transform.SetParent(GameObject.Find("Canvas").transform, false);
                //go.transform.localPosition = new Vector3();
            }
            else
            {
               // mainScript.MoveCameraTo(activePlayer.getCharacters()[0].x, activePlayer.getCharacters()[0].z);
                GameObject go = GameObject.Instantiate(mainScript.PlayerTurnAnimation, new Vector3(), Quaternion.identity) as GameObject;
                go.transform.localPosition = new Vector3();
                go.transform.SetParent(GameObject.Find("Canvas").transform, false);
                
            }
            if (activePlayer.number == 1)
            {
                mainScript.turncount++;
                foreach(Character c in mainScript.characterList)
                {
                    c.UpdateOnWholeTurn();

                }
                Debug.Log("Turn: " + mainScript.turncount);
            }
            //mainScript.gridScript.fields[x, z].gameObject.GetComponent<FieldClicked>().hovered = false;
            foreach (global::Character c in activePlayer.getCharacters())
            {
                c.UpdateTurn();
                //c.gameObject.GetComponent<CharacterScript>().WaitAnimation(false);
                if (activePlayer.isPlayerControlled)
                    GameObject.Instantiate(GameObject.FindObjectOfType<UXRessources>().activeUnitField, c.gameObject.transform.position, Quaternion.identity, c.gameObject.transform);
            }
    }

        public void EndTurn()
        {
            foreach (global::Character c in activePlayer.getCharacters())
            {
                c.isWaiting = false;
                //c.gameObject.GetComponent<CharacterScript>().SetSelected(false);
            }
            MainScript.ActivePlayerNumber++;
            activePlayer = MainScript.players[MainScript.ActivePlayerNumber];
            mainScript.activeCharacter = null;
            StartTurn();
        }

        private void ShowAllEnemyAttackRanges()
        {
            mainScript.gridScript.HideMovement();//TODO
            if (!attackRangesOn)
            {
                attackRangesOn = true;
                if (mainScript.activeCharacter != null)
                {
                    mainScript.activeCharacter.Selected = false;
                    if (mainScript.activeCharacter.gameObject != null)
                        //if (mainScript.activeCharacter.gameObject.GetComponentInChildren<HighlightSelected>() != null)
                        //    mainScript.activeCharacter.gameObject.GetComponentInChildren<HighlightSelected>().Hovered = false;
                    mainScript.activeCharacter = null;
                }
                foreach (Player p in MainScript.players)
                {
                    if (p != activePlayer)
                    {
                        foreach (global::Character c in p.getCharacters())
                        {
                            mainScript.gridScript.ShowMovement(c);
                            mainScript.gridScript.ShowAttack(c, c.AttackRanges, true);
                            mainScript.gridScript.ResetActiveFields();

                        }
                    }
                }
            }
            else
            {
                attackRangesOn = false;
            }
        }

		private void SwitchPreviousCharacter()
		{
			Debug.Log ("Previous");
			cameraSpeed = 0.1f;
			if (mainScript == null)
				mainScript = GameObject.Find (MainScript.MAIN_GAME_OBJ).GetComponent<MainScript> ();
			List<LivingObject> characters =mainScript.activePlayer.getCharacters();
			for (int i = CharacterNumber; i > 0; i--)
			{
                LivingObject c = characters[i];
				if (c != mainScript.activeCharacter && c.isAlive && !c.isWaiting)
				{
					Debug.Log ("SET1");
					SetActiveCharacter(c, true);
					CharacterNumber = i;
					return;
				}
			}
			for (int i = characters.Count-1; i >= 0; i--)
			{
                LivingObject c = characters[i];
				if (c != mainScript.activeCharacter && c.isAlive && !c.isWaiting)
				{
					Debug.Log ("SET2");
					SetActiveCharacter(c, true);
					CharacterNumber = i;
					return;
				}
			}

		}
        private void SwitchCharacter()
        {
			Debug.Log ("Next");
            cameraSpeed = 0.1f;
			if (mainScript == null)
				mainScript = GameObject.Find (MainScript.MAIN_GAME_OBJ).GetComponent<MainScript> ();
			List<LivingObject> characters = mainScript.activePlayer.getCharacters();
            for (int i = CharacterNumber; i < characters.Count; i++)
            {
                LivingObject c = characters[i];
                if (c != mainScript.activeCharacter && c.isAlive && !c.isWaiting)
                {
                    SetActiveCharacter(c, true);
                    CharacterNumber = i;
                    return;
                }
            }
            for (int i = 0; i < characters.Count; i++)
            {
                LivingObject c = characters[i];
                if (c != mainScript.activeCharacter && c.isAlive && !c.isWaiting)
                {
                    SetActiveCharacter(c, true);
                    CharacterNumber = i;
                    return;
                }
            }

        }

        public  void FieldClicked(Vector2 v)
        {
            if (mainScript.gridScript.fields[(int)v.x, (int)v.y].character == null)
            {
                int x = (int)v.x;
                int y = (int)v.y;
                if (mainScript.activeCharacter != null)
                {
                    if (mainScript.gridScript.fields[x, y].isActive)
                    {
                        int sx = (int)mainScript.activeCharacter.gameObject.transform.position.x;
                        int sy = (int)mainScript.activeCharacter.gameObject.transform.position.y;
                        int tx = (int)x;
                        int ty = (int)y;
                        List<Vector3> movePath = new List<Vector3>();
                        for (int i = 0; i < MouseManager.oldMousePath.Count; i++)
                        {
                            movePath.Add(new Vector2(MouseManager.oldMousePath[i].x, MouseManager.oldMousePath[i].y));
                        }
                        MoveCharacter(mainScript.activeCharacter, x,y,movePath,false, new GameplayState());
                        mainScript.gridScript.HideMovement();
                        return;
                    }
                    else
                    { // clicked on Field where he cant walk to
                        //Do nothing
                    }
                }
            }
            
        }
        Character fightCharacter;
        void Fight()
        {

            Debug.Log(mainScript.activeCharacter.name);
            mainScript.SwitchState(new FightState(mainScript.activeCharacter, fightCharacter, new GameplayState()));
            MainScript.endOfMoveCharacterEvent -= Fight;
        }
        public void GoToEnemy(LivingObject character, LivingObject enemy, bool drag)
        {
            MouseManager.ResetMoveArrow();
            
            if (MouseManager.oldMousePath.Count==0&&character.AttackRanges.Contains<int>((int)(Mathf.Abs(enemy.GetPositionOnGrid().x - character.GetPositionOnGrid().x) + Mathf.Abs(enemy.GetPositionOnGrid().y - character.GetPositionOnGrid().y))))
            {
                mainScript.gridScript.HideMovement();
                Debug.Log("Enemy is in Range:");
                mainScript.oldPosition = new Vector2(character.GetPositionOnGrid().x, character.GetPositionOnGrid().y);
                Debug.Log(mainScript.oldPosition);
                mainScript.activeCharacter.OldPosition = new Vector2(mainScript.activeCharacter.x, mainScript.activeCharacter.y);
                if (!drag)
                {
                    mainScript.SwitchState(new FightState(character, enemy, new GameplayState()));

                }
                else
                {
                    mainScript.SwitchState(new FightState(character, enemy, new GameplayState()));
                }
                return;
            }
            else//go to enemy cause not in range
            {
                Debug.Log("Got to Enemy!");
                if (mainScript.gridScript.IsFieldAttackable(enemy.x, enemy.y))
                {
                    Debug.Log("Field Attackable");
                    mainScript.gridScript.HideMovement();
                    int sx = (int)character.gameObject.transform.position.x;
                    int sy = (int)character.gameObject.transform.position.y;
                    int tx = (int)enemy.gameObject.transform.position.x;
                    int ty = (int)enemy.gameObject.transform.position.y;

 
                    List<Vector3> movePath = new List<Vector3>();
                    for (int i = 0; i < MouseManager.oldMousePath.Count; i++)
                    {
                        movePath.Add(new Vector2(MouseManager.oldMousePath[i].x, MouseManager.oldMousePath[i].y));
                        Debug.Log(movePath[i]);
                    }
                    if (drag)
                    {
                        MoveCharacter(character,0,0, movePath, false, new FightState(character, enemy, new GameplayState()));
                    }
                    else
                    {
                        MoveCharacter(character,0,0, movePath, false, new FightState(character, enemy, new GameplayState()));

                    }
                    mainScript.AttackRangeFromPath = 0;

                    return;
                }
                else
                {
                    return;
                }
            }
        }

        public void SetActiveCharacter(LivingObject c, bool switchChar)//TODO will be called by CHaracterClicked
        {     
            if (!switchChar && mainScript.activeCharacter != null && mainScript.activeCharacter.gameObject != null && c != mainScript.activeCharacter)
            {

                if (c.team != mainScript.activeCharacter.team)//Clicked On Enemy
                {
                    //Enemy already in Range
                    if (MouseManager.confirmClick && MouseManager.clickedField == new Vector2(c.x,c.y))
                        GoToEnemy(mainScript.activeCharacter, c, false);
                    else
                    {
                        MouseManager.confirmClick = true;
                        MouseManager.clickedField = new Vector2(c.x, c.y);
                        MouseManager.CalculateMousePathToPositon(mainScript.activeCharacter, new Vector2(c.x, c.y));
                        MouseManager.DrawMousePath((int)c.x, (int)c.y);
                    }
                   
                    return;
                }
            }
            if (!isAnimation)
            {
                if (mainScript.activePlayer.getCharacters().Contains(c))
                {
                    if (!c.isWaiting)
                    {
                        if (!switchChar && mainScript.activeCharacter != null && mainScript.activeCharacter == c)
                        {
                            SameCharacterSelected(c);
                        }
                        else
                        {
                            mainScript.gridScript.HideMovement();
                            MouseManager.confirmClick = false;
                            MouseManager.clickedField = new Vector2(-1, -1);
                            SelectCharacter(c);
                        }
                    }
                    else if (c.hasMoved && !c.hasAttacked)
                    {
                        mainScript.gridScript.HideMovement();
                        mainScript.activeCharacter = c;
                        mainScript.ShowAttackRange(c);
                    }
                }
                else
                {
                    mainScript.activeCharacter = null;
                    Debug.Log("Enemy Selectd!");
                    EnemySelected(c);
                }
            }
        }

        void EnemySelected(LivingObject c)
        {
            GridScript gridScript = mainScript.gridScript;

            if (c.IsAttackRangeShown())
            {
                c.SetAttackRangeShown(false);
            }
            else
            {
                c.SetAttackRangeShown(true);
                gridScript.ShowMovement(c); 
                gridScript.ShowAttack(c, new List<int>(c.AttackRanges), false);
                gridScript.ResetActiveFields();
                if (mainScript.activeCharacter != null)
                {
                    mainScript.activeCharacter.Selected = false;
                    if (mainScript.activeCharacter.gameObject != null)
                        mainScript.activeCharacter = null;
                }
            }
        }

        void SameCharacterSelected(LivingObject c)
        {
            mainScript.oldPosition = new Vector2(mainScript.activeCharacter.gameObject.transform.localPosition.x, mainScript.activeCharacter.gameObject.transform.localPosition.y);
			GridScript s = mainScript.GetComponentInChildren<GridScript>();
            UXRessources ux = GameObject.FindObjectOfType<UXRessources>();
            Debug.Log("Same Selected");
            if (c.gameObject.GetComponentInChildren<ActiveUnitEffect>() != null) 
                GameObject.Instantiate(ux.activeUnitField, c.gameObject.transform.position, Quaternion.identity, c.gameObject.transform);
        }

        void SelectCharacter(LivingObject c)
        {
            mainScript.uiController.HideBottomUI();
            mainScript.uiController.ShowTopUI(c);
            Debug.Log("Select " + c.name);
			if (mainScript.activeCharacter != null) {
				mainScript.activeCharacter.Selected = false;
			}
            mainScript.activeCharacter = c;
            c.Selected = true;
            GridScript s = mainScript.gridScript;
            s.HideMovement();
            s.ShowMovement(c);
            s.ShowAttack(c, new List<int>(c.AttackRanges), false);
            mainScript.activeCharacter.hovered = true;

        }

        bool IsAdjacenFieldActive(MapField f)
        {
            int x = f.x;
            int y = f.y;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int xi = x + i;
                    int yj = y + j;

                    if (i == 0 && j == 0) continue;
                    if (i != 0 && j != 0) continue;
                    if ((xi < 0) || (yj < 0) || (xi >= mainScript.gridScript.grid.width) || (yj >= mainScript.gridScript.grid.height)) continue;

                    if (mainScript.gridScript.GetField(new Vector2(xi, yj)).isActive)
                        return true;
                }
            }
            return false;
        }

       public void MoveCharacter(LivingObject c, int x, int y, bool drag, GameState targetState)
        {
            mainScript.SwitchState(new MovementState(mainScript, c, x, y,drag, targetState));
        }
        public void MoveCharacter(LivingObject c, int x,int y,List<Vector3>path, bool drag, GameState targetState)
        {
            mainScript.SwitchState(new MovementState(mainScript, c,x,y,path, drag, targetState));
        }
    }
}
