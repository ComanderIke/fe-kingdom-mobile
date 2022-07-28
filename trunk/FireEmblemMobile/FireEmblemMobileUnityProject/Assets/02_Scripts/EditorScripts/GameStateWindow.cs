using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.Manager;
using Game.Mechanics;
using GameEngine;
using GameEngine.GameStates;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace __2___Scripts.External.Editor
{
   
    public class GameStateWindow: EditorWindow
    {
        private GameState<NextStateTrigger> activeState;
        private List<GameState<NextStateTrigger>> previousStates;

        public GameStateWindow()
        {
            previousStates = new List<GameState<NextStateTrigger>>();
        }
        [MenuItem("Tools/GamestateWindow")]
        public static void ShowMyEditor()
        {
            // This method is called when the user selects the menu item in the Editor
            EditorWindow wnd = GetWindow<GameStateWindow>();
          
            wnd.titleContent = new GUIContent("GamestateWindow");
        }
       
        public void Update()
        {
            if (GridGameManager.Instance == null)
                return;
            var currentState=GridGameManager.Instance.GameStateManager.GetActiveState();
            if (currentState != null)
            {
                if (activeState != currentState)
                {
                    if(previousStates.Count>10)
                        previousStates.RemoveAt(0);
                    if(activeState!=null)
                        previousStates.Add(activeState);
                    activeState = currentState;
                    
                    Repaint();
                    //Debug.Log("Repaint!");
                }
            }

           
        }

        public void OnGUI()
        {
            if (activeState != null)
            {
                
                GUILayout.Label("ActiveState: " + activeState);
                GUILayout.Label("PreviousStates: ");
                foreach(var prevState in previousStates)
                    GUILayout.Label(prevState.ToString());
                
            }
            else
            {
                GUILayout.Label("No GameState Somehow!");
            }
        }
    }
}