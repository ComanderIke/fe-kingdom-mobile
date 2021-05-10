using UnityEngine;

namespace Game.WorldMapStuff.Systems
{
    internal class WM_GameplayInput
    {
        public void SelectParty(Party party)
        {
            if (!party.TurnState.IsWaiting)
            {
                Debug.Log(" Select Active Party");
            }
            else
            {
                Debug.Log("Select Unactive Party");
            }
        }

        public void SelectEnemyParty(Party party)
        {
            throw new System.NotImplementedException();
        }

        public void AttackPreviewEnemyParty(Party party)
        {
            throw new System.NotImplementedException();
        }

        public void AttackEnemyParty(Party party)
        {
            throw new System.NotImplementedException();
        }
    }
}