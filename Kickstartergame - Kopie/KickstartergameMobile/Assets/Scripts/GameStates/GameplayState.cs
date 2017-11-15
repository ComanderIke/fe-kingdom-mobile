
using Assets.Scripts.Characters;
using Assets.Scripts.Players;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    public class GameplayState : GameState
    {
        MainScript mainScript;
        public UnitSelectionManager UnitSelectionManager { get; set; }
        public UnitActionManager UnitActionManager { get; set; }

        public GameplayState()
        {
            UnitSelectionManager = new UnitSelectionManager();
            UnitActionManager = new UnitActionManager();
            mainScript = MainScript.GetInstance();
        }

        public override void enter()
        {
            InitCharacters();
        }

        public override void update()
        {
            CheckGameOver();
        }

        public override void exit()
        {

        }

        private void InitCharacters()
        {
            TurnManager turnManager = mainScript.GetSystem<TurnManager>();
            Player p = turnManager.Players[0];
            LivingObject filler = null;
            LivingObject filler2 = null;
            LivingObject filler3 = null;
            LivingObject filler4 = null;
            filler = new Human("Leila");
            filler2 = new Human("Flora");
            filler3 = new Human("Eldric");
            filler4 = new Human("Hector");
            SpriteScript ss = GameObject.FindObjectOfType<SpriteScript>();
            filler.Sprite = ss.swordActiveSprite;
            filler2.Sprite = ss.axeActiveSprite;
            filler3.Sprite = ss.archerActiveSprite;
            filler4.Sprite = ss.lancerActiveSprite;

            p.AddUnit(filler);
            p.AddUnit(filler2);
            p.AddUnit(filler3);
            p.AddUnit(filler4);
            StartPosition[] startPositions = GameObject.FindObjectsOfType<StartPosition>();
            UnitInstantiater cc = GameObject.FindObjectOfType<UnitInstantiater>();
            cc.PlaceCharacter(0, filler, startPositions[0].GetXOnGrid(), startPositions[0].GetYOnGrid());
            cc.PlaceCharacter(0, filler2, startPositions[1].GetXOnGrid(), startPositions[1].GetYOnGrid());
            cc.PlaceCharacter(0, filler3, startPositions[2].GetXOnGrid(), startPositions[2].GetYOnGrid());
            cc.PlaceCharacter(0, filler4, startPositions[3].GetXOnGrid(), startPositions[3].GetYOnGrid());
            Monster monster = new Monster("Mammoth", MonsterType.Mammoth);
            turnManager.Players[1].AddUnit(monster);

            cc.PlaceCharacter(1, monster, 3, 3);
        }

        public void CheckGameOver()
        {
            
        }

        

    }
}
