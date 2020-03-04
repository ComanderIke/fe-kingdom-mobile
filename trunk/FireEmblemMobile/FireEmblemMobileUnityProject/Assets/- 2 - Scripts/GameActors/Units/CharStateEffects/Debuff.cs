namespace Assets.GameActors.Units.CharStateEffects
{
    public abstract class Debuff : CharacterState
    {
        public override void Remove(Unit unit)
        {
            unit.Debuffs.Remove(this);
        }
    }
}