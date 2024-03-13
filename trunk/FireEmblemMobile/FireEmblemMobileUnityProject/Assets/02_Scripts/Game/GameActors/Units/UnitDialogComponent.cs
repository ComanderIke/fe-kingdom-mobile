using Game.Dialog;
using Game.GameActors.Units.Visuals;
using UnityEngine;

namespace Game.GameActors.Units
{
    [CreateAssetMenu(menuName = "GameData/Dialog/UnitDialogComponent")]
    public class UnitDialogComponent:ScriptableObject
    {
        [SerializeField]private Conversation goodLevelUpConversation;
        [SerializeField]private Conversation averageLevelUpConversation;
        [SerializeField]private Conversation badLevelUpConversation;
        public DialogSpriteSet DialogueSpriteSet;

        public Conversation GetGoodLevelUpConversation()
        {
            return averageLevelUpConversation;
        }
        public Conversation GetNormalLevelUpConversation()
        {
            return averageLevelUpConversation;
        }
        public Conversation GetBadLevelUpConversation()
        {
            return averageLevelUpConversation;
        }
        
    }
}