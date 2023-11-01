using UnityEditor;
using UnityEngine;

namespace _02_Scripts.EditorScripts.Editors
{
	[CustomEditor(typeof(BattlePreviewForEditor))]
	public class BattlePreviewEditor : Editor
	{
		public const float MIN_WIDTH = 150.0f;

		private SerializedProperty battlePreviews;

		private void OnEnable()
		{

			attackerLevel = serializedObject.FindProperty("attackerLevel");
			defenderLevel = serializedObject.FindProperty("defenderLevel");
			attacker = serializedObject.FindProperty("attacker");
			defender = serializedObject.FindProperty("defender");

			attackerHp = serializedObject.FindProperty("attackerHp");
			attackerDamage = serializedObject.FindProperty("attackerDamage");
			attackerHit = serializedObject.FindProperty("attackerHit");
			attackerCrit = serializedObject.FindProperty("attackerCrit");
			attackerAttackSpeed = serializedObject.FindProperty("attackerAttackSpeed");

			defenderHp = serializedObject.FindProperty("defenderHp");
			defenderDamage = serializedObject.FindProperty("defenderDamage");
			defenderHit = serializedObject.FindProperty("defenderHit");
			defenderCrit = serializedObject.FindProperty("defenderCrit");
			defenderAttackSpeed = serializedObject.FindProperty("defenderAttackSpeed");
		}


		private SerializedProperty attackerLevel;
		private SerializedProperty defenderLevel;
		private SerializedProperty attacker;
		private SerializedProperty defender;

		private SerializedProperty attackerHp;
		private SerializedProperty attackerDamage;
		private SerializedProperty attackerHit;
		private SerializedProperty attackerCrit;
		private SerializedProperty attackerAttackSpeed;

		private SerializedProperty defenderHp;
		private SerializedProperty defenderDamage;
		private SerializedProperty defenderHit;
		private SerializedProperty defenderCrit;
		private SerializedProperty defenderAttackSpeed;


		public override void OnInspectorGUI()
		{

			// have 2 slots for Unit BP left and right and next to them input field for level
			// and a swap button in the middle
			// then below left and right
			// atk hit crit AS
			//calculate stats whenever unit or level input field changes.
			serializedObject.Update();
			GUILayout.BeginHorizontal();
			GUILayout.ExpandWidth(false);

			//GUILayout.Label("Level");
			EditorGUIUtility.labelWidth = 65;
			EditorGUILayout.PropertyField(attackerLevel, new GUIContent("Level"), GUILayout.Height(20),
				GUILayout.Width(100)); //, GUILayout.ExpandWidth(false));
			EditorGUILayout.PropertyField(attacker, GUILayout.Height(20), GUILayout.Width(200));
			// if (GUILayout.Button("Swap", GUILayout.Height(20), GUILayout.Width(45)))
			// {
			// 	Debug.Log("Swap");
			// 	(attacker, defender) = (defender, attacker);
			// 	(attackerLevel, defenderLevel) = (defenderLevel, attackerLevel);
			// }
			EditorGUILayout.PropertyField(defenderLevel, new GUIContent("Level"), GUILayout.Height(20),
				GUILayout.Width(100));
			EditorGUILayout.PropertyField(defender, GUILayout.Height(20), GUILayout.Width(200));


			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			EditorGUILayout.PropertyField(attackerHp, new GUIContent("Hp"), GUILayout.Height(20), GUILayout.Width(100));
			EditorGUILayout.PropertyField(attackerDamage, new GUIContent("Damage"), GUILayout.Height(20),
				GUILayout.Width(100));
			EditorGUILayout.PropertyField(attackerHit, new GUIContent("Hit"), GUILayout.Height(20),
				GUILayout.Width(100));
			EditorGUILayout.PropertyField(attackerCrit, new GUIContent("Crit"), GUILayout.Height(20),
				GUILayout.Width(100));
			EditorGUILayout.PropertyField(attackerAttackSpeed, new GUIContent("AS"), GUILayout.Height(20),
				GUILayout.Width(100));


			GUILayout.EndVertical();
			GUILayout.BeginVertical();
			EditorGUILayout.PropertyField(defenderHp, new GUIContent("Hp"), GUILayout.Height(20), GUILayout.Width(100));
			EditorGUILayout.PropertyField(defenderDamage, new GUIContent("Damage"), GUILayout.Height(20),
				GUILayout.Width(100));
			EditorGUILayout.PropertyField(defenderHit, new GUIContent("Hit"), GUILayout.Height(20),
				GUILayout.Width(100));
			EditorGUILayout.PropertyField(defenderCrit, new GUIContent("Crit"), GUILayout.Height(20),
				GUILayout.Width(100));
			EditorGUILayout.PropertyField(defenderAttackSpeed, new GUIContent("AS"), GUILayout.Height(20),
				GUILayout.Width(100));


			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			serializedObject.ApplyModifiedProperties();


		}
	}
}