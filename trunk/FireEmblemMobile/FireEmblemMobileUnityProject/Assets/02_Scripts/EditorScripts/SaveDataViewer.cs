using System;
using System.IO;
using System.Net;
using Game.EncounterAreas.AreaConstruction;
using Game.GameActors.Player;
using Game.GameActors.Units;
using Game.Manager;
using Game.SerializedData;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace __2___Scripts.External.Editor
{
    public class SaveDataWindow : EditorWindow
    {
        private PlayerData playerData;
        private EncounterTreeData encounterData;

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


        string jsonBeatuify = "";
        Vector2 scrollPosition1;
        Vector2 scrollPosition2;
        Vector2 scrollPositionLoadFiles;
        private int activeIndex = 0;
        private string saveName = "File001";
        private string[] loadFiles;
        private string chosenFile = "";

        public void OnGUI()
        {
            loadFiles = SaveGameManager.GetLoadFiles();
            //Debug.Log("OnGUI");
            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("ShowPlayerData!", GUILayout.Width(150)))
            {
                activeIndex = 0;
                playerData = new PlayerData(Player.Instance);
            }
            else if (GUILayout.Button("Show EncounterData!", GUILayout.Width(150)))
            {
                activeIndex = 1;
                encounterData = new EncounterTreeData(EncounterTree.Instance);
            }
            
            if (GUILayout.Button("Save Data!", GUILayout.Width(150)))
            {
                SaveGameManager.Save();
                loadFiles = SaveGameManager.GetLoadFiles();
            }
            
            if (GUILayout.Button("Load Autosave Data!", GUILayout.Width(150)))
            {
                SaveGameManager.Load(0);
                playerData = SaveGameManager.currentSaveData.playerData;
                encounterData = SaveGameManager.currentSaveData.EncounterAreaData.encounterTreeData;
            }
            
            
            
            
            
            
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            
            if (activeIndex == 0)
                ShowPlayerData();
            else
            {
                ShowEncounterTreeData();
            }
            GUILayout.BeginVertical();
            GUILayout.Label("SaveGames: ");
            scrollPositionLoadFiles=GUILayout.BeginScrollView(scrollPositionLoadFiles);
            int index = 0;
            foreach (string loadFile in loadFiles)
            {
                string shortName = Path.GetFileNameWithoutExtension(loadFile);
                if(GUILayout.Button(shortName, GUILayout.Width(150)))
                {
                    chosenFile = shortName;
                    SaveGameManager.Load(index);
                    playerData = SaveGameManager.currentSaveData.playerData;
                    encounterData = SaveGameManager.currentSaveData.EncounterAreaData.encounterTreeData;
                }

                index++;
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            
            saveName = GUILayout.TextField(saveName);
            if (GUILayout.Button("Save File: " + saveName, GUILayout.Width(150)))
            {
                SaveGameManager.Save(0);
                
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void ShowEncounterTreeData()
        {
            if (encounterData != null)
            {
                jsonBeatuify = JsonUtility.ToJson(encounterData, true);
                GUILayout.BeginHorizontal();
                scrollPosition1 =
                    GUILayout.BeginScrollView(scrollPosition1, GUILayout.Width(295), GUILayout.Height(400));
                GUILayout.BeginVertical();

                GUILayout.Label("EncounterTree: ");
                GUILayout.TextArea(EncounterTree.Instance.ToString());

                GUILayout.EndVertical();
                GUILayout.EndScrollView();
                scrollPosition2 =
                    GUILayout.BeginScrollView(scrollPosition2, GUILayout.Width(445), GUILayout.Height(400));
                GUILayout.BeginVertical();
                GUILayout.Label("EncounterTreeData: ");
                GUILayout.TextArea(jsonBeatuify);
                GUILayout.EndVertical();
                GUILayout.EndScrollView();
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.Label("No EncounterTree Data!");
            }
        }

        private void ShowPlayerData()
        {
            if (playerData != null)
            {
                jsonBeatuify = JsonUtility.ToJson(playerData, true);
                GUILayout.BeginHorizontal();
                scrollPosition1 =
                    GUILayout.BeginScrollView(scrollPosition1, GUILayout.Width(295), GUILayout.Height(400));
                GUILayout.BeginVertical();

                GUILayout.Label("Player: ");
                GUILayout.TextArea(Player.Instance.ToString());

                GUILayout.EndVertical();
                GUILayout.EndScrollView();
                scrollPosition2 =
                    GUILayout.BeginScrollView(scrollPosition2, GUILayout.Width(445), GUILayout.Height(400));
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