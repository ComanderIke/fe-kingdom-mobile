using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Ressources
{
    [System.Serializable]
    public class Prefabs
    {
        public GameObject[] specialAttackPrefabs;
        [Header("CharacterMovement")]
        public GameObject moveArrowDot;
        public GameObject activeUnitField;
        public GameObject moveCursor;
        public GameObject attackableEnemyPrefrab;

        public GameObject GetSpecialAttackPrefab(int prefabId)
        {
            if (prefabId >= 0 && prefabId < specialAttackPrefabs.Length)
                return specialAttackPrefabs[prefabId];
            return null;
        }
    }
}
