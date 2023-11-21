using UnityEngine;
using UnityEngine.Rendering.LookDev;

namespace Game.GameActors.Units.CharStateEffects
{
    public abstract class BuffDebuffBase : ScriptableObject
    {
        [SerializeField] private GameObject vfx;
        [SerializeField] public int[] duration;
        [field:SerializeField] public Sprite Icon { get; set; }

        protected int level;

        public virtual void Apply(Unit caster, Unit target, int skilllevel)
        {
            if (vfx != null)
            {
                var go = Instantiate(vfx, null);
                go.transform.position = target.GameTransformManager.GetCenterPosition();
            }

            Debug.Log("SPAWNED VFX");
            this.level = skilllevel;
        }
        public void IncreaseCurrentDuration()
        {
            duration[level]++;
        }
        public virtual bool TakeEffect(Unit unit)
        {
            Debug.Log("TAKE EFFECT"+duration[level]);
            duration[level]--;
            return duration[level] <= 0;
        }

        public virtual void Unapply(Unit target)
        {
            level = 0;
        }

        public int GetDuration()
        {
            return duration[level];
        }
    }
}