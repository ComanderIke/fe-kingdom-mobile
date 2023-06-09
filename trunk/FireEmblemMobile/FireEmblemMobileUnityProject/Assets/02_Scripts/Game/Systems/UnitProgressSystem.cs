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
        public IExpRenderer expRenderer;
        public ExpBarController ExpBarController;
        private bool finished = false;
        private SkillSystem skillSystem;
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
            
            skillSystem = ServiceProvider.Instance.GetSystem<SkillSystem>();
         
        }

        void OnLevelUp(Unit u)
        {
            //check if Exp Animation Finished
            //check if Couroutine is active or flag finished or whatever
            AnimationQueue.Add(()=>LevelUp(u));
            LearnNewSkill(u);
        }

        public void LearnNewSkill(Unit u)
        {
            skillSystem.LearnNewSkill(u);
        }

        void Expgained(Unit unit, int exp)
        {
      
            AnimationQueue.Add(() =>
            {
                Debug.Log("Show from AnimationQueue"+unit.name);
                ExpBarController.Show(unit.ExperienceManager.Exp);
                ExpBarController.UpdateWithAnimatedTextOnly(exp);
                ExpBarController.onFinished -= FinishedExpAnimation;
                ExpBarController.onFinished += FinishedExpAnimation;
            });
           
        }
        void FinishedExpAnimation()
        {
            ExpBarController.Hide();
            AnimationQueue.OnAnimationEnded?.Invoke();
        }
        public void Deactivate()
        {
            // foreach (var unit in units)
            // {
            //     unit.OnLevelUp -= LevelUp;
            // }
            Unit.OnExpGained -= Expgained;
            Unit.OnLevelUp -= OnLevelUp;
        }

        public void Activate()
        {
            Unit.OnExpGained += Expgained;
            Unit.OnLevelUp += OnLevelUp;
            // foreach (var unit in units)
            // {
            //     unit.OnLevelUp -= LevelUp;
            //     unit.OnLevelUp += LevelUp;
            // }
        }


        public void LevelUp(Unit unit)
        {
        
            int[] statIncreases = CalculateStatIncreases(unit.Growths.AsArray());

            if (levelUpRenderer != null)
            {
                levelUpRenderer.UpdateValues(unit.name, unit.visuals.CharacterSpriteSet.FaceSprite,unit.ExperienceManager.Level - 1, unit.ExperienceManager.Level,
                    unit.Stats.BaseAttributes.AsArray(), statIncreases);
                
                levelUpRenderer.Play();
                levelUpRenderer.OnFinished -= LevelUpFinished;
                levelUpRenderer.OnFinished += LevelUpFinished;
            }

            if (unit.ExperienceManager.Level % 2 == 0)
            {
                unit.SkillManager.SkillPoints += 1;
            }

            unit.Stats.BaseAttributes.Update(statIncreases);
           
        }

        void LevelUpFinished()
        {
            AnimationQueue.OnAnimationEnded?.Invoke();
        }

        void DistributeBattleExperienceFinished()
        {
            AnimationQueue.OnAllAnimationsEnded -= DistributeBattleExperienceFinished;
            finished = true;
        }
        public void DistributeExperience(IBattleActor opponent, IBattleActor expReceiver)
        {
            AnimationQueue.OnAllAnimationsEnded += DistributeBattleExperienceFinished;
            finished = false;
            int exp=0;
            if (expReceiver.IsAlive() && expReceiver.IsPlayerControlled())
            {
                exp = CalculateExperiencePoints(expReceiver, opponent);
                if (exp != 0)
                {
                    //var expRenderer = ((Unit)defender).visuals.UnitCharacterCircleUI.GetExpRenderer();
                    ServiceProvider.Instance.GetSystem<UiSystem>()?.SelectedCharacter((Unit)expReceiver);
                   
                    Debug.Log("Calculated Exp: "+exp);
                   // expRenderer.Play((Unit)expReceiver, pos, exp);

                    expReceiver.ExperienceManager.AddExp(exp);
                    
                    //expRenderer.OnFinished += FinishedOnce;
                    
                    
                   
                    return;
                }
                
            }
            // FinishedOnce();
        }
        
        
        // void FinishedOnce()
        // {
        //     ExpBarController.Hide();
        //     finished = true;
        //     onFinished?.Invoke();
        // }
        private int CalculateExperiencePoints(IBattleActor expReceiver, IBattleActor enemyFought)
        {
 
            int levelDifference = expReceiver.ExperienceManager.Level - enemyFought.ExperienceManager.Level;
            bool killEXP = !enemyFought.IsAlive();
            int expLeft = enemyFought.ExperienceManager.expLeftToDrain;
            int maxEXPDrain = enemyFought.ExperienceManager.drainableExp;
  
            float chipExpPercent = 0.2f;
            float killExpPercent = 1.0f;
           
            int exp =(int)( killEXP ? killExpPercent * maxEXPDrain : chipExpPercent * maxEXPDrain);
    
            if(exp > expLeft)
            {
                exp = expLeft;
            }
            
            enemyFought.ExperienceManager.expLeftToDrain-= exp;
            if (enemyFought.ExperienceManager.expLeftToDrain < 0)
                enemyFought.ExperienceManager.expLeftToDrain = 0;
      
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