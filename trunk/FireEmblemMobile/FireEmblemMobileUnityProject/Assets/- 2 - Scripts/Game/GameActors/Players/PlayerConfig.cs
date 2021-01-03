using UnityEngine;

namespace Game.GameActors.Players
{
    public class PlayerConfig : MonoBehaviour
    {
        [Header("Please specify players")] [SerializeField]
        public Faction[] Factions;
    }
}