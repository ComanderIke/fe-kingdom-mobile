using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "GameData/Boss", fileName = "Boss")]
public class Boss : UnitBP
{
    public BossStage[] stages;
    private BossStage activeStage;
    public BossAttackPreviewRenderer previewRenderer;
}