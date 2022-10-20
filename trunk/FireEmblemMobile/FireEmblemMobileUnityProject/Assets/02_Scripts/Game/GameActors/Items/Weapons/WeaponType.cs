using System;
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

        public Sprite Icon => icon;
    }
}