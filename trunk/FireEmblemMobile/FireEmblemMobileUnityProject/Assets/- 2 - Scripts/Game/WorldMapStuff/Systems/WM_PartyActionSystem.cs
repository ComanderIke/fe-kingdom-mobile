using Game.WorldMapStuff.Systems;
using UnityEngine;

namespace Game.WorldMapStuff.Input
{
    internal class WM_PartyActionSystem
    {
 
        public void AttackPreviewParty(IWM_Actor party)
        {
            Debug.Log("TODO UI AttackPreview Party");
        }

        public void AttackParty(IWM_Actor party)
        {
            Debug.Log("TODO Attack Party");
        }

        public void MoveParty(IWM_Actor party, WorldMapPosition location)
        {
            Debug.Log("TODO Move Party");
        }

        public void Wait(IWM_Actor party)
        {
            Debug.Log("TODO Wait Party");
        }
    }
}