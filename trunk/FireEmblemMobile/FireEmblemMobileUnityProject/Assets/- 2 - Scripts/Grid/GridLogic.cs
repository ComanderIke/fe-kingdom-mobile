﻿using Assets.Core;
using Assets.GameActors.Units;
using Assets.GameActors.Units.Monsters;
using Assets.GameInput;
using Assets.Grid.PathFinding;
using Assets.Map;
using Assets.Mechanics;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Grid
{
    public class GridLogic
    {
        private readonly MainScript mainScript;
        public Tile[,] Tiles { get; set; }
        public MapSystem GridManager { get; set; }
        private readonly GridData gridData;

        public GridLogic(MapSystem gridManager)
        {
            mainScript = MainScript.Instance;
            GridManager = gridManager;
            Tiles = gridManager.Tiles;
            gridData = gridManager.GridData;
            InputSystem.OnClickedField += FieldClicked;
        }

        public void FieldClicked(int x, int y)
        {
            var selectedCharacter = mainScript.GetSystem<UnitSelectionSystem>().SelectedCharacter;
            if (Tiles[x, y].Unit == null)
            {
                if (selectedCharacter != null)
                {
                    if (Tiles[x, y].IsActive)
                    {
                        var movePath = new List<Vector2>();
                        for (int i = 0; i < GridManager.GridResources.PreferredPath.Path.Count; i++)
                        {
                            movePath.Add(new Vector2(GridManager.GridResources.PreferredPath.Path[i].x,
                                GridManager.GridResources.PreferredPath.Path[i].y));
                        }

                        mainScript.GetSystem<UnitActionSystem>().MoveCharacter(selectedCharacter, x, y, movePath);
                        UnitActionSystem.OnAllCommandsFinished += SwitchToGamePlayState;
                        Debug.Log("All Commands Setup");
                        mainScript.GetSystem<UnitActionSystem>().ExecuteActions();

                        mainScript.GetSystem<MapSystem>().HideMovement();
                        return;
                    }
                    else
                    {
                        // clicked on Field where he cant walk to
                        //Do nothing
                    }
                }
            }
        }

        private void SwitchToGamePlayState()
        {
            Debug.Log("Switch State Commands Delete");
            UnitActionSystem.OnAllCommandsFinished -= SwitchToGamePlayState;
            mainScript.GameStateManager.SwitchState(GameStateManager.GameplayState);
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
                            if (unitOnTile != null && unitOnTile.Player.Id != unit.Player.Id)
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
                        if (Math.Abs(j + i) == attackRange)
                        {
                            // Debug.Log("attackTargets at "+ unit.GridPosition.GetPos()+": " +(i + x) + " " + (j + y));
                            if (IsOutOfBounds(new Vector2(x + i, y + j)))
                                continue;
                            var unitOnTile = Tiles[i + x, j + y].Unit;
                            if (unitOnTile != null && unitOnTile.Player.Id != unit.Player.Id)
                            {
                                targets.Add(unitOnTile);
                            }
                        }
                    }
                }
            }

            return targets;
        }

        public bool IsFieldAttackable(int x, int z)
        {
            Debug.Log(x + " " + z + " " + Tiles[x, z].GameObject.GetComponent<MeshRenderer>().sharedMaterial.name +
                      " " + GridManager.GridResources.CellMaterialAttack.name);
            return Tiles[x, z].GameObject.GetComponent<MeshRenderer>().sharedMaterial ==
                   GridManager.GridResources.CellMaterialAttack;
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
                if (Tiles[(int) pos.x, (int) pos.y].Unit != null)
                    if (Tiles[(int) pos.x, (int) pos.y].Unit.Player.Id == team)
                        invalid = false;
                if (!Tiles[(int) pos.x, (int) pos.y].IsActive)
                    invalid = true;
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
                var moveOption = GetMoveableBigTileFromPosition(leftPosition, character.Player.Id, character);
                if (moveOption != null)
                    return moveOption;
            }

            if (IsValidLocation(rightPosition, character))
            {
                var moveOption = GetMoveableBigTileFromPosition(rightPosition, character.Player.Id, character);
                if (moveOption != null)
                    return moveOption;
            }

            if (IsValidLocation(topPosition, character))
            {
                var moveOption = GetMoveableBigTileFromPosition(topPosition, character.Player.Id, character);
                if (moveOption != null)
                    return moveOption;
            }

            if (IsValidLocation(bottomPosition, character))
            {
                var moveOption = GetMoveableBigTileFromPosition(bottomPosition, character.Player.Id, character);
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
                    if (field.Unit.Player.Id == team)
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
                    if (Tiles[x, y].Unit.Player.Id != team)
                    {
                        invalid = true;
                    }

                    if (isAdjacent) //TODO passthrough should be ok but not stopping on it
                    {
                        invalid = true;
                    }
                }
            }

            return !invalid;
        }

        public void ResetActiveFields()
        {
            GridManager.NodeHelper.Reset();
            for (int i = 0; i < gridData.Width; i++)
            {
                for (int j = 0; j < gridData.Height; j++)
                {
                    Tiles[i, j].IsActive = false;
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

        public MovementPath GetMonsterPath(Monster monster, BigTile position, bool adjacent, List<int> attackRanges)
        {
            MainScript.Instance.GetSystem<InputSystem>().AttackRangeFromPath = 0;
            var nodes = new PathFindingNode[gridData.Width, gridData.Height];
            for (int x = 0; x < gridData.Width; x++)
            {
                for (int y = 0; y < gridData.Height; y++)
                {
                    bool isAccessible = Tiles[x, y].IsAccessible;
                    if (Tiles[x, y].Unit != null && Tiles[x, y].Unit.Player.Id != monster.Player.Id)
                        isAccessible = false;
                    nodes[x, y] = new PathFindingNode(x, y, isAccessible);
                }
            }

            var aStar = new AStar2X2(nodes);
            var p = aStar.GetPath(((BigTilePosition) monster.GridPosition).Position, position,
                monster.Player.Id, adjacent, attackRanges);
            return p;
        }

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