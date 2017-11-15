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
        public delegate void EndTurn();
        public static EndTurn endTurn;

        public delegate void ClickedOnGrid(int x, int y, Vector2 clickedPos);
        public static ClickedOnGrid clickedOnGrid;

        public delegate void ClickedOnField(int x, int y);
        public static ClickedOnField clickedOnField;

        public delegate void UnitClickedOnActiveTile(LivingObject unit, int x, int y);
        public static UnitClickedOnActiveTile unitClickedOnActiveTile;

        public delegate void MonsterClickedOnActiveBigTile(LivingObject unit, BigTile position);
        public static MonsterClickedOnActiveBigTile monsterClickedOnActiveBigTile;
        

        public delegate void DraggedOverUnit(LivingObject unit);
        public static DraggedOverUnit draggedOverUnit;

        public delegate void StartDrag(int gridX, int gridY);
        public static StartDrag startDrag;

        public delegate void UnitClicked(LivingObject character);
        public static UnitClicked unitClicked;

        public delegate void UnitDragged(int x,int y,LivingObject character);
        public static UnitDragged unitDragged;

        public delegate void EndDragOverNothing();
        public static EndDragOverNothing endDragOverNothing;

        public delegate void EndDragOverUnit(LivingObject character);
        public static EndDragOverUnit endDragOverUnit;

        public delegate void EndDragOverGrid(int x, int y);
        public static EndDragOverGrid endDragOverGrid;
    }
}
