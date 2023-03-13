using UnityEngine;

namespace Game.GameActors.Units
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "GameData/Units/Audio", fileName = "CharAudioSet")]
    public class CharacterAudioSet:ScriptableObject
    {
        [field:SerializeField] public AudioClip TakingDamage { get; set; }
        [field:SerializeField] public AudioClip Attack { get; set; }
        [field:SerializeField] public AudioClip Critical { get; set; }
        [field:SerializeField] public AudioClip Dodge { get; set; }
        [field:SerializeField] public AudioClip Walk { get; set; }
 
    }
}