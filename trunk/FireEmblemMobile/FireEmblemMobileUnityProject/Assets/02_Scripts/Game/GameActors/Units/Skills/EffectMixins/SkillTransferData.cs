using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/TransferData", fileName = "SkillTransferData")]
    public class SkillTransferData : ScriptableObject
    {
        public object data;
    }
}