using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Goddess", fileName = "Goddess")]
public class Goddess:ScriptableObject, IDialogActor
{
    [SerializeField] private new string name;
    [SerializeField] private Sprite faceSprite;
    public string Name => name;
    public Sprite FaceSprite => faceSprite;
}