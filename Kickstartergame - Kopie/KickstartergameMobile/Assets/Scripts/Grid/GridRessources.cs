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
        public Texture MoveTexture;
        public Texture StandOnTexture;
        public Texture AttackTexture;
        public Texture EnemyAttackTexture;
        public Texture StandardTexture;
        public Texture healTexture;
        public Texture mouseHoverTexture;
        public Texture skillRangeTexture;
        public Material cellMaterialValid;
        public Material cellMaterialInvalid;
        public PreferedMovementPath preferedPath;
    }
}
