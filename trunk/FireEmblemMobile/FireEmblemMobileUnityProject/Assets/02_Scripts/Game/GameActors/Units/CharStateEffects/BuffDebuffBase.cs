using UnityEngine;

namespace Game.GameActors.Units.CharStateEffects
{
    public abstract class BuffDebuffBase : ScriptableObject
    {
        [SerializeField] public int[] duration;
        [field:SerializeField] public Sprite Icon { get; set; }

        protected int level;

        public void ApplyBuff( int skilllevel)
        {
            this.level = skilllevel;
        }
        public void IncreaseCurrentDuration()
        {
            duration[level]++;
        }
        public virtual bool TakeEffect(Unit unit)
        {
            return false;
        }
    }
}