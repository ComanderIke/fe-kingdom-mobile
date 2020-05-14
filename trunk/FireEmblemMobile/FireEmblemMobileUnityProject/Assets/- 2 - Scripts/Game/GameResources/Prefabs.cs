using System;
using UnityEngine;

namespace Assets.GameResources
{
    [Serializable]
    public class Prefabs
    {
        public GameObject ActiveUnitField;
        public GameObject ActiveUnitFieldAlternate;
        public GameObject AttackableEnemyPrefab;

        [Header("CharacterMovement")] public GameObject MoveArrowDot;

        public GameObject MoveCursor;
        public GameObject MoveCursorViolet;

        public GameObject attackIconPrefab;
        [Header("Battle")]
        public GameObject slashRed;
        public GameObject slashBlue;
        public GameObject DamagePopUptext;

    }
}