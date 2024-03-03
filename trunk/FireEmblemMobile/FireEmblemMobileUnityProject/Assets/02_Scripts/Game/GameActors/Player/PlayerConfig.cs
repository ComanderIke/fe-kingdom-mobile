using Game.GameActors.Factions;
using UnityEngine;

namespace Game.GameActors.Player
{
    public class PlayerConfig : MonoBehaviour
    {
        [Header("Please specify players")] [SerializeField]
        public Faction[] Factions;
    }
}