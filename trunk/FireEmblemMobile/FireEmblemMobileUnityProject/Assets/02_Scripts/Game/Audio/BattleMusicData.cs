using System;
using UnityEngine;

namespace Audio
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Audio/BattleMusic", fileName = "Music")]
    public class BattleMusicData : MusicData
    {
        public AudioClip ClipInBattle;
    
        [Range(0f, 1.0f)] public float VolumeBattle = 1;
    }
}