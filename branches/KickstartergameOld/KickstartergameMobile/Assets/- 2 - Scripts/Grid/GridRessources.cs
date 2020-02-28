using Assets.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Grid
{
    [System.Serializable]
    public class GridRessources
    {

        public Texture healTexture;
        public Texture mouseHoverTexture;
        public Texture skillRangeTexture;
        public Material cellMaterialStandard;
        public Material cellMaterialAttack;
        public Material cellMaterialEnemyAttack;
        public Material cellMaterialMovement;
        public Material cellMaterialInvalid;
        public Material cellMaterialStandOn;
        public Material cellMaterialAttackableEnemy;
        public PreferedMovementPath preferedPath;
    }
}
