using System;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.Manager;
using Game.Mechanics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace __2___Scripts.External.Editor
{
   
    public class DebugWindow: EditorWindow
    {
        private Unit activeUnit;
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
                
                if (activeUnit != (Unit)system.SelectedCharacter)
                {
                    activeUnit = (Unit)system.SelectedCharacter;
                    
                    Repaint();
                    //Debug.Log("Repaint!");
                }
            }
            else
            {
               
                if (Player.Instance != null && Player.Instance.Party != null &&
                    Player.Instance.Party.ActiveUnit != null)
                    activeUnit = Player.Instance.Party.ActiveUnit;
            }

           
        }

        public void OnGUI()
        {
            //Debug.Log("OnGUI");
            
          
            string selectedUnitName = "";
            if (activeUnit != null)
            {
                selectedUnitName = activeUnit.name;
                if (GUILayout.Button("+100 EXP"))
                {
                    activeUnit.ExperienceManager.AddExp(new Vector2(0,0),100);
                }
                GUILayout.Label("Selected Unit: " + selectedUnitName);
                GUILayout.Label("Hp: " + activeUnit.Hp + "/" + activeUnit.MaxHp);
                GUILayout.Label("SkillsCount: " + activeUnit.SkillManager.Skills.Count);
                foreach (var skill in activeUnit.SkillManager.Skills)
                {
                    GUILayout.Label("Skill: " + skill.name);
                }

                if (GUILayout.Button("Deal 5 DMG"))
                {
                    activeUnit.Hp -= 5;
                }
            }
            else
            {
                GUILayout.Label("No Unit Selected!");
            }
        }
    }
}