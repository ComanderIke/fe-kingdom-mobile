using Game.GameActors.Units.Skills;

namespace Game.Mechanics
{
    public interface ISkillClickedReceiver
    {
        void SkillClicked(Skill skill);
    }
}