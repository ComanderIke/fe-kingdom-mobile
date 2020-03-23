using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SerializedData
{
    [Serializable]
    public class GameProgress
    {

        [SerializeField] private int currentChapter = default;
        [SerializeField] private List<int> completedChapter = default;
 
    }
}
