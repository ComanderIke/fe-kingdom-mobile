using System;
using System.Collections.Generic;
using _02_Scripts.EditorScripts;
using UnityEngine;

[CreateAssetMenu(menuName = "BattlePreviewEditorSO")]
public class BattlePreviewEditorSO : ScriptableObject
{
    [SerializeField]private List<BattlePreviewForEditor> battlePreviews;
   

    public void OnValidate()
    {
        foreach (var battlePreview in battlePreviews)
        {
            battlePreview.OnValidate();
        }
        
    }
}