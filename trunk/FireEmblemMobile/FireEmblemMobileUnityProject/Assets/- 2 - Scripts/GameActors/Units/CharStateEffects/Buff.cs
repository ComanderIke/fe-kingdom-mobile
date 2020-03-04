namespace Assets.GameActors.Units.CharStateEffects
{
    public abstract class Buff : CharacterState
    {
        public override void Remove(Unit unit)
        {
            unit.Buffs.Remove(this);
        }
    }
}