namespace Game.GameActors.Units.Skills.Passive
{
    public interface IBattleEventListener
    {
        public void Activate(Unit attacker, Unit defender);
        public void Deactivate(Unit attacker, Unit defender);
    }
}