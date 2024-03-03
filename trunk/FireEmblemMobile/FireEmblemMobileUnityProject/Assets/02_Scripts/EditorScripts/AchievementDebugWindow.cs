using Game.AchievementSystem;
using Game.GameActors.Units;
using Game.Manager;
using UnityEditor;
using UnityEngine;

namespace __2___Scripts.External.Editor
{
    public class AchievementDebugWindow: EditorWindow
    {
      
        [MenuItem("Tools/AchievementDebugWindow")]
        public static void ShowMyEditor()
        {
            // This method is called when the user selects the menu item in the Editor
            EditorWindow wnd = GetWindow<AchievementDebugWindow>();
            wnd.titleContent = new GUIContent("Achievement Debug Viewer");
        }
        

        public void OnGUI()
        {
            //Debug.Log("OnGUI");
            var am = AchievementManager.instance;
            if (am != null)
            {
                foreach (var achievement in am.AchievementList)
                {
                    if (GUILayout.Button("CompleteAchievement "+achievement.Key))
                    {
                        am.Unlock(achievement.Key);
                    }
                }
            }
            else
            {
                GUILayout.Label("No Unit Selected!");
            }
        }
    }
}