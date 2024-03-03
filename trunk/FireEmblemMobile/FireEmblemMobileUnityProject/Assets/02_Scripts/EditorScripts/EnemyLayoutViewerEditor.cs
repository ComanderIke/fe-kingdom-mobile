using Game.Grid;
using Game.Utility;
using UnityEditor;
using UnityEngine;

namespace __2___Scripts.External.Editor
{
    [CustomEditor(typeof(EnemyLayoutViewer))]
    public class EnemyLayoutViewerEditor : UnityEditor.Editor
    {

        private EnemyLayoutViewer mytarget;

        private void OnEnable()
        {
            mytarget = (EnemyLayoutViewer)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Show"))
            {
                mytarget.Show();
            }
            else if (GUILayout.Button("Hide"))
            {
                mytarget.Hide();
            }
            else if (GUILayout.Button("ApplyChanges"))
            {
                mytarget.ApplyChanges();
            }
        }
    }
}