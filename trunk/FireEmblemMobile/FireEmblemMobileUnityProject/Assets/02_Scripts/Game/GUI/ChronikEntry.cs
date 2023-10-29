using UnityEngine;

namespace LostGrace
{
    [System.Serializable]
    public class ChronikEntry : IChronikEntry
    {
        [field:SerializeField ]public Sprite BodySprite { get; set; }
        [field:SerializeField ]public Sprite FaceSprite { get; set; }
        [field:SerializeField ]public string Name { get; set; }
        [field:SerializeField, TextAreaAttribute]public string Description { get; set; }
    }
}