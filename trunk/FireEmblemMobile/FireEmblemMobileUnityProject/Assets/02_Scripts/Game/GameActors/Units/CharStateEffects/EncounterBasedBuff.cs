using LostGrace;

namespace Game.GameActors.Units.CharStateEffects
{
    public abstract class EncounterBasedBuff : ITemporaryEffect
    {
        public int duration;
        public EncounterBasedBuff(int duration)
        {
            this.duration = duration;
        }
        public void DecreaseDuration()
        {
            duration--;
        }

        public int GetDuration()
        {
            return duration;
        }

        public abstract void Apply(Unit unit);

    }
}