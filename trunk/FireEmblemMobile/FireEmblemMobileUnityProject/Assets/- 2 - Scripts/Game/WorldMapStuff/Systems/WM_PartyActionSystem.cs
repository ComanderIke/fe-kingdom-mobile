using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Systems;
using GameEngine;
using UnityEngine;

namespace Game.WorldMapStuff.Input
{
    public class WM_PartyActionSystem:IEngineSystem
    {
 
        public void AttackPreviewParty(WM_Actor party)
        {
            Debug.Log("TODO UI AttackPreview Party");
        }

        public void AttackParty(WM_Actor party)
        {
            Debug.Log("TODO Attack Party");
        }

        public void MoveParty(WM_Actor party, WorldMapPosition location)
        {
            Debug.Log("TODO Move Party");
        }

        public void Wait(WM_Actor party)
        {
            Debug.Log("TODO Wait Party");
        }

        public void Init()
        {
            
        }
    }
}