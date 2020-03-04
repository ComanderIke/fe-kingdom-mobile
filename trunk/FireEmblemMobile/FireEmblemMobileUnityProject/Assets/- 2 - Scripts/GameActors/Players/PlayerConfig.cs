using UnityEngine;

namespace Assets.GameActors.Players
{
    public class PlayerConfig : MonoBehaviour
    {
        [Header("Please specify players")] [SerializeField]
        public Faction[] Factions;
    }
}