﻿using System;
using Game.DataAndReferences.Data;
using Game.EncounterAreas.Management;
using Game.GameActors.Player;
using Game.GameActors.Units;
using Game.Manager;
using Game.States;
using Game.Systems;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace __2___Scripts.External.Editor
{
   
    public class DebugWindow: EditorWindow
    {
        private Unit activeUnit;
        private Unit activeEnemy;
        [MenuItem("Tools/FE_Debug_Window")]
        public static void ShowMyEditor()
        {
            // This method is called when the user selects the menu item in the Editor
            EditorWindow wnd = GetWindow<DebugWindow>();
            wnd.titleContent = new GUIContent("Selected Unit Viewer");
        }
       
        public void Update()
        {
            UnitSelectionSystem system = null;
            if (GridGameManager.Instance != null)
                system = GridGameManager.Instance.GetSystem<UnitSelectionSystem>();
            if (system != null)
            {
                
                if (!Equals(activeUnit,(Unit)system.SelectedCharacter)&&system.SelectedCharacter!=null)
                {
                    activeUnit = (Unit)system.SelectedCharacter;
                    
                    Repaint();
                    //Debug.Log("Repaint!");
                } 
                if (!Equals((Unit)system.SelectedEnemy, activeEnemy) && system.SelectedEnemy != null)
                {
                    activeEnemy = (Unit)system.SelectedEnemy;
                    Repaint();
                }
            }
            else
            {
               
                if (Player.Instance != null && Player.Instance.Party != null &&
                    Player.Instance.Party.ActiveUnit != null)
                    activeUnit = Player.Instance.Party.ActiveUnit;
            }

           
        }

        private string nodeTextFieldText="0";
        public void OnGUI()
        {
            //Debug.Log("OnGUI");
            if(GUILayout.Button("Reset"))
            {
                activeUnit = null;
                activeEnemy = null;
            }
          
            string selectedUnitName = "";
            if (GUILayout.Button("Win Battle"))
            {
                GameSceneController.Instance.LoadEncounterAreaAfterBattle(true);
            }
            if (GUILayout.Button("ResultScreen"))
            {
                GridGameManager.Instance.GameStateManager.SwitchState(WinState.Create());
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Go to Node: "))
            {
                AreaGameManager.Instance.MoveClicked(Int32.Parse(nodeTextFieldText));
            }
            nodeTextFieldText=GUILayout.TextField(nodeTextFieldText);
            GUILayout.EndHorizontal();
            if (activeUnit != null)
            {
                selectedUnitName = activeUnit.name;
                if (GUILayout.Button("+1 Stone"))
                {
                    Player.Instance.Party.AddItem(GameBPData.Instance.GetSmithingStone());
                }
                if (GUILayout.Button("-1 Stone"))
                {
                    Player.Instance.Party.RemoveItem(GameBPData.Instance.GetSmithingStone());
                }
                if (GUILayout.Button("+1 DragonScale"))
                {
                    Player.Instance.Party.AddItem(GameBPData.Instance.GetDragonScale());
                }
                if (GUILayout.Button("-1 DragonScale"))
                {
                    Player.Instance.Party.RemoveItem(GameBPData.Instance.GetDragonScale());
                }
                if (GUILayout.Button("+50 Gold"))
                {
                    Player.Instance.Party.AddGold(50);
                }
                if (GUILayout.Button("-50 Gold"))
                {
                    Player.Instance.Party.AddGold(-50);
                }
                if (GUILayout.Button("+50 Supplies"))
                {
                    Player.Instance.Party.AddSupplies(50);
                }
                if (GUILayout.Button("-50 Supplies"))
                {
                    Player.Instance.Party.AddSupplies(-50);
                }
                if (GUILayout.Button("+50 Grace (Player)"))
                {
                    Player.Instance.Grace+=50;
                }
                if (GUILayout.Button("+500 Grace (Player)"))
                {
                    Player.Instance.Grace+=500;
                }
                if (GUILayout.Button("-50 Grace (Player)"))
                {
                    Player.Instance.Grace-=50;
                }
                if (GUILayout.Button("+50 Grace (Party)"))
                {
                    Player.Instance.Party.AddGrace(50);
                }
                if (GUILayout.Button("-50 Grace (Party)"))
                {
                    Player.Instance.Party.AddGrace(-50);
                }
                if (GUILayout.Button("+20 Morality"))
                {
                    Player.Instance.Party.Morality.AddMorality(20);
                }
                if (GUILayout.Button("-20 Morality"))
                {
                    Player.Instance.Party.Morality.AddMorality(-20);
                }
                if (GUILayout.Button("+30 EXP"))
                {
                    activeUnit.ExperienceManager.AddExp(30);
                }
                if (GUILayout.Button("+100 EXP"))
                {
                    activeUnit.ExperienceManager.AddExp(100);
                }
                if (GUILayout.Button("+100 EXP Party"))
                {
                    foreach(var unit in Player.Instance.Party.members)
                        unit.ExperienceManager.AddExp(100);
                }
                GUILayout.Label("Selected Unit: " + selectedUnitName);
                GUILayout.Label("Hp: " + activeUnit.Hp + "/" + activeUnit.MaxHp);
                if (activeUnit.SkillManager!=null && activeUnit.SkillManager.Skills != null)
                {
                    GUILayout.Label("SkillsCount: " + activeUnit.SkillManager.Skills.Count);
                    foreach (var skill in activeUnit.SkillManager.Skills)
                    {
                        GUILayout.Label("Skill: " + skill.Name);
                    }

                }

                if (GUILayout.Button("Lose 5 HP"))
                {
                    activeUnit.Hp -= 5;
                }
                if (GUILayout.Button("Heal 5"))
                {
                    activeUnit.Heal(5);
                }
                if (GUILayout.Button("Take 5 Damage"))
                {
                    activeUnit.InflictFixedDamage(null,5, DamageType.True);
                }
                if (GUILayout.Button("Remove Random Skill"))
                {
                    activeUnit.SkillManager.RemoveRandomSkill();
                }
                if (GUILayout.Button("Grant SoulCharge"))
                {
                    activeUnit.EquippedRelic.GetGem().IncreaseSouls();
                }
            }
            else if (activeEnemy != null)
            {
                GUILayout.Label("Selected Enemy: " + activeEnemy.name);
                GUILayout.Label("Hp: " + activeEnemy.Hp + "/" + activeEnemy.MaxHp);
                if (GUILayout.Button("Lose 5 HP"))
                {
                    activeEnemy.Hp -= 5;
                }
                if (GUILayout.Button("Heal 5"))
                {
                    activeEnemy.Heal(5);
                }
                if (GUILayout.Button("Take 5 Damage"))
                {
                    activeEnemy.InflictFixedDamage(null,5, DamageType.True);
                }
            }
            else
            {
                GUILayout.Label("No Unit Selected!");
            }
        }
    }
}