using Game.GameActors.Units.CharStateEffects;
using Game.GameResources;
using UnityEngine;

namespace Game.GameActors.Players
{
    [System.Serializable]
    public class EncounterBasedBuffData
    {
        [SerializeField] public string buffId;
        [SerializeField] public int duration;

        public EncounterBasedBuffData(EncounterBasedBuff encounterBuff)
        {
            this.duration = encounterBuff.GetDuration();
            this.buffId = encounterBuff.ToString();
        }

        public EncounterBasedBuff Load()
        {
            EncounterBasedBuff buff=GameBPData.Instance.GetEncounterBuff(buffId);
            buff.duration = duration;
            return buff;
        }
    }
}