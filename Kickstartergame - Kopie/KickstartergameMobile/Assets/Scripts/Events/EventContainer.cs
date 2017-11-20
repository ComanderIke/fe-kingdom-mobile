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
        
        public delegate void CommandFinished();
        public static CommandFinished commandFinished;

        public delegate void EndTurn();
        public static EndTurn endTurn;

        public delegate void Undo();
        public static Undo undo;

        public delegate void DeselectActiveCharacter();
        public static DeselectActiveCharacter deselectActiveCharacter;

        public delegate void EnemyClicked(LivingObject unit);
        public static EnemyClicked enemyClicked;

        public delegate void UnitClickedConfirmed(LivingObject unit, bool confirm);
        public static UnitClickedConfirmed unitClickedConfirmed;

        #region UnitMovement
        public delegate void UnitMoveToEnemy();
        public static UnitMoveToEnemy unitMoveToEnemy;

        public delegate void StartMovingUnit();
        public static StartMovingUnit startMovingUnit;

        public delegate void StopMovingUnit();
        public static StopMovingUnit stopMovingUnit;
        #endregion

        #region Clicks
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
