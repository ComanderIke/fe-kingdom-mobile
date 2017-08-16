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

        public GameplayState()
        {
           

			//MouseWheelInput.mouseWheelDown +=SwitchCharacter;
        }
        public override void enter()
        {
            //MainScript.moveCharacterEvent += MoveActiveCharacter;
            mainScript = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>();
            //GameObject.FindObjectOfType<HudScript>().SetEndTurnButton(true);
            activePlayer = MainScript.players[MainScript.ActivePlayerNumber];
        }

        public override void exit()
        {
            //GameObject.FindObjectOfType<HudScript>().SetEndTurnButton(false);
            //global::Character c = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().activeCharacter;
            //if(c!= null)
            //GameObject.Find(MainScript.CURSOR_NAME).GetComponent<CursorScript>().SetPosition(c.GetPositionOnGrid().x, GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().gridScript.fields[(int)c.gameObject.transform.localPosition.x, (int)c.gameObject.transform.localPosition.z].height + MainScript.CURSOROFFSET, c.GetPositionOnGrid().y);
        }

        public override void update()
        {
            CheckGameOver();
        }

        
        public void CheckGameOver()
        {
            Player loser = null;
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
                //mainScript.SwitchState(new GameOverState(loser,mainScript.gameOverScreen));
                //Application.LoadLevel("Levelauswahl");
            }
        }
        /*
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
            Vector3 firstCharacterPosition = activePlayer.getCharacters()[0].gameObject.transform.position;
            GameObject cursor = GameObject.Find("Cursor");
            cursor.GetComponent<CursorScript>().SetPosition(firstCharacterPosition.x, firstCharacterPosition.y, firstCharacterPosition.z);
            CursorPositionChanged(cursor);
            int x = (int)(cursor.transform.localPosition.x - 0.5f);
            int z = (int)(cursor.transform.localPosition.z - 0.5f);
            //mainScript.gridScript.fields[x, z].gameObject.GetComponent<FieldClicked>().hovered = false;
            foreach (global::Character c in activePlayer.getCharacters())
            {
                c.UpdateTurn();
                c.gameObject.GetComponent<CharacterScript>().WaitAnimation(false);
                if (activePlayer.isPlayerControlled)
                    GameObject.Instantiate(GameObject.FindObjectOfType<UXRessources>().activeUnitField, c.gameObject.transform.position, Quaternion.identity, c.gameObject.transform);
            }
    }

        public void EndTurn()
        {
            foreach (global::Character c in activePlayer.getCharacters())
            {
                c.IsWaiting = false;
                c.gameObject.GetComponent<CharacterScript>().SetSelected(false);
            }
            foreach (MapField f in mainScript.gridScript.fields)
            {
                    f.effect.Effect(f.character, activePlayer.number);
            }
            MainScript.ActivePlayerNumber++;
            activePlayer = MainScript.players[MainScript.ActivePlayerNumber];
            mainScript.activeCharacter = null;
            StartTurn();

        }

        public void CursorPositionChanged(GameObject cursor)
        {

            clampOnGrid(cursor);
            int x = (int)(cursor.transform.localPosition.x - 0.5f);
            int z = (int)(cursor.transform.localPosition.z - 0.5f);
            cursor.transform.localPosition = new Vector3(cursor.transform.localPosition.x, mainScript.gridScript.fields[x, z].height + MainScript.CURSOROFFSET, cursor.transform.localPosition.z);
            resetHoverOnCharacters();
            if (mainScript.gridScript.fields[x, z].character != null)
            {
                mainScript.gridScript.fields[x, z].character.hovered = true;
                //if (mainScript.activeCharacter != mainScript.gridScript.fields[x, z].character)
                    //mainScript.gridScript.fields[x, z].character.gameObject.GetComponentInChildren<HighlightSelected>().Hovered = true;
            }
            //mainScript.gridScript.fields[x, z].gameObject.GetComponent<FieldClicked>().hovered = true;
        }

        private void resetHoverOnCharacters()
        {
            foreach (Player p in MainScript.players)
            {
                foreach (global::Character c in p.getCharacters())
                {
                    c.hovered = false;
                    //if (c.gameObject != null)//TODO
                    //    c.gameObject.GetComponentInChildren<HighlightSelected>().Hovered = false;
                }
            }
        }


        private void clampOnGrid(GameObject go)
        {
            if (go.transform.localPosition.x - 0.5f >= mainScript.gridScript.grid.width)
                go.transform.localPosition = new Vector3(mainScript.gridScript.grid.width - 0.5f, go.transform.localPosition.y, go.transform.localPosition.z);
            if (go.transform.localPosition.x - 0.5f < 0f)
                go.transform.localPosition = new Vector3(0.5f, go.transform.localPosition.y, go.transform.localPosition.z);
            if (go.transform.localPosition.z - 0.5f >= mainScript.gridScript.grid.height)
                go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y, mainScript.gridScript.grid.height - 0.5f);
            if (go.transform.localPosition.z - 0.5f < 0f)
                go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y, 0.5f);
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
                            mainScript.gridScript.ShowMovement((int)c.GetPositionOnGrid().x, (int)c.GetPositionOnGrid().y, c.charclass.movRange, 0, c.charclass.AttackRanges, 0, c.team, true);
                            mainScript.gridScript.ShowAttack(c, c.charclass.AttackRanges, true);
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

        public void Back()
        {
            Debug.Log("BACK");
            MouseManager.ResetMoveArrow();
            GameObject.Find(MainScript.CURSOR_NAME).GetComponent<CursorScript>().lightningcursor = false;
            GameObject.Find(MainScript.CURSOR_NAME).GetComponent<CursorScript>().wallcursor = false;
            if (mainScript.activeCharacter != null)
            {
                mainScript.gridScript.HideMovement();
                mainScript.activeCharacter.Selected = false;
                mainScript.activeCharacter = null;
            }

        }

		private void SwitchPreviousCharacter()
		{

			Debug.Log ("Previous");
			cameraSpeed = 0.1f;
			if (mainScript == null)
				mainScript = GameObject.Find (MainScript.MAIN_GAME_OBJ).GetComponent<MainScript> ();
			List<global::Character> characters =mainScript.activePlayer.getCharacters();
			for (int i = CharacterNumber; i > 0; i--)
			{
				global::Character c = characters[i];
				if (c != mainScript.activeCharacter && c.isAlive && !c.IsWaiting)
				{
					Debug.Log ("SET1");
					SetActiveCharacter(c, true);
					mainScript.clickedCharacter = c;
					CharacterNumber = i;
					return;
				}
			}
			for (int i = characters.Count-1; i >= 0; i--)
			{
				global::Character c = characters[i];
				if (c != mainScript.activeCharacter && c.isAlive && !c.IsWaiting)
				{
					Debug.Log ("SET2");
					SetActiveCharacter(c, true);
					mainScript.clickedCharacter = c;
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
			List<global::Character> characters = mainScript.activePlayer.getCharacters();
            for (int i = CharacterNumber; i < characters.Count; i++)
            {
                global::Character c = characters[i];
                if (c != mainScript.activeCharacter && c.isAlive && !c.IsWaiting)
                {
					Debug.Log ("SET3");
                    SetActiveCharacter(c, true);
					mainScript.clickedCharacter = c;
                    CharacterNumber = i;
                    return;
                }
            }
            for (int i = 0; i < characters.Count; i++)
            {
                global::Character c = characters[i];
                if (c != mainScript.activeCharacter && c.isAlive && !c.IsWaiting)
                {
					Debug.Log ("SET4");
                    SetActiveCharacter(c, true);
					mainScript.clickedCharacter = c;
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
                int z = (int)v.y;
                if (mainScript.activeCharacter != null)
                {

                    if (mainScript.gridScript.fields[x, z].isActive)
                    {
                        int sx = (int)mainScript.activeCharacter.gameObject.transform.position.x;
                        int sy = (int)mainScript.activeCharacter.gameObject.transform.position.z;
                        int tx = (int)x;
                        int ty = (int)z;
                       // List<int> range = new List<int>();
                       // range.Add(1);
                        //Debug.Log(mainScript.activeCharacter.team + " " + mainScript.gridScript.GetField(c.GetPositionOnGrid()).character.team);
                      //  MovementPath path = mainScript.gridScript.getPath(sx, sy, tx, ty, mainScript.activeCharacter.team, mainScript.activeCharacter.CanPassThrough(), false, range);//last step not possible cause enemy?!
                        List<Vector3> movePath = new List<Vector3>();
                        for (int i = 0; i < MouseManager.oldMousePath.Count; i++)
                        {
                            movePath.Add(new Vector3(MouseManager.oldMousePath[i].x, mainScript.gridScript.GetHeight((int)MouseManager.oldMousePath[i].x, (int)MouseManager.oldMousePath[i].y), MouseManager.oldMousePath[i].y));
                        }
                        //for (int i = 0; i < path.getLength(); i++)
                        //{
                        //    Debug.Log(path.getStep(i).getX() + " " + path.getStep(i).getZ());
                        //}
                        //Debug.Log(path.getLength() + " " + mainScript.activeCharacter.charclass.movRange);//+ATTACKRANGE?

                        MoveCharacter(mainScript.activeCharacter, movePath,false, new GameplayState());
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
            mainScript.SwitchState(new FightState(mainScript.activeCharacter, new AttackTarget(fightCharacter), new GameplayState()));
            MainScript.endOfMoveCharacterEvent -= Fight;
        }
        public void GoToEnemy(Character character, Character enemy, bool drag)
        {
            MouseManager.ResetMoveArrow();
            
            if (MouseManager.oldMousePath.Count==0&&character.charclass.AttackRanges.Contains<int>((int)(Mathf.Abs(enemy.GetPositionOnGrid().x - character.GetPositionOnGrid().x) + Mathf.Abs(enemy.GetPositionOnGrid().y - character.GetPositionOnGrid().y))))
            {
                mainScript.gridScript.HideMovement();
                Debug.Log("Enemy is in Range:");
                mainScript.oldPosition = new Vector2(character.GetPositionOnGrid().x, character.GetPositionOnGrid().y);
                Debug.Log(mainScript.oldPosition);
                //mainScript.gridScript.HideMovement();
                //mainScript.SwitchState(new ActionMenueState("attack", c)); //mainScript.activeCharacter, mainScript.GetTradeMenuePartners(), mainScript.GetAttackTargets(mainScript.activeCharacter), mainScript.skillPositionTargets, mainScript.jumpPositionTargets, mainScript.GetNearbyChest(), mainScript.GetNearbyDoorSwitch()));
                mainScript.activeCharacter.OldPosition = new Vector2(mainScript.activeCharacter.x, mainScript.activeCharacter.z);
                if (!drag)
                {
                   // if (GameObject.FindObjectOfType<AttackPreview>().visible && GameObject.FindObjectOfType<AttackPreview>().defender == enemy)
                  //  {
                        GameObject.FindObjectOfType<AttackPreview>().Hide();
                        mainScript.SwitchState(new FightState(character, new AttackTarget(enemy), new GameplayState()));
                   // }
                   // else
                   // {
                    //    mainScript.RotateCharacterTo(character, enemy);
                   //     GameObject.FindObjectOfType<AttackPreview>().Show(character, enemy);
                    //}
                }
                else
                {
                    mainScript.RotateCharacterTo(character, enemy);
                    GameObject.FindObjectOfType<AttackPreview>().Hide();
                    mainScript.SwitchState(new FightState(character, new AttackTarget(enemy), new GameplayState()));
                }
                return;
            }
            else//go to enemy cause not in range
            {
                Debug.Log("Got to Enemy!");
                if (mainScript.gridScript.IsFieldAttackable(enemy.x, enemy.z))
                {
                    Debug.Log("Field Attackable");
                    mainScript.gridScript.HideMovement();
                    int sx = (int)character.gameObject.transform.position.x;
                    int sy = (int)character.gameObject.transform.position.z;
                    int tx = (int)enemy.gameObject.transform.position.x;
                    int ty = (int)enemy.gameObject.transform.position.z;
                    //List<int> attackranges = character.charclass.AttackRanges;
                    //MovementPath path = mainScript.gridScript.getPath(sx, sy, tx, ty, character.team, character.CanPassThrough(), true, attackranges);

 
                    List<Vector3> movePath = new List<Vector3>();
                    for (int i = 0; i < MouseManager.oldMousePath.Count; i++)
                    {
                        movePath.Add(new Vector3(MouseManager.oldMousePath[i].x,mainScript.gridScript.GetHeight((int)MouseManager.oldMousePath[i].x, (int)MouseManager.oldMousePath[i].y), MouseManager.oldMousePath[i].y));
                        Debug.Log(movePath[i]);
                    }
                    //if (moveP.getLength() - 2 <= character.charclass.movRange + (character.GetMaxAttackRange() - 1))
                    //{
                    
                        Debug.Log("In Range");
                        if (drag)
                        {
                            GameObject.FindObjectOfType<AttackPreview>().Hide();
                            MoveCharacter(character, movePath, false, new FightState(character, new AttackTarget(enemy), new GameplayState()));
                        }
                        else
                        {
                            GameObject.FindObjectOfType<AttackPreview>().Hide();
                            MoveCharacter(character, movePath, false, new FightState(character, new AttackTarget(enemy), new GameplayState()));

                        }
                        mainScript.AttackRangeFromPath = 0;
                   // }
                   
                    if (!drag)
                    { 
                        RotateCharacterA = mainScript.activeCharacter;
                        RotateCharacterB = enemy;
                        MainScript.endOfMoveCharacterEvent += Rotate;

                    }
                    return;
                }
                else
                {
                    return;
                }
            }
            }
        public void SetActiveCharacter(global::Character c, bool switchChar)//TODO will be called by CHaracterClicked
        {
            if (MyInputManager.isLevelUpState)
            	return;
            
            if (!switchChar && mainScript.activeCharacter != null && mainScript.activeCharacter.gameObject != null && c != mainScript.activeCharacter)
            {

                if (c.team != mainScript.activeCharacter.team)//Clicked On Enemy
                {
                    //Enemy already in Range
                    GoToEnemy(mainScript.activeCharacter, c,false);
                    return;
                }
            }
            if (!isAnimation)
            {
                if (mainScript.activePlayer.getCharacters().Contains(c))
                {
                    if (!c.IsWaiting)
                    {
                        if (!switchChar && mainScript.activeCharacter != null && mainScript.activeCharacter == c)
                        {
                            SameCharacterSelected(c);
                        }
                        else
                        {
                            mainScript.gridScript.HideMovement();
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
                    mainScript.gridScript.HideAllyMovement();
                    Debug.Log("Enemy Selectd!");
                    EnemySelected(c);
                }
            }
        }
        Character RotateCharacterA;
        Character RotateCharacterB;
        void Rotate()
        {
            MainScript.endOfMoveCharacterEvent -= Rotate;
            mainScript.RotateCharacterTo(RotateCharacterA, RotateCharacterB);
            //GameObject.FindObjectOfType<AttackPreview>().Show(RotateCharacterA, RotateCharacterB);
        }
        void EnemySelected(global::Character c)
        {
            GridScript s = mainScript.GetComponentInChildren<GridScript>();

            if (c.IsAttackRangeShown())
            {
                s.HideCharacterMovement(c);
                c.SetAttackRangeShown(false);
            }
            else
            {

                s.HideAllyMovement();
                c.SetAttackRangeShown(true);
                s.ShowMovement((int)c.gameObject.transform.localPosition.x, (int)c.gameObject.transform.localPosition.z, c.charclass.movRange, c.charclass.movRange, new List<int>(c.charclass.AttackRanges), 0, c.team, false);
                s.ShowAttack(c, new List<int>(c.charclass.AttackRanges), false);
                s.ResetActiveFields();
                if (mainScript.activeCharacter != null)
                {
                    mainScript.activeCharacter.Selected = false;
                    if (mainScript.activeCharacter.gameObject != null)
                        //if (mainScript.activeCharacter.gameObject.GetComponentInChildren<HighlightSelected>() != null)//TODO
                        //    mainScript.activeCharacter.gameObject.GetComponentInChildren<HighlightSelected>().Hovered = false;
                    mainScript.activeCharacter = null;
                }
            }
        }

        void SameCharacterSelected(global::Character c)
        {
            mainScript.oldPosition = new Vector2(mainScript.activeCharacter.gameObject.transform.localPosition.x, mainScript.activeCharacter.gameObject.transform.localPosition.z);
			GridScript s = mainScript.GetComponentInChildren<GridScript>();
            s.HideAllyMovement();
            UXRessources ux = GameObject.FindObjectOfType<UXRessources>();
            Debug.Log("Same Selected");
            if (c.gameObject.GetComponentInChildren<ActiveUnitEffect>() != null) 
                GameObject.Instantiate(ux.activeUnitField, c.gameObject.transform.position, Quaternion.identity, c.gameObject.transform);
        }

        void SelectCharacter(global::Character c)
        {
            Debug.Log("Select " + c.name);
			if (mainScript.activeCharacter != null) {
				mainScript.activeCharacter.Selected = false;
				Debug.Log ("SetSelectedFalse" + mainScript.activeCharacter.name);
			}
            mainScript.activeCharacter = c;
            c.Selected = true;
            mainScript.faceSprite.sprite = c.activeSpriteObject;
            GridScript s = mainScript.GetComponentInChildren<GridScript>();
            s.HideMovement();
            s.ShowMovement((int)c.gameObject.transform.localPosition.x, (int)c.gameObject.transform.localPosition.z, c.charclass.movRange, 0, new List<int>(c.charclass.AttackRanges), 0, c.team, false);
            s.ShowAttack(c, new List<int>(c.charclass.AttackRanges), false);
           
            //GameObject.Find(MainScript.CURSOR_NAME).GetComponent<CursorScript>().SetPosition(c.gameObject.transform.localPosition.x + 0.5f, mainScript.gridScript.fields[(int)c.gameObject.transform.localPosition.x, (int)c.gameObject.transform.localPosition.z].height + MainScript.CURSOROFFSET, c.gameObject.transform.localPosition.z + 0.5f);
            resetHoverOnCharacters();
            mainScript.activeCharacter.hovered = true;
            //mainScript.activeCharacter.gameObject.GetComponentInChildren<HighlightSelected>().Selected(true);
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

       public void MoveCharacter(Character c, int x, int z, bool drag, GameState targetState)
        {
            mainScript.SwitchState(new MovementState(mainScript, c, x, z,drag, targetState));
        }
        public void MoveCharacter(Character c, List<Vector3>path, bool drag, GameState targetState)
        {
            mainScript.SwitchState(new MovementState(mainScript, c,path, drag, targetState));
        }
        private void SwitchCameraAngle()
        {
            //GameObject cam = GameObject.Find("Main Camera");
            //CameraDegrees -= 10;
            //if (CameraDegrees < 45)
            //{
            //    CameraDegrees = 90;
            //    cam.transform.RotateAround(cam.transform.position, new Vector3(1, 0, 0), -310f);
            //}
            //else
            //    cam.transform.RotateAround(cam.transform.position, new Vector3(1, 0, 0), -10f);

        }

        private void SwitchCameraHeight()
        {
            //GameObject cam = GameObject.Find("Main Camera");
            //CameraHeight += 1;
            //if (CameraHeight > 15)
            //{
            //    CameraHeight = 6;

            //}
            //cam.transform.position = new Vector3(cam.transform.position.x, CameraHeight, cam.transform.position.z);
        }
*/
    }
}
