using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Mechanics;
using UnityEngine;

namespace Game.AI
{
    public class DecisionMaker
    {
        private readonly IGridInformation gridInfo;
        private readonly ICombatInformation combatInfo;
        private ScoreCalculator scoreCalculator;
        public List<IAIAgent> moveOrderList;
        public List<IAIAgent> attackerList;
        public List<Vector2Int> blockedTiles;


        public DecisionMaker(IGridInformation gridInfo,ICombatInformation combatInfo, IPathFinder pathFinder)
        {
            this.gridInfo = gridInfo;
            this.combatInfo = combatInfo;
            scoreCalculator = new ScoreCalculator(pathFinder);
        }


        public void InitTargets(IEnumerable<IAIAgent> units)
        {
           
            foreach (var unit in units)
            {
                var minDistance = int.MaxValue;
                unit.AIComponent.ClosestTarget = null;
                unit.AIComponent.Targets.Clear();
                foreach (var destroyable in from faction in unit.Faction.GetOpponentFactions()
                         from enemy in faction.Destroyables
                         where enemy.IsAlive()
                         select enemy)
                {
                    var AITarget = new AITarget();
                    AITarget.TargetObject = destroyable;

                    AITarget.Path = scoreCalculator.GetPathToEnemy(unit, destroyable);
                    if(AITarget.Path!=null)
                        AITarget.Distance = AITarget.Path.GetLength();
                    else
                    {
                        AITarget.Distance = int.MaxValue;
                    }
                    unit.AIComponent.Targets.Add(AITarget);
                    if ( AITarget.Distance <= minDistance)
                    {
                        minDistance = AITarget.Distance;
                        unit.AIComponent.ClosestTarget = AITarget;
                    }
                }

                foreach (var enemyAgent in from faction in unit.Faction.GetOpponentFactions()
                         from enemy in faction.Units
                         where enemy.IsAlive()
                         select enemy)
                {
                    var AITarget = new AITarget();
                    AITarget.TargetObject = enemyAgent;

                    AITarget.Path = scoreCalculator.GetPathToEnemy(unit, enemyAgent);
                    if(AITarget.Path!=null)
                        AITarget.Distance = AITarget.Path.GetLength();
                    else
                    {
                        AITarget.Distance = int.MaxValue;
                    }
                    unit.AIComponent.Targets.Add(AITarget);
                    if ( AITarget.Distance <= minDistance)
                    {
                        minDistance = AITarget.Distance;
                        unit.AIComponent.ClosestTarget = AITarget;
                    }
                }
            }
        }

        private List<IAIAgent> CreateMoveOrderList(IEnumerable<IAIAgent> units)
        {
            var moveOrderList = new List<IAIAgent>();
            foreach (var unit in units)
            {
                moveOrderList.Add(unit);
            }

            moveOrderList.Sort(new UnitComparer());
            moveOrderList.Reverse();
            return moveOrderList;
        }

        public AIUnitAction ChooseBestMovementAction()
        {
            AIUnitAction bestAction = new AIUnitAction();
            Debug.Log(moveOrderList.Count);
            moveOrderList.RemoveAll(unit => unit.TurnStateManager.HasMoved);
            IAIAgent unit = moveOrderList.First();
            Debug.Log("First Unit in MoveOrderList: " + unit);
            var chaseTarget = ChooseChaseTarget(unit);
            if (chaseTarget == null)
                Debug.Log("Chase Target = null!");
            else
            {
                Vector2Int location = ChooseBestLocationToChaseTarget(unit, chaseTarget);
                return new AIUnitAction(location, null, UnitActionType.Wait, (Unit)unit);
            }

            return bestAction;
        }

        private Vector2Int ChooseBestLocationToChaseTarget(IAIAgent unit, AITarget chaseTarget)
        {
            var moveLocs = unit.AIComponent.MovementOptions;
            int minDistance = int.MaxValue;
            Vector2Int bestloc = new Vector2Int(unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y);
            foreach (var loc in moveLocs)
            {
                var gridObject = gridInfo.GetGridObject(loc);
                if(gridObject!=null && gridObject != unit)
                    continue;
                int distanceToTarget = scoreCalculator.GetDistanceToEnemy(loc, chaseTarget.TargetObject, unit);
                if (distanceToTarget < minDistance)
                {
                    bestloc = loc;
                    minDistance = distanceToTarget;
                }
            }

            return bestloc;
        }

        private AITarget ChooseChaseTarget(IAIAgent agent)
        {
            int MaxPrioValue = int.MinValue;
            AITarget target = null;
            foreach (var unit in from faction in agent.Faction.GetOpponentFactions()
                     from unit in faction.Units
                     where unit.IsAlive()
                     select unit)
            {
                Debug.Log("Opponent Unit: " + unit);
                int dmg = unit.BattleComponent.BattleStats.GetTotalDamageAgainstTarget(unit);
                //Take Terrain into Account instead of using path.getLength)
                int turnRange = int.MaxValue;
                if(agent.MovementRange!=0)
                    turnRange = agent.AIComponent.GetTarget(unit).Distance / agent.MovementRange;
                Debug.Log("TurnRange: " + turnRange);
                var prioValue = dmg - 2 * turnRange;
                Debug.Log("PrioValue: " + prioValue);
                if (prioValue > MaxPrioValue)
                {
                    MaxPrioValue = prioValue;
                    target = agent.AIComponent.GetTarget(unit);
                }
            }

            return target;
        }
        public AIUnitAction ChooseBestAction(IEnumerable<IAIAgent> units)
        {

            CreateAttackerList(units);
            if (attackerList.Count != 0)
            {
                CalculateOptimalTilesToAttack();
                ChooseBestAttackTargets();
                var bestAttacker = ChooseBestAttacker();
                //Debug.Log("Best Atttacker: "+bestAttacker);
               // Debug.Log("Best Target: "+bestAttacker.AIComponent.BestAttackTarget.Target+" "+bestAttacker.AIComponent.AttackableTargets.Last().Target);
                return CreateAttackAction(bestAttacker);
            }
            return ChooseBestMovementAction();
            //return bestAction;
        }

        public void RemoveUnitFromListPool(IAIAgent unit)
        {
            if(moveOrderList.Contains(unit))
                moveOrderList.Remove(unit);
            if(attackerList.Contains(unit))
                attackerList.Remove(unit);
        }
        private AIUnitAction CreateAttackAction(IAIAgent attacker)
        {
            return new AIUnitAction(attacker.AIComponent.BestAttackTarget.OptimalAttackPos,attacker.AIComponent.BestAttackTarget.Target, UnitActionType.Attack, attacker );

        }
        private void ChooseBestAttackTargets()
        {
            foreach (var attacker in attackerList)
            {
                attacker.AIComponent.AttackableTargets.Sort(new AttackTargetComparer());

                attacker.AIComponent.BestAttackTarget = attacker.AIComponent.AttackableTargets.Last();
            }
        }

        private IAIAgent ChooseBestAttacker()
        {
            attackerList.Sort(new AttackerComparer());
            return attackerList.Last();
        }

        private void CalculateOptimalTilesToAttack()
        {
            foreach (var attacker in attackerList)
            {
               // Debug.Log("attacker in List: " +attacker);
               // Debug.Log("AttackTargetCount: " +attacker.AIComponent.AttackableTargets.Count());
                foreach (var target in attacker.AIComponent.AttackableTargets)
                {
                    var tiles = target.AttackableTiles;
                    var combatInfos = new List<ICombatResult>();
                    if (target.Target == null)
                    {
                       // Debug.Log("AttackableTargetTarget is null!" + target);
                        continue;
                    }
                    foreach (var tile in tiles)
                    {
                        combatInfos.Add(combatInfo.GetCombatResultAtAttackLocation((IBattleActor)attacker,target.Target, tile));
                    }
                    combatInfos.Sort(new CombatResultComparer());
                    target.OptimalAttackPos =combatInfos.Last().GetAttackPosition() ;
                    target.CombatResult = combatInfos.Last();
                }
            }
        }

       
       

        private void CreateAttackerList(IEnumerable<IAIAgent> units)
        {
            attackerList.Clear();
            foreach (var unit in units)
            {
                unit.AIComponent.AttackableTargets=GetAttackTargets(unit);
                
                if(unit.AIComponent.AttackableTargets.Count()!=0)
                    attackerList.Add(unit);
            }
        }

        List<AIAttackTarget> GetAttackTargets(IAIAgent unit)
        {
            var attackTargetList = new List<AIAttackTarget>();
            var targetList = new List<IAttackableTarget>();
            foreach (var moveOption in unit.AIComponent.MovementOptions)
            {
                if (gridInfo.GetGridObject(moveOption) != null && gridInfo.GetGridObject(moveOption) != unit)
                    continue;
                var targets = gridInfo.GetAttackTargetsAtPosition(unit, moveOption.x, moveOption.y);
                foreach (var target in targets)
                {
                    var attackTarget = new AIAttackTarget(target);
                    attackTarget.AttackableTiles.Add(moveOption);
                    if (!targetList.Contains(target))
                    {
                        attackTargetList.Add(attackTarget);
                        targetList.Add(target);
                    }
                }
            }

            return attackTargetList;
        }

        public void InitMoveOptions(IEnumerable<IAIAgent> units)
        {
            foreach (var unit in units)
            {
               unit.AIComponent.MovementOptions=gridInfo.GetMoveLocations(unit);
            }
        }
        public void InitTurnData(IEnumerable<IAIAgent> units)
        {
            //Debug.Log("InitTurnData");
            InitTargets(units);
            moveOrderList = CreateMoveOrderList(units);
            InitMoveOptions(units);
            attackerList = new List<IAIAgent>();
            CreateAttackerList(units);
            //Debug.Log("moveOrderlistCount: "+moveOrderList.Count());
        }
    }
}