using UnityEngine;

[CreateAssetMenu(menuName = "GameData/DialogActor", fileName = "DialogActor")]
public class DialogActor : ScriptableObject, IDialogActor
{
    [SerializeField] private new string name;
    [SerializeField] private Sprite faceSprite;
    public string Name => name;
    public Sprite FaceSprite => faceSprite;
}