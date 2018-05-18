using Assets.Scripts.Characters;
using Assets.Scripts.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    class MoveCharacterCommand : Command
    {
        private Unit unit;
        private int x;
        private int y;
        private int oldX;
        private int oldY;
        private List<Vector2> path;
        
        public MoveCharacterCommand(Unit unit, int x, int y)
        {
            this.unit = unit;
            oldX = unit.GridPosition.x;
            oldY = unit.GridPosition.y;
            this.x = x;
            this.y = y;
        }
        public MoveCharacterCommand(Unit unit, int x, int y, List<Vector2> path):this(unit,x,y)
        {
            this.path = path;
        }
        public override void Execute()
        {
            if(path==null)
                MainScript.instance.SwitchState(new MovementState(unit, x, y));
            else
                MainScript.instance.SwitchState(new MovementState(unit, x, y,path));
        }

        public override void Undo()
        {
            unit.SetPosition(oldX, oldY);
            unit.UnitTurnState.Reset();
        }
    }
}
