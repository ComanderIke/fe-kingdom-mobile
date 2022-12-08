using System;
using System.Collections.Generic;
using System.Linq;
using Game.AI;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using Game.GUI;
using Game.Manager;
using Game.States;
using Game.WorldMapStuff.Model;
using GameEngine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.PlayerLoop;
using Utility;

namespace Game.Mechanics
{
    public interface IExpRenderer
    {
        void Play(Unit unit, Vector3 startPos, int exp);
        event Action OnFinished;
    }
    public class UnitProgressSystem : IEngineSystem
    {
        public ILevelUpRenderer levelUpRenderer;//injected
        private List<Unit> units;
        private List<Faction> factions;
        public event Action onFinished;
        public IExpRenderer expRenderer;
        private bool finished = false;
        public UnitProgressSystem(Party party)
        {
            factions = new List<Faction>();
            units = new List<Unit>();
            foreach(var unit in party.members)
                MemberAdded(unit);
            party.onMemberAdded -= MemberAdded;
            party.onMemberAdded += MemberAdded;
        }
        public UnitProgressSystem(FactionManager fm)
        {
            factions = new List<Faction>();
            units = new List<Unit>();
            foreach(var faction in fm.Factions)
                AddFaction(faction);
        }
        private void AddFaction(Faction faction)
        {
            factions.Add(faction);
            foreach (var unit in faction.Units)
            {
                MemberAdded(unit);
            }
            faction.OnAddUnit -= MemberAdded;
            faction.OnAddUnit += MemberAdded;
        }
        private void MemberAdded(Unit u)
        {
            // u.OnLevelUp -= LevelUp;
            // u.OnLevelUp += LevelUp;
            units.Add(u);
          

        }

        
        
        public void Init()
        {
            
        }

        public void Deactivate()
        {
            // foreach (var unit in units)
            // {
            //     unit.OnLevelUp -= LevelUp;
            // }
        }

        public void Activate()
        {
            // foreach (var unit in units)
            // {
            //     unit.OnLevelUp -= LevelUp;
            //     unit.OnLevelUp += LevelUp;
            // }
        }


        public void LevelUp(Unit unit)
        {
            Debug.Log("LevelUp System Called!");
            int[] statIncreases = CalculateStatIncreases(unit.Growths.AsArray());

            if (levelUpRenderer != null)
            {
                levelUpRenderer.UpdateValues(unit.name, unit.visuals.CharacterSpriteSet.FaceSprite,unit.ExperienceManager.Level - 1, unit.ExperienceManager.Level,
                    unit.Stats.BaseAttributes.AsArray(), statIncreases);
                Debug.Log("Add LevelUpAnimation!");
                levelUpRenderer.Play();
                levelUpRenderer.OnFinished -= FinishedOnce;
                levelUpRenderer.OnFinished += FinishedOnce;
            }

            if (unit.ExperienceManager.Level % 2 == 0)
            {
                unit.SkillManager.SkillPoints += 1;
            }

            unit.Stats.BaseAttributes.Update(statIncreases);
           
        }

        
        public void DistributeExperience(IBattleActor attacker, IBattleActor defender)
        {
            finished = false;
            int exp=0;
            if (defender.IsAlive() && defender.IsPlayerControlled())
            {
                exp = CalculateExperiencePoints(defender, attacker);
                if (exp != 0)
                {
                    //var expRenderer = ((Unit)defender).visuals.UnitCharacterCircleUI.GetExpRenderer();
                    ServiceProvider.Instance.GetSystem<UiSystem>()?.SelectedCharacter((Unit)defender);
                    Vector3 pos = new Vector3();
                    if (defender.BattleGO != null)//In BattleAnimation use this
                    {
                        pos = defender.BattleGO.GameObject.transform.position;
                        var expController = defender.BattleGO.GetExpRenderer();
                        expController.Show(defender.ExperienceManager.Exp);
                        expController.UpdateWithAnimatedParticles(exp);
                    }
                    else if (defender.GameTransformManager != null)//Map Animations use this
                    {
                        pos = defender.GameTransformManager.Transform.position + new Vector3(0.5f, 0.5f, 0);
                    }
                    Debug.Log("Calculated Exp: "+exp);
                    expRenderer.Play((Unit)defender, pos, exp);

                    defender.ExperienceManager.AddExp(exp);
                    
                    expRenderer.OnFinished += FinishedOnce;
                    
                    
                   
                    return;
                }
                
            }
            FinishedOnce();
        }
        
        
        void FinishedOnce()
        {
            finished = true;
            onFinished?.Invoke();
        }
        private int CalculateExperiencePoints(IBattleActor expReceiver, IBattleActor enemyFought)
        {
 
            int levelDifference = expReceiver.ExperienceManager.Level - enemyFought.ExperienceManager.Level;
            bool killEXP = !enemyFought.IsAlive();
            int expLeft = enemyFought.ExperienceManager.ExpLeftToDrain;
            int maxEXPDrain = ExperienceManager.MAX_EXP_TO_DRAIN;
  
            float chipExpPercent = 0.2f;
            float killExpPercent = 1.0f;
            int exp =(int)( killEXP == true ? killExpPercent * maxEXPDrain : chipExpPercent * maxEXPDrain);
      
            if(exp > expLeft)
            {
                exp = expLeft;
            }
            if(!killEXP&&expLeft-exp < maxEXPDrain / 5)
            {
                exp = expLeft - maxEXPDrain / 5;
            }
            enemyFought.ExperienceManager.ExpLeftToDrain -= exp;
            if (enemyFought.ExperienceManager.ExpLeftToDrain < 0)
                enemyFought.ExperienceManager.ExpLeftToDrain = 0;
            if (levelDifference < 0)
            {
                exp = (int)(exp * (1f + ((levelDifference * -1) / 10f)));
            }
            if (levelDifference >= 0)
            {
                exp = (int)(exp * (1f - ((levelDifference) / 10f)));
            }
            if (exp <= 0)
                exp = 0;
            Debug.Log("EXP : " +exp);
            return exp;
        }
        private int[] CalculateStatIncreases(int[] growths)
        {
            int[] increaseAmount = new int[growths.Length];
            bool atleast1 = false;
            while (!atleast1)
            {
                for (int i = 0; i < growths.Length; i++)
                {
                    increaseAmount[i] = Method(growths[i]);
                    if (increaseAmount[i] > 0)
                        atleast1 = true;
                }
            }

            return increaseAmount;
        }
        private int Method(int Growth)
        {
            int rngNumber = (int) (UnityEngine.Random.value * 100f);
            if (Growth > 100)
            {
                return 1 + Method(Growth - 100);
            }

            if (rngNumber <= Growth)
            {
                return 1;
            }

            return 0;
        }


        public bool IsFinished()
        {
            return finished;
        }
        
        public void DoLevelUp(Unit unit)
        {
            finished = false;
            LevelUp(unit);
        }
    }
}