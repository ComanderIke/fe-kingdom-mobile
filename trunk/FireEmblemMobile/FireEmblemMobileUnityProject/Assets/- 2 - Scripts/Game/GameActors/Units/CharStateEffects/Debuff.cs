namespace Game.GameActors.Units.CharStateEffects
{
    public abstract class Debuff : CharacterState
    {
        public override void Remove(Unit unit)
        {
            unit.RemoveDebuff(this);
        }
    }
}