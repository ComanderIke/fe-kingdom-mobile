using Assets.Scripts.AI.AttackPatterns;
using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Monsters;
using Assets.Scripts.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Events
{
    public class EventContainer
    {

        #region Commands
        public delegate void CommandFinished();
        public static CommandFinished commandFinished;

        public delegate void ReactionFinished();
        public static ReactionFinished reactionFinished;

        public delegate void AllCommandsFinished();
        public static AllCommandsFinished allCommandsFinished;

        public delegate void Undo();
        public static Undo undo;
        #endregion

        #region TurnManagement
        public delegate void EndTurn();
        public static EndTurn endTurn;
        public delegate void StartTurn();
        public static StartTurn startTurn;
        #endregion

        #region AttackPatterns
        public delegate void ContinuePressed();
        public static ContinuePressed continuePressed;

        public delegate void AttackPatternUsed(LivingObject user, AttackPattern pattern);
        public static AttackPatternUsed attackPatternUsed;

       /* public delegate void StampedeUsed(LivingObject user, AttackPattern pattern);
        public static StampedeUsed stampedeUsed;

        public delegate void HowlUsed(LivingObject user, AttackPattern pattern);
        public static HowlUsed howlUsed;*/
        #endregion

        #region Fight
        public delegate void AttackerDmgChanged(int dmg);
        public static AttackerDmgChanged attackerDmgChanged;
        public delegate void AttackerHitChanged(int hit);
        public static AttackerHitChanged attackerHitChanged;
        public delegate void StartAttack(AttackType attackType = null);
        public static StartAttack startAttack;

        #endregion

        #region UI

        public delegate void AttackButtonCLicked();
        public static AttackButtonCLicked attacktButtonCLicked;
        public delegate void FrontalAttackAnimationEnd();
        public static FrontalAttackAnimationEnd frontalAttackAnimationEnd;
        
        public delegate void AttackUIVisible( bool visible);
        public static AttackUIVisible attackUIVisible;

        public delegate void ReactUIVisible(bool visible);
        public static ReactUIVisible reactUIVisible;

        public delegate void DodgeClicked();
        public static DodgeClicked dodgeClicked;

        public delegate void GuardClicked();
        public static GuardClicked guardClicked;

        public delegate void DeselectButtonClicked();
        public static DeselectButtonClicked deselectButtonClicked;

        public delegate void CounterClicked();
        public static CounterClicked counterClicked;

        public delegate void ShowCursor(int x, int y);
        public static ShowCursor showCursor;

        public delegate void HideCursor();
        public static HideCursor hideCursor;

        #endregion

        #region Unit
        public delegate void HPValueChanged();
        public static HPValueChanged hpValueChanged;

        public delegate void UnitWaiting(LivingObject unit, bool waiting);
        public static UnitWaiting unitWaiting;

        public delegate void UnitCanMove(LivingObject unit, bool canMove);
        public static UnitCanMove unitCanMove;
        public delegate void UnitShowActiveEffect(LivingObject unit, bool canMove, bool disableOthers);
        public static UnitShowActiveEffect unitShowActiveEffect;

        public delegate void UnitDied(LivingObject unit);
        public static UnitDied unitDied;


        #endregion

        #region UnitActions
        public delegate void UnitMoveToEnemy();
        public static UnitMoveToEnemy unitMoveToEnemy;

        public delegate void StartMovingUnit();
        public static StartMovingUnit startMovingUnit;

        public delegate void StopMovingUnit();
        public static StopMovingUnit stopMovingUnit;

        public delegate void DeselectActiveCharacter();
        public static DeselectActiveCharacter deselectActiveCharacter;

        public delegate void SelectActiveCharacter();
        public static SelectActiveCharacter selectedActiveCharacter;

        

        #endregion

        #region Clicks
        public delegate void EnemyClicked(LivingObject unit);
        public static EnemyClicked enemyClicked;

        public delegate void UnitClickedConfirmed(LivingObject unit, bool confirm);
        public static UnitClickedConfirmed unitClickedConfirmed;

        public delegate void ClickedOnGrid(int x, int y, Vector2 clickedPos);
        public static ClickedOnGrid clickedOnGrid;

        public delegate void ClickedOnField(int x, int y);
        public static ClickedOnField clickedOnField;

        public delegate void UnitClickedOnActiveTile(LivingObject unit, int x, int y);
        public static UnitClickedOnActiveTile unitClickedOnActiveTile;

        public delegate void MonsterClickedOnActiveBigTile(LivingObject unit, BigTile position);
        public static MonsterClickedOnActiveBigTile monsterClickedOnActiveBigTile;

        public delegate void UnitClicked(LivingObject character);
        public static UnitClicked unitClicked;
        #endregion

        #region DragStuff
        public delegate void DraggedOverUnit(LivingObject unit);
        public static DraggedOverUnit draggedOverUnit;

        public delegate void StartDrag(int gridX, int gridY);
        public static StartDrag startDrag;



        public delegate void UnitDragged(int x,int y,LivingObject character);
        public static UnitDragged unitDragged;


        public delegate void EndDrag();
        public static EndDrag endDrag;

        public delegate void EndDragOverNothing();
        public static EndDragOverNothing endDragOverNothing;

        public delegate void EndDragOverUnit(LivingObject character);
        public static EndDragOverUnit endDragOverUnit;

        public delegate void EndDragOverGrid(int x, int y);
        public static EndDragOverGrid endDragOverGrid;
        #endregion

        #region SwipeInteraction
        public delegate void SwipeFastEvent();
        public static SwipeFastEvent swipeFastEvent;

        public delegate void SwipeStrongEvent();
        public static SwipeStrongEvent swipeStrongEvent;

        public delegate void SwipeIdleEvent();
        public static SwipeIdleEvent swipeIdleEvent;

        public delegate void SwipeLeftConfirmedEvent();
        public static SwipeLeftConfirmedEvent swipeLeftConfirmedEvent;

        public delegate void SwipeRightConfirmedEvent();
        public static SwipeRightConfirmedEvent swipeRightConfirmedEvent;
        #endregion

        public static void ResetEvents()
        {
            #region Commands
            commandFinished = null;
            allCommandsFinished = null;
            undo = null;
            #endregion

            #region TurnManagement
            endTurn = null;
            #endregion

            #region AttackPatterns
            continuePressed = null;
            attackPatternUsed = null;
            #endregion

            #region Fight
            attackerDmgChanged = null;
            attackerHitChanged = null;
            #endregion

            #region UI
            attackUIVisible = null;

            reactUIVisible = null;

            dodgeClicked = null;

            guardClicked = null;

            counterClicked = null;

            deselectButtonClicked = null;
            #endregion

            #region UnitActions
            unitMoveToEnemy = null;

            startMovingUnit = null;

            stopMovingUnit = null;

            deselectActiveCharacter = null;

            selectedActiveCharacter = null;
            #endregion

            #region Clicks
            enemyClicked = null;

            unitClickedConfirmed = null;

            clickedOnGrid = null;

            clickedOnField = null;

            unitClickedOnActiveTile = null;

            monsterClickedOnActiveBigTile = null;

            unitClicked = null;
            #endregion

            #region DragStuff
            draggedOverUnit = null;

            startDrag = null;

            unitDragged = null;

            endDrag = null;

            endDragOverNothing = null;

            endDragOverUnit = null;

            endDragOverGrid = null;
            #endregion
        }
}
}
