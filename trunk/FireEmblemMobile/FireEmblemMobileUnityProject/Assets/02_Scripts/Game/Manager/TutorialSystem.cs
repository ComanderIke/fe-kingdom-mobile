using System;
using System.Collections.Generic;
using Game.Dialog;
using Game.GUI;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.Manager
{
    public enum TutorialFeed
    {
        Feed1,
        Feed2
    }

    public class TutorialSystem : MonoBehaviour, IEngineSystem
    {
        [SerializeField] private List<GameObject> tutorialPopUps;
        [SerializeField] private List<Conversation> tutorialDialogs;
        [SerializeField] private GoddessUI goddess;
        [SerializeField] private DialogueManager dialogManager;
        private StateMachine<TutorialFeed> stateMachine;

        private SelectTutorialState selectTutorialState;
        private MoveTutorialState moveTutorialState;
        private AttackTutorialState attackTutorialState;
        private EndTurnState endTurnState;
        private AttackTutorial2State attackTutorial2State;
        private UseItemState useItemState;
        private bool tutorialEnabled = false;
        public void Init()
        {
            selectTutorialState=new SelectTutorialState(goddess, dialogManager, tutorialDialogs[0]);
            moveTutorialState = new MoveTutorialState(goddess, dialogManager);
            attackTutorialState = new AttackTutorialState(goddess, dialogManager);
            attackTutorial2State = new AttackTutorial2State(goddess, dialogManager);
            endTurnState = new EndTurnState(goddess, dialogManager);
            useItemState = new UseItemState(goddess, dialogManager);
            stateMachine = new StateMachine<TutorialFeed>(selectTutorialState);
        }

        public void Update()
        {
            if (!tutorialEnabled)
                return;
            stateMachine.Update();
        }
        private void OnDisable()
        {
            Deactivate();
            //TODO if turned into a system Deactivate all events and stuff
        }
        public void Deactivate()
        {
            tutorialEnabled = false;
        }

        public void Activate()
        {
            tutorialEnabled = true;
        }
    }

    public abstract class TutorialState : GameState<TutorialFeed>
    {
         protected GoddessUI goddess;
         protected DialogueManager dialogManager;
         

         public TutorialState(GoddessUI goddess, DialogueManager dialogManager)
         {
             this.goddess = goddess;
             this.dialogManager = dialogManager;
    
         }
    }
    public class SelectTutorialState : TutorialState
    {
        [SerializeField] private List<GameObject> tutorialPopUps;
        [SerializeField] private List<Conversation> tutorialDialogs;
        private Conversation conversation;
        public override void Enter()
        {
            //Blockall Unwanted Input
        }

        public override GameState<TutorialFeed> Update()
        {
            return null;
            //Check if Selected Unit is Faction.FieldedUnits[0]
        }

        public override void Exit()
        {
            //Unblock all unwanted Input
        }

        public SelectTutorialState(GoddessUI goddess, DialogueManager dialogManager, Conversation conversion) : base(goddess, dialogManager)
        {
            this.conversation = conversion;
        }
    }
    public class MoveTutorialState : TutorialState
    {
        public override void Enter()
        {
            //Blockall Unwanted Input
        }

        public override GameState<TutorialFeed> Update()
        {
            return null;
            //Check if Faction.FieldedUnits[0] positionchanged and if it is the targetPosition
            //if moved but on the wrong position either reset the move and complain or teleport him on the right position
        }

        public override void Exit()
        {
            //Unblock all unwanted Input
        }

        public MoveTutorialState(GoddessUI goddess, DialogueManager dialogManager) : base(goddess, dialogManager)
        {
        }
    }

    public class EndTurnState: TutorialState
    {
        public override void Enter()
        {
            //Blockall Unwanted Input
        }

        public override GameState<TutorialFeed> Update()
        {
            return null;
            //Check if End Turn Pressed/New Turn Happened
        }

        public override void Exit()
        {
            //Unblock all unwanted Input
        }

        public EndTurnState(GoddessUI goddess, DialogueManager dialogManager) : base(goddess, dialogManager)
        {
        }
    }
    public class AttackTutorialState : TutorialState
    {
        public override void Enter()
        {
            //Blockall Unwanted Input
        }

        public override GameState<TutorialFeed> Update()
        {
            return null;
            //Check if Unit end turn without attacking //restore or teleport unit to the right place and attacking
            //Check if Attack Happened
            //Rig Battle RNG so battle is always the same result
        }

        public override void Exit()
        {
            //Unblock all unwanted Input
        }

        public AttackTutorialState(GoddessUI goddess, DialogueManager dialogManager) : base(goddess, dialogManager)
        {
        }
    }
    public class UseItemState : TutorialState {
        public override void Enter()
        {
            //Blockall Unwanted Input
        }

        public override GameState<TutorialFeed> Update()
        {
            return null;
            //Check if Item used
        }

        public override void Exit()
        {
            //Unblock all unwanted Input
        }

        public UseItemState(GoddessUI goddess, DialogueManager dialogManager) : base(goddess, dialogManager)
        {
        }
    }
    public class AttackTutorial2State : TutorialState
    {
        public override void Enter()
        {
            //Blockall Unwanted Input
        }

        public override GameState<TutorialFeed> Update()
        {
            return null;
            //Stop at AttackPreview to tell player he will not get counterattacked
            //explain weapon Advantage etc..
            //Check if Unit end turn without attacking //restore or teleport unit to the right place and attacking
            //Check if Attack Happened
            //Rig Battle RNG so battle is always the same result
        }

        public override void Exit()
        {
            //Unblock all unwanted Input
        }

        public AttackTutorial2State(GoddessUI goddess, DialogueManager dialogManager) : base(goddess, dialogManager)
        {
        }
    }
    
  
}