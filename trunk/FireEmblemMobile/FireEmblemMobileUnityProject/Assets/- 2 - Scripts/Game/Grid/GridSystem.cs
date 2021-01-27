using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameResources;
using Game.Grid;
using Game.Grid.PathFinding;
using Game.Manager;
using Game.Mechanics;
using GameEngine;
using UnityEngine;

namespace Game.Map
{
    [Serializable]
    [RequireComponent(typeof(GridBuilder))]
    public class GridSystem : MonoBehaviour, IEngineSystem
    {
        public const float GRID_X_OFFSET = 0.0f;
        private string MapName;
        [HideInInspector]
        public GridData GridData;
        public Tile[,] Tiles { get; private set; }
        public GridRenderer GridRenderer { get; set; }
        public GridLogic GridLogic { get; set; }
        public NodeHelper NodeHelper;
        public AStar pathFinder;

        public void Init()
        {
            Tiles = GetComponent<GridBuilder>().GetTiles();
            GridData = ResourceScript.Instance.grid.gridData;
            GridRenderer = new GridRenderer(this);
            GridLogic = new GridLogic(this);
            NodeHelper = new NodeHelper(GridData.width, GridData.height);
            pathFinder = new AStar(GridLogic.tileChecker);
            UnitSelectionSystem.OnDeselectCharacter += HideMoveRange;
            UnitSelectionSystem.OnSelectedCharacter += SelectedCharacter;
            UnitSelectionSystem.OnEnemySelected += OnEnemySelected;
            UnitSelectionSystem.OnSelectedInActiveCharacter += OnEnemySelected;
            MovementState.OnMovementFinished += (IGridActor u) => HideMoveRange();
            Unit.UnitDied += RemoveUnitFromGrid;
        }

        public ITileChecker GetTileChecker()
        {
            return GridLogic.tileChecker;
        }
        void RemoveUnitFromGrid(IGridActor u)
        {
            Tiles[u.GridPosition.X, u.GridPosition.Y].Actor = null;
        }
      
        private void OnEnemySelected(IGridActor gridActor)
        {

            HideMoveRange();
            ShowMovementRangeOnGrid(gridActor);

            ShowAttackRangeOnGrid(gridActor, new List<int>(gridActor.AttackRanges), true);
            GridLogic.ResetActiveFields();
        }
        public Tile GetTileFromVector2(Vector2 pos)
        {
            return Tiles[(int) pos.x, (int) pos.y];
        }
        private void SelectedCharacter(IGridActor gridActor)
        {

            HideMoveRange();

            ShowMovementRangeOnGrid(gridActor);
            //if (!u.UnitTurnState.HasAttacked)
            ShowAttackRangeOnGrid(gridActor, new List<int>(gridActor.AttackRanges));

        }

        public void ShowMovementRangeOnGrid(IGridActor c)
        {
            ShowMovement(c.GridPosition.X, c.GridPosition.Y, c.MovementRage, 0, c);
        }

      

        public void ShowAttackRangeOnGrid(IGridActor character, List<int> attack, bool soft=false)
        {
            NodeHelper.Reset();
            foreach (var f in GridLogic.TilesFromWhereYouCanAttack(character))
            {
                int x = f.X;
                int y = f.Y;
                foreach (int range in attack)
                {
                    ShowAttackRecursive(character, x, y, range, new List<int>(), soft);
                }
            }

            GridRenderer.ShowStandOnVisual(character);
        }

        public void ShowAttackFromPosition(Unit character, int x, int y)
        {
            foreach (int range in character.Stats.AttackRanges)
            {
                ShowAttackRecursive(character, x, y, range, new List<int>());
            }
            GridRenderer.ShowStandOnVisual(character);
        }
        public void HideMoveRange()//In Order to work with ActionEvent!!!
        {
            for (int i = 0; i < GridData.width; i++)
            {
                for (int j = 0; j < GridData.height; j++)
                {
                    Tiles[i, j].Reset();
                }
            }
            GridLogic.ResetActiveFields();
            NodeHelper.Reset();
        }
        public void ShowAttackRecursive(IGridActor character, int x, int y, int range, List<int> direction, bool soft=false)
        {
            if (range <= 0)
            {
                if (!IsTileMoveableAndActive(x, y))
                {
                    GridRenderer.SetFieldMaterialAttack(new Vector2(x, y), character.FactionId, !character.TurnStateManager.HasMoved, GridGameManager.Instance.FactionManager.IsActiveFaction(character.FactionId));
                    GridLogic.gridSessionData.AddValidAttackPosition(x, y);
                }

                return;
            }

            if (!direction.Contains(2))
            {
                if (GridLogic.CheckAttackField(x + 1, y))
                {
                    var newDirection = new List<int>(direction) {1};
                    ShowAttackRecursive(character, x + 1, y, range - 1, newDirection, soft);
                }
            }

            if (!direction.Contains(1))
            {
                if (GridLogic.CheckAttackField(x - 1, y))
                {
                    var newDirection = new List<int>(direction) {2};
                    ShowAttackRecursive(character, x - 1, y, range - 1, newDirection, soft);
                }
            }

            if (!direction.Contains(4))
            {
                if (GridLogic.CheckAttackField(x, y + 1))
                {
                    var newDirection = new List<int>(direction) {3};
                    ShowAttackRecursive(character, x, y + 1, range - 1, newDirection, soft);
                }
            }

            if (!direction.Contains(3))
            {
                if (GridLogic.CheckAttackField(x, y - 1))
                {
                    var newDirection = new List<int>(direction) {4};
                    ShowAttackRecursive(character, x, y - 1, range - 1, newDirection, soft);
                }
            }
        }
        private void ShowMovement(int x, int y, int range, int c, IGridActor unit)
        {
            if (range < 0)
            {
                return;
            }

            GridRenderer.SetFieldMaterial(new Vector2(x, y), unit.FactionId, !unit.TurnStateManager.HasMoved,GridGameManager.Instance.FactionManager.IsActiveFaction(unit.FactionId));
            GridLogic.gridSessionData.AddValidPosition(x, y);
            NodeHelper.Nodes[x, y].C = c;
            c++;
            if (GridLogic.CheckField(x - 1, y, unit, range) && NodeHelper.NodeFaster(x - 1, y, c))
                ShowMovement(x - 1, y, range - 1, c, unit);
            if (GridLogic.CheckField(x + 1, y, unit, range) && NodeHelper.NodeFaster(x + 1, y, c))
                ShowMovement(x + 1, y, range - 1, c, unit);
            if (GridLogic.CheckField(x, y - 1, unit, range) && NodeHelper.NodeFaster(x, y - 1, c))
                ShowMovement(x, y - 1, range - 1, c, unit);
            if (GridLogic.CheckField(x, y + 1, unit, range) && NodeHelper.NodeFaster(x, y + 1, c))
                ShowMovement(x, y + 1, range - 1, c, unit);
        }

        public bool IsTileMoveableAndActive(int x, int y)
        {
            return GridLogic.IsMoveableAndActive(x,y);
        }
        public bool IsAttackableAndActive(int x, int y)
        {
            return GridLogic.IsAttackableAndActive(x,y);
        }
        public bool IsOutOfBounds(int x, int y)
        {
            return x < 0 || x >= GridData.width || y < 0 ||
                   y >= GridData.height;
        }

        public Tile GetTile(int x, int y)
        {
            return Tiles[x, y];
        }

        public void SetUnitPosition(IGridActor unit, int x, int y)
        {
            if (x != -1 && y != -1)
            {
                if(unit.GridPosition.X != -1 && unit.GridPosition.Y!=-1)
                    Tiles[unit.GridPosition.X, unit.GridPosition.Y].Actor = null;
                Tiles[x, y].Actor = unit;
                unit.SetPosition(x, y);
            }
        }
    }
}