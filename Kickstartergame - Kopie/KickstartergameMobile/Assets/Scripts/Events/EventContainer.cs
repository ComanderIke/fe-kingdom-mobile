using Assets.Scripts.AI.AttackPatterns;
using Assets.Scripts.Characters;
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

        public delegate void AllCommandsFinished();
        public static AllCommandsFinished allCommandsFinished;

        public delegate void Undo();
        public static Undo undo;
        #endregion

        #region TurnManagement
        public delegate void EndTurn();
        public static EndTurn endTurn;
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

        #endregion
        
        #region UI
        public delegate void AttackUIVisible( bool visible);
        public static AttackUIVisible attackUIVisible;

        public delegate void ReactUIVisible(bool visible);
        public static ReactUIVisible reactUIVisible;

        #endregion

        #region Unit
        public delegate void HPValueChanged();
        public static HPValueChanged hpValueChanged;

        public delegate void UnitActiveChanged(LivingObject unit, bool active);
        public static UnitActiveChanged unitActiveChanged;

        public delegate void UnitWaiting(LivingObject unit, bool waiting);
        public static UnitWaiting unitWaiting;

        public delegate void UnitCanMove(LivingObject unit, bool canMove);
        public static UnitCanMove unitCanMove;

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
    }
}
