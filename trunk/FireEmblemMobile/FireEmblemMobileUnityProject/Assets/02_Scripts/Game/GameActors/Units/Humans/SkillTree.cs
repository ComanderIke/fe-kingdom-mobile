using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillTree
{
    public string name="TestSkillTree";
    [SerializeField] public List<SkillTreeEntry> skillEntries;

    public void Init()
    {
        foreach (var skillEntry in skillEntries)
        {
            skillEntry.Init();
        }
     
    }
}