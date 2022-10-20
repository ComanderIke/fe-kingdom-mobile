using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Weapons/WeaponType", fileName = "weaponType1")]
    public class WeaponType: ScriptableObject
    {
        [SerializeField] private String name;
        [SerializeField] private Sprite icon;

        public string Name => name;
        [SerializeField] private List<MoveType> effectiveAgainst;
        [SerializeField] private List<MoveType> inEffectiveAgainst;
        [SerializeField] private List<WeaponType> effectiveAgainstWeapons;
        [SerializeField] private List<WeaponType> inEffectiveAgainstWeapons;
        public Sprite Icon => icon;
        public bool IsEffective(MoveType unitMoveType)
        {
            return effectiveAgainst.Contains(unitMoveType);
        }
        public bool IsInEffective(MoveType unitMoveType)
        {
            return inEffectiveAgainst.Contains(unitMoveType);
        }
        public bool IsEffective(WeaponType weaponType)
        {
            return effectiveAgainstWeapons.Contains(weaponType);
        }
        public bool IsInEffective(WeaponType weaponType)
        {
            return inEffectiveAgainstWeapons.Contains(weaponType);
        }
    }
}