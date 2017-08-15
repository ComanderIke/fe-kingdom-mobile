using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameStates
{
    class CharacterScreenState : GameState
    {
        Character character;
        GameState targetState;
        GameObject screen;
        Text AttributePoints;
        Text SkillPoints;
        Text Mana;
        Text HP;
        Text Str;
        Text Def;
        Text Skill;
        Text Speed;
        Text Skill1;
        Text Skill2;
        Text Skill3;
        Text Skill4;

        public CharacterScreenState(Character character, GameState gameState)
        {
            this.character = character;
            this.targetState = gameState;
        }
        public override void enter()
        {
            screen = GameObject.Instantiate(GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().CharacterScreenPrefab);
            screen.transform.SetParent(GameObject.Find("Canvas").transform, false);
            GameObject.Find("Name").GetComponent<Text>().text=character.name;
            GameObject.Find("Class").GetComponent<Text>().text =""+character.characterClassType;
            HP = GameObject.Find("HPText").GetComponent<Text>();
            Mana = GameObject.Find("ManaText").GetComponent<Text>();
            Str = GameObject.Find("STRText").GetComponent<Text>();
            Skill = GameObject.Find("SKLText").GetComponent<Text>();
            Speed = GameObject.Find("SPDText").GetComponent<Text>();
            Def = GameObject.Find("DEFText").GetComponent<Text>();

        }
 

        public override void exit()
        {
            GameObject.Destroy(screen);
        }

        public override void update()
        {
            AttributePoints.text = "" + character.attributepoints;
            HP.text = "HP: " + character.stats.maxHP;
            Str.text = "Atk:" + character.stats.attack;
            Def.text = "Def: " + character.stats.defense;
            Skill.text = "Acc:" + character.stats.accuracy;
            Speed.text = "Spd:" + character.stats.speed;
            if (character.charclass.skills.Count >= 1)
                Skill1.text = character.charclass.skills[0].name+ ": Level: " + character.charclass.skills[0].Level+"\n "+character.charclass.skills[0].description;
            if (character.charclass.skills.Count >= 2)
                Skill2.text = character.charclass.skills[1].name + ": Level: " + character.charclass.skills[1].Level + "\n " + character.charclass.skills[1].description;
            if (character.charclass.skills.Count >= 3)
                Skill3.text = character.charclass.skills[2].name + ": Level: " + character.charclass.skills[2].Level + "\n " + character.charclass.skills[2].description;
            if (character.charclass.skills.Count >= 4)
                Skill4.text = character.charclass.skills[3].name + ": Level: " + character.charclass.skills[3].Level + "\n " + character.charclass.skills[3].description;
            if (Input.GetMouseButtonDown(1))
            {
                GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().SwitchState(targetState);
            }
        }
    }
}
