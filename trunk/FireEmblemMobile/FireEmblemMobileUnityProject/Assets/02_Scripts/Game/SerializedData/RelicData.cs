using Game.GameActors.Items.Weapons;
using Game.GameResources;
using UnityEngine;

namespace Game.GameActors.Players
{
    [System.Serializable]
    public class RelicData
    {
        [SerializeField] public string relicId;
        [SerializeField] public int relicLvl;
        [SerializeField] public GemData gemData;

        public RelicData(Relic relic)
        {
            relicId = relic.Name;
            relicLvl = relic.level;
            if (!relic.gemSlot.IsEmpty())
                   gemData=new GemData(relic.gemSlot.gem);
                
            
        }

        public Relic Load()
        {
            Relic relic = null;
            if (!string.IsNullOrEmpty(relicId))
            {
                
                relic = GameBPData.Instance.GetRelic(relicId);
                relic.level = relicLvl;
                if (gemData != null)
                    relic.gemSlot.gem = gemData.Load();
            }
            return relic;
        }
    }
}