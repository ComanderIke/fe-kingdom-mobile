using UnityEngine;

namespace Game.GUI
{
    [System.Serializable]
    public class ChronikEntry : IChronikEntry
    {
        [field:SerializeField ]public Sprite BodySprite { get; set; }
        [field:SerializeField ]public Sprite FaceSprite { get; set; }
        [field:SerializeField ]public Sprite AlternateBodySprite { get; set; }
        [field:SerializeField ]public Sprite AlternateFaceSprite { get; set; }
        [field:SerializeField ]public string Name { get; set; }
        [field:SerializeField, TextArea]public string Description { get; set; }
    }
}