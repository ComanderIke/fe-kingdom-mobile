using System;
using System.Collections.Generic;
using System.Linq;
using Game.AI.UnitSpecific;
using Game.GameActors.InteractableGridObjects;
using Game.GameActors.Units;
using Game.GameActors.Units.Interfaces;
using Game.GameActors.Units.Skills.Active;
using Game.GameActors.Units.Skills.Base;
using Game.Grid;
using Game.Grid.Tiles;
using Game.States.Mechanics;
using Game.Systems;
using UnityEngine;

namespace Game.AI.DecisionMaking
{
    public class DecisionMaker
    {
        private readonly IGridInformation gridInfo;
        private readonly ICombatInformation combatInfo;
        private ScoreCalculator scoreCalculator;
        public List<IAIAgent> moveOrderList;
        public List<IAIAgent> attackerList;
        public List<IAIAgent> skillUserList;
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
                minDistance = NonUnitsAsTargets(unit, minDistance);

                GetMinDistanceFromUnitTargets(unit, minDistance);
            }
        }

        private void GetMinDistanceFromUnitTargets(IAIAgent unit, int minDistance)
        {
            foreach (var enemyAgent in from faction in unit.Faction.GetOpponentFactions()
                     from enemy in faction.Units
                     where enemy.IsAlive()
                     select enemy)
            {
                var AITarget = new AITarget();
                AITarget.TargetObject = enemyAgent;
//Todo only calculate path set goals if unit is within 10 manhatten distance
//otherwise set Path to null
                var gridPos = unit.GridComponent.GridPosition.AsVectorInt();
                var enemyPos=enemyAgent.GridComponent.GridPosition.AsVectorInt();
                if (ManhattanDistance(gridPos.x, enemyPos.x, gridPos.y,enemyPos.y)<=10)
                    AITarget.Path = scoreCalculator.GetPathToEnemy(unit, enemyAgent, minDistance);
                if (AITarget.Path != null)
                    AITarget.Distance = AITarget.Path.GetLength();
                else
                {
                    AITarget.Distance = int.MaxValue;
                }

                unit.AIComponent.Targets.Add(AITarget);
                if (AITarget.Distance <= minDistance)
                {
                    minDistance = AITarget.Distance;
                    unit.AIComponent.ClosestTarget = AITarget;
                }
            }
        }
        public static int ManhattanDistance(int x1, int x2, int y1, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }
        private int NonUnitsAsTargets(IAIAgent unit, int minDistance)
        {
            foreach (var destroyable in from faction in unit.Faction.GetOpponentFactions()
                     from enemy in faction.Destroyables
                     where enemy.IsAlive()
                     select enemy)
            {
                var AITarget = new AITarget();
                AITarget.TargetObject = destroyable;

                AITarget.Path = scoreCalculator.GetPathToEnemy(unit, destroyable, minDistance);
                if (AITarget.Path != null)
                    AITarget.Distance = AITarget.Path.GetLength();
                else
                {
                    AITarget.Distance = int.MaxValue;
                }

                unit.AIComponent.Targets.Add(AITarget);
                if (AITarget.Distance <= minDistance)
                {
                    minDistance = AITarget.Distance;
                    unit.AIComponent.ClosestTarget = AITarget;
                }
            }

            return minDistance;
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
            while (moveOrderList.Count>0)
            {
                IAIAgent unit = moveOrderList.First();
                Debug.Log("First Unit in MoveOrderList: " + unit);

                if (unit.AIComponent.AIBehaviour != null &&
                    unit.AIComponent.AIBehaviour.GetState() == AIBehaviour.State.Patrol)
                {
                    var activePatrolPoint =unit.AIComponent.AIBehaviour.GetActivePatrolPoints();
                    if (unit.GridComponent.GridPosition.AsVector() == unit.AIComponent.AIBehaviour.GetActivePatrolPoints())
                    {
                        unit.AIComponent.AIBehaviour.UpdatePatrolPoint();
                        activePatrolPoint = unit.AIComponent.AIBehaviour.GetActivePatrolPoints();
                    }

                    var location = ChooseBestLocationToNextPatrolPoint(unit, activePatrolPoint);
              
                    return new AIUnitAction(location, null, UnitActionType.Wait, (Unit)unit, Vector2Int.zero);
            
                    //Get towards current patrolPoint and if reached activate it.
                }
                else if (unit.AIComponent.AIBehaviour != null &&
                         unit.AIComponent.AIBehaviour.WillStayIfNoEnemies())
                {
                    moveOrderList.Remove(unit);
                    unit.TurnStateManager.HasMoved = true;
                    //return new AIUnitAction(unit.GridComponent.GridPosition.AsVectorInt(), null, UnitActionType.Wait, unit,Vector2Int.zero);
                }
                else
                {
                    var chaseTarget = ChooseChaseTarget(unit);
                    if (chaseTarget == null)
                        Debug.Log("Chase Target = null!");
                    else
                    {
                        Vector2Int location = ChooseBestLocationToChaseTarget(unit, chaseTarget);
                        return new AIUnitAction(location, null, UnitActionType.Wait, (Unit)unit, Vector2Int.zero);
                    }
                }
            }
           

            return bestAction;
        }
        private Vector2Int ChooseBestLocationToNextPatrolPoint(IAIAgent unit, Vector2Int patrolPoint)
        {
            var moveLocs = unit.AIComponent.MovementOptions;
            int minDistance = int.MaxValue;
            Vector2Int bestloc = new Vector2Int(unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y);
            foreach (var loc in moveLocs)
            {
                var gridObject = gridInfo.GetGridObject(loc);
                if(gridObject!=null && gridObject != unit)
                    continue;
                int distanceToTarget = scoreCalculator.GetDistanceToLocation(loc, patrolPoint, unit);
                if (distanceToTarget < minDistance)
                {
                    bestloc = loc;
                    minDistance = distanceToTarget;
                }
            }
            

            return bestloc;
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
               // Debug.Log("Opponent Unit: " + unit);
                int dmg = unit.BattleComponent.BattleStats.GetTotalDamageAgainstTarget(unit);
                //Take Terrain into Account instead of using path.getLength)
                int turnRange = int.MaxValue;
                if(agent.MovementRange!=0)
                    turnRange = agent.AIComponent.GetTarget(unit).Distance / agent.MovementRange;
                //Debug.Log("TurnRange: " + turnRange);
                var prioValue = dmg - 2 * turnRange;
                //Debug.Log("PrioValue: " + prioValue);
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

            if (moveOrderList.Count() == 0)
                CreateMoveOrderList(units);
            CreateAttackerList(units);
            CreateSkillUserList(units);
            if (skillUserList.Count != 0)
            {
                CalculateOptimalTilesToUseSkills();
                ChooseBestSkillTargets();
                var bestSkillUser = ChooseBestSkillUser();
                // bestSkillUser.AIComponent.AIBehaviour.UpdateState(bestSkillUser, false);
          
                    return CreateUseSkillAction(bestSkillUser);
            }
            else if (attackerList.Count != 0)
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
            if(skillUserList.Contains(unit))
                skillUserList.Remove(unit);
        }
        private AIUnitAction CreateAttackAction(IAIAgent attacker)
        {
            if(attacker.AIComponent.AIBehaviour!=null)
                attacker.AIComponent.AIBehaviour.AttackTriggered();
            return new AIUnitAction(attacker.AIComponent.BestAttackTarget.OptimalAttackPos,attacker.AIComponent.BestAttackTarget.Target, UnitActionType.Attack, attacker, Vector2Int.zero );

        }
        private AIUnitAction CreateUseSkillAction(IAIAgent attacker)
        {
            return new AIUnitAction(attacker.AIComponent.BestSkillTarget==null?attacker.GridComponent.GridPosition.AsVectorInt():attacker.AIComponent.BestSkillTarget.OptimalAttackPos,attacker.AIComponent.BestSkillTarget==null?null:attacker.AIComponent.BestSkillTarget.Target, UnitActionType.UseSkill, attacker , attacker.AIComponent.BestSkillTarget==null?
                attacker.GridComponent.GridPosition.AsVectorInt():
                attacker.AIComponent.BestSkillTarget.OptimalCastPos);

        }
        private void ChooseBestAttackTargets()
        {
            foreach (var attacker in attackerList)
            {
                attacker.AIComponent.AttackableTargets.Sort(new AttackTargetComparer());

                attacker.AIComponent.BestAttackTarget = attacker.AIComponent.AttackableTargets.Last();
            }
        }
        private void ChooseBestSkillTargets()
        {
            foreach (var attacker in skillUserList)
            {
                if(attacker.AIComponent.SkillTargets.Count==0)
                    continue;
                attacker.AIComponent.SkillTargets.Sort(new AttackTargetComparer());
                attacker.AIComponent.BestSkillTarget = attacker.AIComponent.SkillTargets.Last();
            }
        }

        private IAIAgent ChooseBestAttacker()
        {
            attackerList.Sort(new AttackerComparer());
            return attackerList.Last();
        }
        private IAIAgent ChooseBestSkillUser()
        {
            skillUserList.Sort(new SkillUserComparer());
            return skillUserList.Last();
        }

        private void CalculateOptimalTilesToUseSkills()
        {
            foreach (var attacker in skillUserList)
            {
                Unit user = (Unit)attacker;
                // Debug.Log("attacker in List: " +attacker);
                // Debug.Log("AttackTargetCount: " +attacker.AIComponent.AttackableTargets.Count());
                
                var skill = user.AIComponent.AIBehaviour.GetSkillToUse();
                var skillMixin = skill.FirstActiveMixin;
                foreach (var target in attacker.AIComponent.SkillTargets)
                {
                    var tiles = target.AttackableTiles;
                    var combatInfos = new List<ISkillResult>();
                    Unit targetUnit = (Unit)target.Target;
                    if (target.Target == null)
                    {
                        // Debug.Log("AttackableTargetTarget is null!" + target);
                        continue;
                    }
                    //check for each tile how many targets get hit and how much damage is done as a whole?
                    // also tile bonuses ofc (not important for charge skill though)
                    //how to get damage done?
                    foreach (var tile in tiles)
                    {
                        if (skillMixin is PositionTargetSkillMixin ptsm)
                        {
                            foreach (var castTarget in ptsm.GetCastTargets(user, gridInfo.GetTiles(), skill.level,
                                         tile.x, tile.y))
                            {
                                var tmpDirection = new Vector2(castTarget.x-tile.x,
                                     castTarget.y-tile.y).normalized;
                                var direction = new Vector2Int((int)tmpDirection.x, (int)tmpDirection.y);
                                int damageRatio = 0;
                                int targetCount = 0;

                                foreach (var skillTarget in ptsm.GetAllTargets((Unit)attacker, gridInfo.GetTiles(),
                                             tile.x,
                                             tile.y, direction))
                                {

                                    var damageMixin = ptsm.GetDamageMixin();
                                    if (damageMixin != null)
                                    {
                                        
                                        int damage = damageMixin.CalculateDamage(user, (Unit)skillTarget, skill.level);
                                        var damageType = damageMixin.GetDamageType();
                                        damageRatio +=
                                            BattleHelper.GetDamageAgainst((Unit)skillTarget, damage, damageType);
                                    }

                                    targetCount++;
                                }

                                ISkillResult combatInfo = new SkillResult((Unit)attacker,
                                    gridInfo.GetTile(tile.x, tile.y), gridInfo.GetTile(castTarget.x, castTarget.y),damageRatio, targetCount);
                                combatInfos.Add(
                                    combatInfo); //combatInfo.GetCombatResultAtAttackLocation((IBattleActor)attacker,target.Target, tile));
                            }
                        }

                        else if (skillMixin is SelfTargetSkillMixin stsm)
                        {
                            ISkillResult combatInfo = new SkillResult((Unit)attacker,
                                gridInfo.GetTile(tile.x, tile.y), gridInfo.GetTile(tile.x, tile.y),0, 0);
                            combatInfos.Add(combatInfo);
                        }

                        
                    }
                    //make new ResultComparer based on target number and damage
                    combatInfos.Sort(new CombatResultComparer());
                    if (combatInfos.Count == 0)
                    {
                        Debug.LogError("Combat Infos is NULL TODO DEBUG should not happen");
                    }
                    target.OptimalAttackPos =combatInfos.Last().GetAttackPosition() ;
                    target.OptimalCastPos = combatInfos.Last().GetCastPosition();
                    target.CombatResult = combatInfos.Last();
                }
            }
        }

        public class BattleHelper
        {
            public static int GetDamageAgainst(Unit damageReceiver, int damage, DamageType damageType)
            {
                int defense = 0;
                switch (damageType)
                {
                    case DamageType.Faith:defense=damageReceiver.BattleComponent.BattleStats.GetFaithResistance();
                        break;
                    case DamageType.Magic:defense=damageReceiver.BattleComponent.BattleStats.GetFaithResistance();
                        break;
                    case DamageType.Physical:defense=damageReceiver.BattleComponent.BattleStats.GetPhysicalResistance();
                        break;
                }

                Debug.Log("Defense: " + defense);
                return damage - defense;
            }
        }
        public class SkillResult : ISkillResult
        {
            
            private Unit user;
            private Tile useSkillPosition;
            private Skill skill;
            private int damageRatio;
            private Tile castTile;
            private int targetCount;
            public SkillResult(Unit user, Tile tile,Tile castTile, int damageRatio, int targetCount)
            {
                AttackResult = AttackResult.Win;
                useSkillPosition = tile;
                this.user = user;
                this.skill = user.SkillManager.ActiveSkills[0];
                this.damageRatio = damageRatio;
                this.castTile = castTile;
                this.targetCount = targetCount;
            }
            public Vector2Int GetAttackPosition()
            {
                return new Vector2Int(useSkillPosition.X, useSkillPosition.Y);
            }
            public Vector2Int GetCastPosition()
            {
                return new Vector2Int(castTile.X, castTile.Y);
            }

            public AttackResult AttackResult { get; set; }
            public int GetDamageRatio()
            {
                
                return damageRatio;
            }
            public int GetTargetCount()
            {
                
                return targetCount;
            }

            public int GetTileDefenseBonuses()
            {
                return useSkillPosition.TileData.defenseBonus;
            }

            public int GetTileSpeedBonuses()
            {
                return useSkillPosition.TileData.speedMalus;
            }
            public int GetTileAvoidBonuses()
            {
                return useSkillPosition.TileData.avoBonus;
            }
        }
        public void CalculateOptimalTilesToAttack()
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

       
       
        private void CreateSkillUserList(IEnumerable<IAIAgent> units)
        {
            skillUserList.Clear();
            foreach (var unit in units)
            {
                if (unit.AIComponent.AIBehaviour != null &&
                    unit.AIComponent.AIBehaviour.GetState() == AIBehaviour.State.UseSkill)
                {
                    Unit u = (Unit)unit;
                
                    if(!unit.AIComponent.AIBehaviour.CanUseSkill())
                        continue;
                    var firstActiveSkill = u.AIComponent.AIBehaviour.GetSkillToUse();
                    if (firstActiveSkill == null)
                        continue;
                    if (firstActiveSkill.FirstActiveMixin is SelfTargetSkillMixin)
                    {
                        skillUserList.Add(unit);
                    }
                    else
                    {
                        unit.AIComponent.SkillTargets = GetSkillTargets(unit);

                        if (unit.AIComponent.SkillTargets.Count() != 0)
                            skillUserList.Add(unit);
                    }
                   
                }
            }
            Debug.Log("SkillUserListCount: "+skillUserList.Count);
        }
        private void CreateAttackerList(IEnumerable<IAIAgent> units)
        {
            attackerList.Clear();
            foreach (var unit in units)
            {
                unit.AIComponent.AttackableTargets=GetAttackTargets(gridInfo, unit);
                if(unit.AIComponent.AIBehaviour!=null)
                    unit.AIComponent.AIBehaviour.UpdateState(unit, unit.AIComponent.AttackableTargets.Count()!=0 );
                if (unit.AIComponent.AttackableTargets.Count() != 0)
                {
                    if (unit.AIComponent.AIBehaviour == null ||
                        unit.AIComponent.AIBehaviour.WillAttackOnTargetInRange())
                    {
                        attackerList.Add(unit);
                    }
                        
                }
                    
            }
        }

        public static List<AIAttackTarget> GetAttackTargets(IGridInformation gridInfo, IAIAgent unit)
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
        List<AIAttackTarget> GetSkillTargets(IAIAgent unit)
        {
            var skillTargetList = new List<AIAttackTarget>();
            var targetList = new List<IAttackableTarget>();
            foreach (var moveOption in unit.AIComponent.MovementOptions)
            {
                if (gridInfo.GetGridObject(moveOption) != null && gridInfo.GetGridObject(moveOption) != unit)
                    continue;
                var targets = gridInfo.GetSkillTargetsAtPosition(unit, unit.AIComponent.AIBehaviour.GetSkillToUse(), moveOption.x, moveOption.y);
                foreach (var target in targets)
                {
                    var attackTarget = new AIAttackTarget(target);
                    attackTarget.AttackableTiles.Add(moveOption);
                    if (!targetList.Contains(target))
                    {
                        skillTargetList.Add(attackTarget);
                        targetList.Add(target);
                    }
                    else
                    {
                        GetTargetFromUnit(skillTargetList, (Unit)target).AddAttackableTile(moveOption);
                    }
                }
            }

            return skillTargetList;
        }

        public AIAttackTarget GetTargetFromUnit(List<AIAttackTarget> targets, Unit unit)
        {
            foreach (var target in targets)
            {
                if (target.Target.Equals(unit))
                    return target;
            }

            return null;
        }

        public void InitMoveOptions(IEnumerable<IAIAgent> units)
        {
            foreach (var unit in units)
            {
                if(unit.AIComponent.AIBehaviour==null ||unit.AIComponent.AIBehaviour.WillMoveIfAble())
                    unit.AIComponent.MovementOptions=gridInfo.GetMoveLocations(unit);
                else
                {
                    unit.AIComponent.MovementOptions = new List<Vector2Int>() { unit.GridComponent.GridPosition.AsVectorInt()};
                }
            }
        }
        public void InitTurnData(IEnumerable<IAIAgent> units)
        {
            //Debug.Log("InitTurnData");
            InitTargets(units);
            moveOrderList = CreateMoveOrderList(units);
            InitMoveOptions(units);
            attackerList = new List<IAIAgent>();
            skillUserList = new List<IAIAgent>();
            UpdateAIBehaviours(units);
            CreateAttackerList(units);
            CreateSkillUserList(units);
            //Debug.Log("moveOrderlistCount: "+moveOrderList.Count());
        }

        void UpdateAIBehaviours(IEnumerable<IAIAgent> units)
        {
            foreach (var unit in units)
            {
                if (unit.AIComponent.AIBehaviour != null)
                {
                    unit.AIComponent.AIBehaviour.UpdateState(unit, false);
                }
            }
        }
    }
}