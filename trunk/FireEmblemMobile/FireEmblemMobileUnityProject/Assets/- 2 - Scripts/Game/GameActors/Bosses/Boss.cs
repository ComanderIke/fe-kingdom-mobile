using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;

[System.Serializable]
public class Boss : Unit
{
    public BossStage[] stages;
    private BossStage activeStage;
    public BossAttackPreviewRenderer previewRenderer;
}