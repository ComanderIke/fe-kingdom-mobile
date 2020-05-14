using Assets.GameEngine;
using Assets.GameActors.Units;
using Assets.GameResources;
using Assets.Grid;
using Assets.Grid.PathFinding;
using Assets.GUI;
using Assets.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Game.GameStates;

namespace Assets.Map
{
    [Serializable]
    public class MapSystem : MonoBehaviour, IEngineSystem
    {
        public const float GRID_X_OFFSET = 0.0f;
        public string MapName;
        public GridData GridData;
        public GridBuilder GridBuilder { get; set; }
        public Transform GridTransform;
        public GridResources GridResources;
        public Tile[,] Tiles { get; set; }
        public GridRenderer GridRenderer { get; set; }
        public GridLogic GridLogic { get; set; }
        public NodeHelper NodeHelper;

        private void Start()
        {
            var mapData = FindObjectOfType<DataScript>().MapData;
            MapName = mapData.Name;
            GridData = new GridData(mapData.Width, mapData.Height);

            Tiles = new Tile[GridData.Width, GridData.Height];
           
            GridBuilder = new GridBuilder(GridResources.GridSprite, GridResources.CellMaterialValid, GridResources.CellMaterialInvalid);
            Tiles = GridBuilder.Build(GridData.Width, GridData.Height, GridTransform);
            GridRenderer = new GridRenderer(this);
            GridLogic = new GridLogic(this);
            NodeHelper = new NodeHelper(GridData.Width, GridData.Height);
            UnitSelectionSystem.OnDeselectCharacter += HideMovementRangeOnGrid;
            UnitSelectionSystem.OnSelectedCharacter += SelectedCharacter;
            UnitSelectionSystem.OnEnemySelected += OnEnemySelected;
            UnitSelectionSystem.OnSelectedInActiveCharacter += OnEnemySelected;
            MovementState.OnMovementFinished += (Unit u) => HideMovementRangeOnGrid();

        }
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
            ShowMovement(c.GridPosition.X, c.GridPosition.Y, c.Ap, c.Ap,
                    new List<int>(c.Stats.AttackRanges), 0, c.Faction.Id, false, soft);
        }

        private void ShowMonsterMovement(int x, int y, int range, List<int> attack, int c, int playerId)
        {
            if (range < 0)
            {
                return;
            }

            GridRenderer.SetBigTileActive(
                new BigTile(new Vector2(x, y), new Vector2(x + 1, y), new Vector2(x, y + 1), new Vector2(x + 1, y + 1)),
                playerId, false);
            if (GridLogic.CheckMonsterField(
                new BigTile(new Vector2(x - 1, y), new Vector2(x, y), new Vector2(x - 1, y + 1), new Vector2(x, y + 1)),
                playerId, range))
                ShowMonsterMovement(x - 1, y, range - 1, new List<int>(attack), c, playerId);
            if (GridLogic.CheckMonsterField(
                new BigTile(new Vector2(x + 1, y), new Vector2(x + 2, y), new Vector2(x + 1, y + 1),
                    new Vector2(x + 2, y + 1)), playerId, range))
                ShowMonsterMovement(x + 1, y, range - 1, new List<int>(attack), c, playerId);
            if (GridLogic.CheckMonsterField(
                new BigTile(new Vector2(x, y - 1), new Vector2(x + 1, y - 1), new Vector2(x, y), new Vector2(x + 1, y)),
                playerId, range))
                ShowMonsterMovement(x, y - 1, range - 1, new List<int>(attack), c, playerId);
            if (GridLogic.CheckMonsterField(
                new BigTile(new Vector2(x, y + 1), new Vector2(x + 1, y + 1), new Vector2(x, y + 2),
                    new Vector2(x + 1, y + 2)), playerId, range))
                ShowMonsterMovement(x, y + 1, range - 1, new List<int>(attack), c, playerId);
        }

        public void ShowAttackRangeOnGrid(Unit character, List<int> attack, bool soft=false)
        {
            var tilesFromWhereUCanAttack = (from Tile f in Tiles where (f.X == character.GridPosition.X && f.Y== character.GridPosition.Y) || (f.IsActive && (f.Unit == null || f.Unit == character)) select f).ToList();
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
        public void HideMovementRangeOnGrid()//In Order to work with ActionEvent!!!
        {
            HideMovementRangeOnGrid(null);
        }
        public void HideMovementRangeOnGrid(List<Vector2> ignorePositions)
        {
            for (int i = 0; i < GridData.Width; i++)
            {
                for (int j = 0; j < GridData.Height; j++)
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
                    //&& AttackNodeFaster(x+1, y, c))
                    var newDirection = new List<int>(direction) {1};
                    ShowAttackRecursive(character, x + 1, y, range - 1, newDirection, soft);
                }
            }

            if (!direction.Contains(1))
            {
                if (GridLogic.CheckAttackField(x - 1, y))
                {
                    //&& AttackNodeFaster(x - 1, y, c))
                    var newDirection = new List<int>(direction) {2};
                    ShowAttackRecursive(character, x - 1, y, range - 1, newDirection, soft);
                }
            }

            if (!direction.Contains(4))
            {
                if (GridLogic.CheckAttackField(x, y + 1))
                {
                    // && AttackNodeFaster(x , y+1, c))
                    var newDirection = new List<int>(direction) {3};
                    ShowAttackRecursive(character, x, y + 1, range - 1, newDirection, soft);
                }
            }

            if (!direction.Contains(3))
            {
                if (GridLogic.CheckAttackField(x, y - 1))
                {
                    //&& AttackNodeFaster(x , y-1, c))
                    var newDirection = new List<int>(direction) {4};
                    ShowAttackRecursive(character, x, y - 1, range - 1, newDirection, soft);
                }
            }
        }
        //public void ShowAttackRanges(int x, int y, int range, List<int> direction)
        //{
        //    if (range <= 0)
        //    {
        //        MeshRenderer m = Tiles[x, y].gameObject.GetComponent<MeshRenderer>();
        //        m.material = gridRessources.cellMaterialAttack;
        //        return;
        //    }
        //    if (!direction.Contains(2))
        //    {
        //        if (GridLogic.CheckAttackField(x + 1, y))
        //        {
        //            List<int> newdirection = new List<int>(direction);
        //            newdirection.Add(1);
        //            ShowAttackRanges(x + 1, y, range - 1, newdirection);
        //        }
        //    }
        //    if (!direction.Contains(1))
        //    {
        //        if (GridLogic.CheckAttackField(x - 1, y))
        //        {
        //            List<int> newdirection = new List<int>(direction);
        //            newdirection.Add(2);
        //            ShowAttackRanges(x - 1, y, range - 1, newdirection);
        //        }
        //    }
        //    if (!direction.Contains(4))
        //    {
        //        if (GridLogic.CheckAttackField(x, y + 1))
        //        {
        //            List<int> newdirection = new List<int>(direction);
        //            newdirection.Add(3);
        //            ShowAttackRanges(x, y + 1, range - 1, newdirection);
        //        }
        //    }
        //    if (!direction.Contains(3))
        //    {
        //        if (GridLogic.CheckAttackField(x, y - 1))
        //        {
        //            List<int> newdirection = new List<int>(direction);
        //            newdirection.Add(4);
        //            ShowAttackRanges(x, y - 1, range - 1, newdirection);
        //        }
        //    }

        //}

        private void ShowMovement(int x, int y, int range, int attackIndex, IReadOnlyCollection<int> attack, int c, int playerId,
            bool enemy, bool soft)
        {
            if (range < 0)
            {
                return;
            }

            if (!enemy)
            {
                GridRenderer.SetFieldMaterial(new Vector2(x, y), playerId, false, soft);
            }

            NodeHelper.Nodes[x, y].C = c;
            c++;
            if (GridLogic.CheckField(x - 1, y, playerId, range) && NodeHelper.NodeFaster(x - 1, y, c))
                ShowMovement(x - 1, y, range - 1, attackIndex, new List<int>(attack), c, playerId, enemy, soft);
            if (GridLogic.CheckField(x + 1, y, playerId, range) && NodeHelper.NodeFaster(x + 1, y, c))
                ShowMovement(x + 1, y, range - 1, attackIndex, new List<int>(attack), c, playerId, enemy, soft);
            if (GridLogic.CheckField(x, y - 1, playerId, range) && NodeHelper.NodeFaster(x, y - 1, c))
                ShowMovement(x, y - 1, range - 1, attackIndex, new List<int>(attack), c, playerId, enemy, soft);
            if (GridLogic.CheckField(x, y + 1, playerId, range) && NodeHelper.NodeFaster(x, y + 1, c))
                ShowMovement(x, y + 1, range - 1, attackIndex, new List<int>(attack), c, playerId, enemy, soft);
        }
    }
}