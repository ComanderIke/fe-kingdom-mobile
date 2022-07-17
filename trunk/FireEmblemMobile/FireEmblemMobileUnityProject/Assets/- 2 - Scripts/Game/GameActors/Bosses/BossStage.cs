using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "GameData/BossStage", fileName = "BossStage")]
public class BossStage:ScriptableObject
{
    public int health;
    //public WeaponType weakness;

    
    public BossAttack[] attackPatterns;
    
}