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
   
    public class SaveDataWindow: EditorWindow
    {
        private PlayerData playerData;
        [MenuItem("Tools/FE_SaveData")]
        public static void ShowMyEditor()
        {
            // This method is called when the user selects the menu item in the Editor
            EditorWindow wnd = GetWindow<SaveDataWindow>();
            wnd.titleContent = new GUIContent("SaveData Viewer");
        }
       
        public void Update()
        {
            if (Player.Instance == null)
                return;
            



        }
        string json = "";
        string jsonBeatuify = "";
        Vector2 scrollPosition1;
        Vector2 scrollPosition2;
        public void OnGUI()
        {
            //Debug.Log("OnGUI");
            
          
            string selectedUnitName = "";
          
            if (GUILayout.Button("UpdatePlayerData!")||playerData==null)
            {
                playerData = new PlayerData(Player.Instance);
                Debug.Log("Updating Player Data: "+Player.Instance.Name);
                json=JsonUtility.ToJson(playerData, false);
                jsonBeatuify=JsonUtility.ToJson(playerData, true);
            }

            if (playerData != null)
            {
                GUILayout.BeginHorizontal();
                scrollPosition1 = GUILayout.BeginScrollView(scrollPosition1,GUILayout.Width(495),GUILayout.Height(400));
                GUILayout.BeginVertical();
               
                GUILayout.Label("Player: ");
                GUILayout.TextArea(Player.Instance.ToString());

                GUILayout.EndVertical();
                GUILayout.EndScrollView();
                scrollPosition2 = GUILayout.BeginScrollView(scrollPosition2,GUILayout.Width(495),GUILayout.Height(400));
                GUILayout.BeginVertical();
                GUILayout.Label("PlayerData: ");
                GUILayout.TextArea(jsonBeatuify);
                GUILayout.EndVertical();
                GUILayout.EndScrollView();
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.Label("No Player Data!");
            }
        }
    }
}