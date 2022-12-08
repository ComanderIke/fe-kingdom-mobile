using LostGrace;

namespace Game.GameActors.Units.CharStateEffects
{
    public abstract class EncounterBasedBuff : ITemporaryEffect
    {
        protected int duration;
        public EncounterBasedBuff(int duration)
        {
            this.duration = duration;
        }
        public void DecreaseDuration()
        {
            throw new System.NotImplementedException();
        }

        public int GetDuration(int faith)
        {
            throw new System.NotImplementedException();
        }

        public abstract void Apply(Unit unit);

    }
}