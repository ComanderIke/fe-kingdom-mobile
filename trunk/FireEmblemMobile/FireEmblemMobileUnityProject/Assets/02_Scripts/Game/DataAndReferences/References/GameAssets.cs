using GameEngine;
using UnityEngine;

namespace Game.DataAndReferences.References
{
    [CreateAssetMenu(fileName = "GameAssets",menuName ="GameData/Config/GameAssets" )]
    public class GameAssets : SingletonScriptableObject<GameAssets>
    {

        public Visuals visuals;
        public Prefabs prefabs;
        public TileResources tiles;
        public GridResources grid;
        public Colors colors;
    }

    [System.Serializable]
    public class Colors
    {

        [ColorUsage(true, true)]
        public Color AttackColor;
        [ColorUsage(true, true)]
        public Color HealColor;
        [ColorUsage(true, true)]
        public Color BuffColor;
    }
}
