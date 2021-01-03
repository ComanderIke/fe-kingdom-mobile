using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.Manager;
using Game.Map;
using UnityEngine;

namespace Game.Grid
{
    public class GridLogic
    {
        private readonly GridGameManager gridGameManager;
        public Tile[,] Tiles { get; set; }
        public MapSystem GridManager { get; set; }
        private readonly GridData gridData;

        public GridLogic(MapSystem gridManager)
        {
            gridGameManager = GridGameManager.Instance;
            GridManager = gridManager;
            Tiles = gridManager.Tiles;
            gridData = gridManager.GridData;
           
        }

       


        public List<Unit> GetAttackTargets(Unit unit)
        {
            int x = unit.GridPosition.X;
            int y = unit.GridPosition.Y;
            var targets = new List<Unit>();
            foreach (int attackRange in unit.Stats.AttackRanges)
            {
                for (int i = -attackRange; i <= +attackRange; i++)
                {
                    for (int j = -attackRange; j <= attackRange; j++)
                    {
                        if (Math.Abs(j + i) == attackRange)
                        {
                            // Debug.Log("attackTargets at "+ unit.GridPosition.GetPos()+": " +(i + x) + " " + (j + y));
                            if (IsOutOfBounds(new Vector2(x + i, y + j)))
                                continue;
                            var unitOnTile = Tiles[i + x, j + y].Unit;
                            if (unitOnTile != null && unitOnTile.Faction.Id != unit.Faction.Id)
                            {
                                targets.Add(unitOnTile);
                            }
                        }
                    }
                }
            }

            return targets;
        }

        public List<Unit> GetAttackTargetsAtGameObjectPosition(Unit unit)
        {
            int x = (int) unit.GameTransform.GetPosition().x;
            int y = (int) unit.GameTransform.GetPosition().y;
            var targets = new List<Unit>();
            foreach (int attackRange in unit.Stats.AttackRanges)
            {
                for (int i = -attackRange; i <= +attackRange; i++)
                {
                    for (int j = -attackRange; j <= attackRange; j++)
                    {
                        if (Math.Abs(j + i) == attackRange||Math.Abs(j)+Math.Abs(i) == attackRange)
                        {
                            //Debug.Log("attackTargets at ["+ x+", "+y+"]: [" +(i + x) + ", " + (j + y)+"]");
                            if (IsOutOfBounds(new Vector2(x + i, y + j)))
                                continue;
                            var unitOnTile = Tiles[i + x, j + y].Unit;
                            if (unitOnTile != null && unitOnTile.Faction.Id != unit.Faction.Id)
                            {
                                targets.Add(unitOnTile);
                            }
                        }
                    }
                }
            }
            //Debug.Log("Attackable Targets at Position: [" + x + " ," + y + "] and AttackRange of: ");
            //foreach (int attackRange in unit.Stats.AttackRanges)
            //{
            //    Debug.Log(attackRange +", ");
            //}
            //foreach (Unit target in targets)
            //{
            //    Debug.Log(target.Name + " [" + target.GridPosition.X + ", " + target.GridPosition.Y+"]");
            //}
            return targets;
        }

        public bool IsFieldAttackable(int x, int z)
        {
            return Tiles[x, z].IsAttackable;
        }

        public bool IsOutOfBounds(Vector2 pos)
        {
            return pos.x < 0 || pos.y < 0 || pos.x >= gridData.Width || pos.y >= gridData.Height;
        }

        private bool IsValidAndActive(Vector2 pos, int team)
        {
            bool invalid = (pos.x < 0) || (pos.y < 0) || (pos.x >= gridData.Width) || (pos.y >= gridData.Height);
            if (!invalid)
            {
                invalid = !Tiles[(int) pos.x, (int) pos.y].IsAccessible;
                if (!Tiles[(int)pos.x, (int)pos.y].IsActive)
                    invalid = true;
                if (Tiles[(int) pos.x, (int) pos.y].Unit != null)
                    if (Tiles[(int) pos.x, (int) pos.y].Unit.Faction.Id == team)
                        invalid = false;
          
            }

            return !invalid;
        }

        public bool IsValidAndActive(BigTile position, int team)
        {
            return IsValidAndActive(position.BottomLeft(), team) && IsValidAndActive(position.BottomRight(), team) &&
                   IsValidAndActive(position.TopLeft(), team) && IsValidAndActive(position.TopRight(), team);
        }

        public BigTile GetMoveableBigTileFromPosition(Vector2 position, int team, Unit character)
        {
            var leftTop = new BigTile(new Vector2(position.x - 1, position.y), new Vector2(position.x, position.y),
                new Vector2(position.x - 1, position.y + 1), new Vector2(position.x, position.y + 1));
            var leftBottom = new BigTile(new Vector2(position.x - 1, position.y - 1),
                new Vector2(position.x, position.y - 1), new Vector2(position.x - 1, position.y),
                new Vector2(position.x, position.y));
            var rightTop = new BigTile(new Vector2(position.x, position.y), new Vector2(position.x + 1, position.y),
                new Vector2(position.x, position.y + 1), new Vector2(position.x + 1, position.y + 1));
            var rightBottom = new BigTile(new Vector2(position.x, position.y - 1),
                new Vector2(position.x + 1, position.y - 1), new Vector2(position.x, position.y),
                new Vector2(position.x + 1, position.y));
            if (IsMovableLocation(leftTop, team, character))
                return leftTop;
            if (IsMovableLocation(leftBottom, team, character))
                return leftBottom;
            if (IsMovableLocation(rightTop, team, character))
                return rightTop;
            if (IsMovableLocation(rightBottom, team, character))
                return rightBottom;
            return null;
        }

        public BigTile GetNearestBigTileFromEnemy(Unit character)
        {
            var leftPosition = new Vector2(character.GridPosition.X - 1, character.GridPosition.Y);
            var rightPosition = new Vector2(character.GridPosition.X + 1, character.GridPosition.Y);
            var topPosition = new Vector2(character.GridPosition.X, character.GridPosition.Y + 1);
            var bottomPosition = new Vector2(character.GridPosition.X, character.GridPosition.Y - 1);
            if (IsValidLocation(leftPosition, character))
            {
                var moveOption = GetMoveableBigTileFromPosition(leftPosition, character.Faction.Id, character);
                if (moveOption != null)
                    return moveOption;
            }

            if (IsValidLocation(rightPosition, character))
            {
                var moveOption = GetMoveableBigTileFromPosition(rightPosition, character.Faction.Id, character);
                if (moveOption != null)
                    return moveOption;
            }

            if (IsValidLocation(topPosition, character))
            {
                var moveOption = GetMoveableBigTileFromPosition(topPosition, character.Faction.Id, character);
                if (moveOption != null)
                    return moveOption;
            }

            if (IsValidLocation(bottomPosition, character))
            {
                var moveOption = GetMoveableBigTileFromPosition(bottomPosition, character.Faction.Id, character);
                if (moveOption != null)
                    return moveOption;
            }

            return null;
        }

        public bool CheckField(int x, int y, int team, int range)
        {
            if (x >= 0 && y >= 0 && x < gridData.Width && y < gridData.Height)
            {
                var field = Tiles[x, y];
                if (field.IsAccessible)
                {
                    if (field.Unit == null)
                        return true;
                    if (field.Unit.Faction.Id == team)
                        return true;
                }
                else
                {
                    //MeshRenderer m = fields[x, y].gameObject.GetComponent<MeshRenderer>();
                    //m.material.mainTexture = AttackTexture;
                    return range < 0;
                }
            }

            return false;
        }

        public bool CheckAttackField(int x, int y)
        {
            return x >= 0 && y >= 0 && x < gridData.Width && y < gridData.Height;
        }

        public bool CheckMonsterField(BigTile bigTile, int team, int range)
        {
            return CheckField((int) bigTile.BottomLeft().x, (int) bigTile.BottomLeft().y, team, range) &&
                   CheckField((int) bigTile.BottomRight().x, (int) bigTile.BottomRight().y, team, range) &&
                   CheckField((int) bigTile.TopLeft().x, (int) bigTile.TopLeft().y, team, range) &&
                   CheckField((int) bigTile.TopRight().x, (int) bigTile.TopRight().y, team, range);
        }

        private bool IsCellValid(int x, int z)
        {
            return true;
        }

        public bool IsValidLocation(int team, int sx, int sy, int x, int y, bool isAdjacent)
        {
            bool invalid = (x < 0) || (y < 0) || (x >= gridData.Width) || (y >= gridData.Height);
            if ((!invalid) && ((sx != x) || (sy != y)))
            {
                invalid = !Tiles[x, y].IsAccessible;
            }

            if (!invalid)
            {
                //Material m = fields[x, y].gameObject.GetComponent<MeshRenderer>().material;
                //if (m.mainTexture != MoveTexture)
                //    invalid = true;
                if (Tiles[x, y].Unit != null)
                {
                    if (Tiles[x, y].Unit.Faction.Id != team)
                    {
                        invalid = true;
                    }

                    if (isAdjacent) 
                    {
                        //Do nothing will be checked with attackRange Later
                        //Passthrough is ok but not stopping on it
                    }
                }
            }

            return !invalid;
        }

        public bool IsFieldFreeAndActive(int x, int y)
        {
            return Tiles[x, y].Unit == null && Tiles[x, y].IsActive;
        }
        public void ResetActiveFields()
        {
            GridManager.NodeHelper.Reset();
            for (int i = 0; i < gridData.Width; i++)
            {
                for (int j = 0; j < gridData.Height; j++)
                {
                    Tiles[i, j].IsActive = false;
                    Tiles[i, j].IsAttackable = false;
                }
            }
        }

        #region AIHELP

        public void GetMoveLocations(int x, int y, List<Vector2> locations, int range, int c, int team)
        {
            if (range < 0)
            {
                return;
            }

            if (!locations.Contains(new Vector2(x, y)) && Tiles[x, y].Unit == null)
                locations.Add(new Vector3(x, y)); //TODO Height?!
            GridManager.NodeHelper.Nodes[x, y].C = c;
            c++;
            if (CheckField(x - 1, y, team, range) && GridManager.NodeHelper.NodeFaster(x - 1, y, c))
                GetMoveLocations(x - 1, y, locations, range - 1, c, team);
            if (CheckField(x + 1, y, team, range) && GridManager.NodeHelper.NodeFaster(x + 1, y, c))
                GetMoveLocations(x + 1, y, locations, range - 1, c, team);
            if (CheckField(x, y - 1, team, range) && GridManager.NodeHelper.NodeFaster(x, y - 1, c))
                GetMoveLocations(x, y - 1, locations, range - 1, c, team);
            if (CheckField(x, y + 1, team, range) && GridManager.NodeHelper.NodeFaster(x, y + 1, c))
                GetMoveLocations(x, y + 1, locations, range - 1, c, team);
        }

        #endregion

        public bool IsValidLocation(Vector2 pos, Unit character)
        {
            bool invalid = (pos.x < 0) || (pos.y < 0) || (pos.x >= gridData.Width) || (pos.y >= gridData.Height);

            if (!invalid)
            {
                if (!Tiles[(int) pos.x, (int) pos.y].IsActive)
                    return false;
                invalid = !Tiles[(int) pos.x, (int) pos.y].IsAccessible;
                if (Tiles[(int) pos.x, (int) pos.y].Unit != null)
                {
                    if (Tiles[(int) pos.x, (int) pos.y].Unit != character)
                        invalid = true;
                }
            }

            return !invalid;
        }

        public bool IsTileAccessible(Vector2 pos, Unit character)
        {
            bool invalid = (pos.x < 0) || (pos.y < 0) || (pos.x >= gridData.Width) || (pos.y >= gridData.Height);

            if (!invalid)
            {
                invalid = !Tiles[(int) pos.x, (int) pos.y].IsAccessible;
                if (Tiles[(int) pos.x, (int) pos.y].Unit != null)
                {
                    if (Tiles[(int) pos.x, (int) pos.y].Unit != character)
                        invalid = true;
                }
            }

            return !invalid;
        }

        public bool IsTileAccessible(Vector2 pos)
        {
            bool invalid = (pos.x < 0) || (pos.y < 0) || (pos.x >= gridData.Width) || (pos.y >= gridData.Height);

            if (!invalid)
            {
                invalid = !Tiles[(int) pos.x, (int) pos.y].IsAccessible;
            }

            return !invalid;
        }
        public bool IsTileFree(Vector2 pos)
        {
            return Tiles[(int)pos.x, (int)pos.y].Unit == null;
        }


        private bool IsMovableLocation(BigTile position, int team, Unit character)
        {
            return IsValidLocation(position.BottomLeft(), character) &&
                   IsValidLocation(position.BottomRight(), character) &&
                   IsValidLocation(position.TopLeft(), character) && IsValidLocation(position.TopRight(), character);
        }

        public bool IsBigTileAccessible(BigTile position, Unit character)
        {
            return IsTileAccessible(position.BottomLeft(), character) &&
                   IsTileAccessible(position.BottomRight(), character) &&
                   IsTileAccessible(position.TopLeft(), character) && IsTileAccessible(position.TopRight(), character);
        }

        public bool IsBigTileAccessible(BigTile position)
        {
            return IsTileAccessible(position.BottomLeft()) && IsTileAccessible(position.BottomRight()) &&
                   IsTileAccessible(position.TopLeft()) && IsTileAccessible(position.TopRight());
        }
    }
}