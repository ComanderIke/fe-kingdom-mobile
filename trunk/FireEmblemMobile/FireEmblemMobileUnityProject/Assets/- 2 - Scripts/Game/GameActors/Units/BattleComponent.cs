using Game.GameInput;
using Game.Mechanics.Battle;

namespace Game.GameActors.Units
{
    public class BattleComponent
    {
        public BattleStats BattleStats { get;  set; }


        public IBattleActor BattleActor;
        public BattleComponent(IBattleActor actor)
        {
            BattleActor = actor;
            BattleStats = new BattleStats(actor);
            
        }

      
        public int InflictDamage(int dmg, IBattleActor damageDealer)
        {
            BattleActor.Hp -= dmg;
            return dmg;
        }
     
    }
}