using Assets.GameInput;
using UnityEngine;

namespace Assets.Grid
{
    [System.Serializable]
    public class GridResources
    {
        public Texture HealTexture;
        public Texture MouseHoverTexture;
        public Texture SkillRangeTexture;
        public Material CellMaterialStandard;
        public Material CellMaterialAttack;
        public Material CellMaterialSoftAttack;
        public Material CellMaterialMovement;
        public Material CellMaterialSoftMovement;
        public Material CellMaterialInvalid;
        public Material CellMaterialStandOn;
        public Material CellMaterialAlly;
        public Material CellMaterialAttackableEnemy;
    }
}