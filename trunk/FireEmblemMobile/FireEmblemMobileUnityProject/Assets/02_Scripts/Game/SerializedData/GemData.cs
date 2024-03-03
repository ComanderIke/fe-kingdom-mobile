using System;
using Game.DataAndReferences.Data;
using Game.GameActors.Items.Gems;
using UnityEngine;

namespace Game.SerializedData
{
    [System.Serializable]
    public class GemData
    {
        [SerializeField] public string gemId;
        [SerializeField] public int souls;

        public GemData(Gem gemSlotGem)
        {
            gemId = gemSlotGem.Name;
            souls = gemSlotGem.GetCurrentSouls();
        }

        public Gem Load()
        {
            Gem gem = null;
            if (!String.IsNullOrEmpty(gemId))
            {
                gem = GameBPData.Instance.GetGem(gemId);
                gem.SetSouls(souls);
            }
            return gem;
        }
    }
}