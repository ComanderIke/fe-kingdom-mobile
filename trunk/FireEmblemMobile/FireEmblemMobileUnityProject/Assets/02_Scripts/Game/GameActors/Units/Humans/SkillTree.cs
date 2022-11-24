using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillTree
{
    public string name="TestSkillTree";
    public int currentDepth = 0;
    [SerializeField] public List<SkillTreeEntry> skillEntries;

    public void Init()
    {
        foreach (var skillEntry in skillEntries)
        {
            skillEntry.tree = this;
        }
     
    }
}