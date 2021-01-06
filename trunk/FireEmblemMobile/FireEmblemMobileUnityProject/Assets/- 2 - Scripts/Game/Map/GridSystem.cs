using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.GameResources;
using Game.Grid;
using Game.Grid.PathFinding;
using Game.Mechanics;
using GameEngine;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Map
{
    [Serializable]
    [RequireComponent(typeof(GridBuilder))]
    public class GridSystem : MonoBehaviour, IEngineSystem
    {
        public const float GRID_X_OFFSET = 0.0f;
        public string MapName;
        [FormerlySerializedAs("GridBuilder")] [SerializeField]
        private GridBuilder gridBuilder;
        public GridResources GridResources;
        public GridData GridData;
        public Tile[,] Tiles { get; set; }
        public GridRenderer GridRenderer { get; set; }
        public GridLogic GridLogic { get; set; }
        public NodeHelper NodeHelper;

        private void Start()
        {

            Tiles = GetComponent<GridBuilder>().GetTiles();
            
            GridRenderer = new GridRenderer(this);
            GridLogic = new GridLogic(this);
            NodeHelper = new NodeHelper(GridData.width, GridData.height);
            UnitSelectionSystem.OnDeselectCharacter += HideMovementRangeOnGrid;
            UnitSelectionSystem.OnSelectedCharacter += SelectedCharacter;
            UnitSelectionSystem.OnEnemySelected += OnEnemySelected;
            UnitSelectionSystem.OnSelectedInActiveCharacter += OnEnemySelected;
            MovementState.OnMovementFinished += (Unit u) => HideMovementRangeOnGrid();
            AStar PathFindingManager = new AStar(this, GridData.width, GridData.height);
            PathFindingManager.FindPath(0, 0, 3, 3, 1, true, new List<int>(1));//Do For JIT Performance thingy
            //test
        }
        [ContextMenu("Test")]
        private void OnEnemySelected(Unit u)
        {
            HideMovementRangeOnGrid();
            ShowMovementRangeOnGrid(u, true);

            ShowAttackRangeOnGrid(u, new List<int>(u.Stats.AttackRanges), true);
            GridLogic.ResetActiveFields();
        }
        public Tile GetTileFromVector2(Vector2 pos)
        {
            return Tiles[(int) pos.x, (int) pos.y];
        }
        private void SelectedCharacter(Unit u)
        {
            HideMovementRangeOnGrid();
            if (u.Ap!=0)
            {
                ShowMovementRangeOnGrid(u);
                if (!u.UnitTurnState.HasAttacked)
                    ShowAttackRangeOnGrid(u, new List<int>(u.Stats.AttackRanges));
            }
            else
            {
               
                if (!u.UnitTurnState.HasAttacked)
                {
                    //Debug.Log("HERE");
                    ShowAttackRangeOnGrid(u, new List<int>(u.Stats.AttackRanges));
                }
            }
        }

        public void ShowMovementRangeOnGrid(Unit c, bool soft=false)
        {
            gridVisible = true;
            ShowMovement(c.GridPosition.X, c.GridPosition.Y, c.Ap, c.Ap,
                    new List<int>(c.Stats.AttackRanges), 0, c.Faction.Id, soft);
        }

      

        public void ShowAttackRangeOnGrid(Unit character, List<int> attack, bool soft=false)
        {
            gridVisible = true;
            var tilesFromWhereUCanAttack = (from Tile f in Tiles where (f.X == character.GridPosition.X && f.Y== character.GridPosition.Y) || (f.IsActive && (f.Unit == null || f.Unit == character)) select f);
         
            NodeHelper.Reset();
            foreach (var f in tilesFromWhereUCanAttack)
            {
                int x = f.X;
                int y = f.Y;
                foreach (int range in attack)
                {
                    ShowAttackRecursive(character, x, y, range, new List<int>(), soft);
                }
            }

            GridRenderer.ShowStandOnTexture(character);
            //StartCoroutine(GridRenderer.FieldAnimation());
        }

        public void ShowAttackFromPosition(Unit character, int x, int y)
        {
            foreach (int range in character.Stats.AttackRanges)
            {
                ShowAttackRecursive(character, x, y, range, new List<int>());
            }
            GridRenderer.ShowStandOnTexture(character);
        }
        bool gridVisible = false;
        public void HideMovementRangeOnGrid()//In Order to work with ActionEvent!!!
        {
            HideMovementRangeOnGrid(null);
        }
        private void HideMovementRangeOnGrid(List<Vector2> ignorePositions)
        {
            if (!gridVisible)
                return;
            gridVisible = false;
            for (int i = 0; i < GridData.width; i++)
            {
                for (int j = 0; j < GridData.height; j++)
                {
                    bool ignore = false;
                    if(ignorePositions != null)
                        foreach (Vector2 v in ignorePositions)
                        {
                            int x = (int)v.x;
                            int y = (int)v.y;
                            if (i == x && j == y)
                            {
                                ignore = true;
                            }
                        }

                    if (!ignore)
                    {
                        var m = Tiles[i, j].GameObject.GetComponent<SpriteRenderer>();
                        m.sprite = Tiles[i, j].IsAccessible
                            ? GridResources.GridSprite
                            : GridResources.GridSpriteInvalid;

                        Tiles[i, j].IsActive = false;
                        Tiles[i, j].IsAttackable = false;
                    }
                }
            }

            NodeHelper.Reset();
           
        }

        public void ShowAttackRecursive(Unit character, int x, int y, int range, List<int> direction, bool soft=false)
        {
            if (range <= 0)
            {
                GridRenderer.SetFieldMaterial(new Vector2(x, y), character.Faction.Id, true, soft);

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
        private void ShowMovement(int x, int y, int range, int attackIndex, List<int> attack, int c, int playerId, bool soft)
        {
            if (range < 0)
            {
                return;
            }

            GridRenderer.SetFieldMaterial(new Vector2(x, y), playerId, false, soft);

            NodeHelper.Nodes[x, y].C = c;
            c++;
            if (GridLogic.CheckField(x - 1, y, playerId, range) && NodeHelper.NodeFaster(x - 1, y, c))
                ShowMovement(x - 1, y, range - 1, attackIndex, new List<int>(attack), c, playerId, soft);
            if (GridLogic.CheckField(x + 1, y, playerId, range) && NodeHelper.NodeFaster(x + 1, y, c))
                ShowMovement(x + 1, y, range - 1, attackIndex, new List<int>(attack), c, playerId, soft);
            if (GridLogic.CheckField(x, y - 1, playerId, range) && NodeHelper.NodeFaster(x, y - 1, c))
                ShowMovement(x, y - 1, range - 1, attackIndex, new List<int>(attack), c, playerId, soft);
            if (GridLogic.CheckField(x, y + 1, playerId, range) && NodeHelper.NodeFaster(x, y + 1, c))
                ShowMovement(x, y + 1, range - 1, attackIndex, new List<int>(attack), c, playerId, soft);
        }
    }
}