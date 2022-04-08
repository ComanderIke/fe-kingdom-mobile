using System;
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
            if (GridGameManager.Instance == null)
                return;
            UnitSelectionSystem system = GridGameManager.Instance.GetSystem<UnitSelectionSystem>();
            if (system != null)
            {
                if (activeUnit != (Unit)system.SelectedCharacter)
                {
                    activeUnit = (Unit)system.SelectedCharacter;
                    Repaint();
                    //Debug.Log("Repaint!");
                }
            }

           
        }

        public void OnGUI()
        {
            //Debug.Log("OnGUI");
            
          
            string selectedUnitName = "";
            if (activeUnit != null)
                selectedUnitName = activeUnit.name;
            GUILayout.Label("Selected Unit: "+selectedUnitName);
        }
    }
}