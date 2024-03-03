using Game.GameActors.Units.Skills.Base;

namespace Game.States
{
    public interface ISkillClickedReceiver
    {
        void SkillClicked(SkillBp skillBp);
    }
}