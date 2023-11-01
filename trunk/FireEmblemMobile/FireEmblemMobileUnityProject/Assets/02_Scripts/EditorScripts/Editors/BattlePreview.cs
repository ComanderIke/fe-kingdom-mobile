using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using _02_Scripts.EditorScripts;
using Game.GameActors.Units;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

[CustomEditor(typeof(BattlePreviewEditorSO))]
public class BattlePreviewWindow : Editor
{
	public const float MIN_WIDTH = 150.0f;

	private SerializedProperty battlePreviews;
	//private SerializedProperty attackerLevel;
	private void OnEnable()
	{
		battlePreviews=serializedObject.FindProperty("battlePreviews");
		
	}
	


	public override void OnInspectorGUI()
	{
		
		// have 2 slots for Unit BP left and right and next to them input field for level
		// and a swap button in the middle
		// then below left and right
		// atk hit crit AS
		//calculate stats whenever unit or level input field changes.
		serializedObject.Update();
		EditorGUILayout.PropertyField(battlePreviews);//, new GUIContent("BattlePreviews"), GUILayout.Height(2000),
			//GUILayout.Width(400));
		for (int i = 0; i < battlePreviews.arraySize; i++) {
			GUILayout.BeginHorizontal();
			//BattlePreviewForEditor bp=(BattlePreviewForEditor)battlePreviews.GetArrayElementAtIndex(i).objectReferenceValue;
				EditorGUIUtility.labelWidth = 65;
				var attackerLevel = battlePreviews.GetArrayElementAtIndex(i).FindPropertyRelative("attackerLevel");
			EditorGUILayout.PropertyField(attackerLevel, new GUIContent("Level"), GUILayout.Height(20),
				GUILayout.Width(100)); //, GUILayout.ExpandWidth(false));
			 var attacker = battlePreviews.GetArrayElementAtIndex(i).FindPropertyRelative("attacker");
			 EditorGUILayout.PropertyField(attacker, GUILayout.Height(20), GUILayout.Width(200));
			 var defenderLevel = battlePreviews.GetArrayElementAtIndex(i).FindPropertyRelative("defenderLevel");
			 EditorGUILayout.PropertyField(defenderLevel, new GUIContent("Level"), GUILayout.Height(20),
			 	GUILayout.Width(100));
			 var defender = battlePreviews.GetArrayElementAtIndex(i).FindPropertyRelative("defender");
			 EditorGUILayout.PropertyField(defender, GUILayout.Height(20), GUILayout.Width(200));
			
			
			 GUILayout.EndHorizontal();
			 GUILayout.BeginHorizontal();
			 GUILayout.BeginVertical();
			 var attackerHp = battlePreviews.GetArrayElementAtIndex(i).FindPropertyRelative("attackerHp");
			 EditorGUILayout.PropertyField(attackerHp, new GUIContent("Hp"), GUILayout.Height(20), GUILayout.Width(100));
			  var attackerDamage = battlePreviews.GetArrayElementAtIndex(i).FindPropertyRelative("attackerDamage");
			 EditorGUILayout.PropertyField(attackerDamage, new GUIContent("Damage"), GUILayout.Height(20),
			 	GUILayout.Width(100));
			 var attackerHit = battlePreviews.GetArrayElementAtIndex(i).FindPropertyRelative("attackerHit");
			 EditorGUILayout.PropertyField(attackerHit, new GUIContent("Hit"), GUILayout.Height(20),
			 	GUILayout.Width(100));
			 var attackerCrit = battlePreviews.GetArrayElementAtIndex(i).FindPropertyRelative("attackerCrit");
			 EditorGUILayout.PropertyField(attackerCrit, new GUIContent("Crit"), GUILayout.Height(20),
			 	GUILayout.Width(100));
			 var attackerAttackSpeed = battlePreviews.GetArrayElementAtIndex(i).FindPropertyRelative("attackerAttackSpeed");
			 EditorGUILayout.PropertyField(attackerAttackSpeed, new GUIContent("AS"), GUILayout.Height(20),
			 	GUILayout.Width(100));


			 GUILayout.EndVertical();
			 GUILayout.BeginVertical();
			 var defenderHp = battlePreviews.GetArrayElementAtIndex(i).FindPropertyRelative("defenderHp");
			 EditorGUILayout.PropertyField(defenderHp, new GUIContent("Hp"), GUILayout.Height(20), GUILayout.Width(100));
			 var defenderDamage = battlePreviews.GetArrayElementAtIndex(i).FindPropertyRelative("defenderDamage");
			 EditorGUILayout.PropertyField(defenderDamage, new GUIContent("Damage"), GUILayout.Height(20),
			 	GUILayout.Width(100));
			 var defenderHit = battlePreviews.GetArrayElementAtIndex(i).FindPropertyRelative("defenderHit");
			 EditorGUILayout.PropertyField(defenderHit, new GUIContent("Hit"), GUILayout.Height(20),
			 	GUILayout.Width(100));
			 var defenderCrit = battlePreviews.GetArrayElementAtIndex(i).FindPropertyRelative("defenderCrit");
			 EditorGUILayout.PropertyField(defenderCrit, new GUIContent("Crit"), GUILayout.Height(20),
			 	GUILayout.Width(100));
			 var defenderAttackSpeed = battlePreviews.GetArrayElementAtIndex(i).FindPropertyRelative("defenderAttackSpeed");
			 EditorGUILayout.PropertyField(defenderAttackSpeed, new GUIContent("AS"), GUILayout.Height(20),
			 	GUILayout.Width(100));


			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			//EditorGUILayout.PropertyField(battlePreviews.GetArrayElementAtIndex(i));
		}
		serializedObject.ApplyModifiedProperties();

		
	}
	
}
