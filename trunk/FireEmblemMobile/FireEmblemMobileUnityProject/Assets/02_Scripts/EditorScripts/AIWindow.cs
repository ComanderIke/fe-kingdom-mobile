using Game.AI;
using Game.GameActors.Players;
using Game.WorldMapStuff.Model;
using UnityEditor;
using UnityEngine;

namespace __2___Scripts.External.Editor
{
    public class AIWindow: EditorWindow
    {
        private AIRenderer renderer;
        [MenuItem("Tools/AI_Window")]
        public static void ShowMyEditor()
        {
            // This method is called when the user selects the menu item in the Editor
            EditorWindow wnd = GetWindow<AIWindow>();
            wnd.titleContent = new GUIContent("AI Viewer");
            
        }
       
        public void Update()
        {
            if(renderer==null)
                renderer = FindObjectOfType<AIRenderer>();
           
        }

        public void OnGUI()
        {
            if (renderer == null)
                return;
            if (GUILayout.Button("ShowTargets"))
            {
                renderer.ShowInitTurnData();
            }
            if (GUILayout.Button("HideTargets"))
            {
                renderer.Hide();
            }
            GUILayout.Label("Current Units: ");
            if (renderer.brain == null)
                return;
            
            foreach (var u in renderer.brain.PlayerFaction.Units)
            {
                GUILayout.Label("Unit: "+u.name+", ");
            }
        }
    }
    
    }