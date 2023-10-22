using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using Game.GameResources;
using Game.Grid;
using Game.Grid.GridPathFinding;
using Game.Manager;
using Game.Mechanics;
using GameEngine;
using UnityEngine;
using EffectType = _02_Scripts.Game.GameActors.Items.Consumables.EffectType;

namespace Game.Map
{
    [Serializable]
    [RequireComponent(typeof(GridBuilder))]
    public class GridSystem : MonoBehaviour, IEngineSystem
    {
        public const float GRID_X_OFFSET = 0.0f;
        private string MapName;
 
        public Tile[,] Tiles { get; private set; }
        public GridRenderer GridRenderer { get; set; }
        public GridLogic GridLogic { get; set; }
        public NodeHelper NodeHelper;
        public IPathFinder pathFinder { get; set; }
        public GridCursor cursor { get; set; }
        public int width;
        public int height;

        private void Awake()
        {
            width = GridGameManager.Instance.BattleMap.width;
            height= GridGameManager.Instance.BattleMap.height;
            GetComponent<GridBuilder>().Build(width,height);
            Tiles = GetComponent<GridBuilder>().GetTiles();

            cursor = new GridCursor();
            GridRenderer = new GridRenderer(this);
            GridLogic = new GridLogic(this);
           
            NodeHelper = new NodeHelper(width,height);
            
        }

        public void Init()
        {
          
        }

        private void OnDisable()
        {
            Deactivate();
        }

        public void Deactivate()
        {
            UnitSelectionSystem.OnDeselectCharacter -=  InvokeHideMoveRange;
            UnitSelectionSystem.OnSelectedCharacter -= SelectedCharacter;
            UnitSelectionSystem.OnEnemySelected -= OnEnemySelected;
            UnitSelectionSystem.OnSelectedInActiveCharacter -= OnEnemySelected;
            MovementState.OnMovementFinished -= InvokeHideMoveRange;
            Unit.UnitDied -= RemoveUnitFromGrid;
        }

        public void Activate()
        {
            UnitSelectionSystem.OnDeselectCharacter += InvokeHideMoveRange;
            UnitSelectionSystem.OnSelectedCharacter += SelectedCharacter;
            UnitSelectionSystem.OnEnemySelected += OnEnemySelected;
            UnitSelectionSystem.OnSelectedInActiveCharacter += OnEnemySelected;
            MovementState.OnMovementFinished += InvokeHideMoveRange;
            
            Unit.UnitDied += RemoveUnitFromGrid;
        }

        private void InvokeHideMoveRange(IGridActor u)
        {
            HideMoveRange();
        }
        public ITileChecker GetTileChecker()
        {
            return GridLogic.tileChecker;
        }
        void RemoveUnitFromGrid(IGridActor u)
        {
            Tiles[u.GridComponent.GridPosition.X, u.GridComponent.GridPosition.Y].GridObject = null;
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
            if(!gridActor.TurnStateManager.HasMoved)
                ShowMovementRangeOnGrid(gridActor);
            //if (!u.UnitTurnState.HasAttacked)
            ShowAttackRangeOnGrid(gridActor, new List<int>(gridActor.AttackRanges));

        }

        public void ShowMovementRangeOnGrid(IGridActor c, bool cantoRange=false)
        {
            ShowMovement(c.GridComponent.GridPosition.X, c.GridComponent.GridPosition.Y, cantoRange?c.GridComponent.Canto:c.MovementRange, 0, c);
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
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j <height; j++)
                {
                    Tiles[i, j].Reset();
                }
            }
            GridLogic.ResetActiveFields();
            NodeHelper.Reset();
        }

        public void ShowCastRange(IGridActor character, int castRange, int minRange)
        {
            HideMoveRange();
            Debug.Log("AFter HideMoveRange");
            Vector2 characterPos = character.GridComponent.GridPosition.AsVector();
            for (int i = 0; i < castRange+1; i++)
            {
                for (int j = 0; j < castRange+1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;
                    if (i + j <= castRange&& i+j >=minRange)
                    {
                        var pos = characterPos + new Vector2(i, j);
                        ShowCastRange(pos,character.Faction.Id);

                        pos = characterPos + new Vector2(-i, j);
                        ShowCastRange(pos,character.Faction.Id);

                        pos = characterPos + new Vector2(i, -j);
                        ShowCastRange(pos,character.Faction.Id);

                        pos = characterPos + new Vector2(-i, -j);
                        ShowCastRange(pos,character.Faction.Id);
                    }
                }
            }
            
        }
        public void ShowAttackRecursive(IGridActor character, int x, int y, int range, List<int> direction, bool soft=false)
        {
            if (range <= 0)
            {
                if (!IsTileMoveableAndActive(x, y))
                {
                    GridRenderer.SetFieldMaterialAttack(new Vector2(x, y), character.Faction.Id, !character.TurnStateManager.IsWaiting, GridGameManager.Instance.FactionManager.IsActiveFaction(character.Faction));
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

            GridRenderer.SetFieldMaterial(new Vector2(x, y), unit.Faction.Id, !unit.TurnStateManager.HasMoved,GridGameManager.Instance.FactionManager.IsActiveFaction(unit.Faction));
            GridLogic.gridSessionData.AddValidPosition(x, y);
            NodeHelper.Nodes[x, y].C = c;
            c+=GetTileChecker().GetMovementCost(x, y, unit);
            if (GridLogic.CheckField(x - 1, y, unit, range) && NodeHelper.NodeFaster(x - 1, y, c))
                ShowMovement(x - 1, y, range - GetTileChecker().GetMovementCost(x-1, y, unit), c, unit);
            if (GridLogic.CheckField(x + 1, y, unit, range) && NodeHelper.NodeFaster(x + 1, y, c))
                ShowMovement(x + 1, y, range -GetTileChecker().GetMovementCost(x+1, y, unit), c, unit);
            if (GridLogic.CheckField(x, y - 1, unit, range) && NodeHelper.NodeFaster(x, y - 1, c))
                ShowMovement(x, y - 1, range - GetTileChecker().GetMovementCost(x, y - 1, unit), c, unit);
            if (GridLogic.CheckField(x, y + 1, unit, range) && NodeHelper.NodeFaster(x, y + 1, c))
                ShowMovement(x, y + 1, range - GetTileChecker().GetMovementCost(x, y+1, unit), c, unit);
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
            return x < 0 || x >= width || y < 0 ||
                   y >= height;
        }
        public bool IsOutOfBounds(Vector2 pos)
        {
            return pos.x < 0 || pos.x >= width || pos.y < 0 ||
                   pos.y >= height;
        }

        public Tile GetTile(int x, int y)
        {
            return Tiles[x, y];
        }

        public void SwapUnits(IGridActor unit,IGridActor unit2)
        {
            Debug.Log("Swap Units:  "+((Unit)unit).name+" "+((Unit)unit2).name);
            var tmpPosX = unit.GridComponent.GridPosition.X;
            var tmpPosY = unit.GridComponent.GridPosition.Y;
            var tmpPos2X = unit2.GridComponent.GridPosition.X;
            var tmpPos2Y = unit2.GridComponent.GridPosition.Y;
            Tiles[unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y].GridObject = null;
            Tiles[unit2.GridComponent.GridPosition.X, unit2.GridComponent.GridPosition.Y].GridObject = null;
            Tiles[tmpPos2X, tmpPos2Y].GridObject = unit;
            unit.SetGridPosition( Tiles[tmpPos2X, tmpPos2Y]);
            Tiles[tmpPosX, tmpPosY].GridObject = unit2;
            unit2.SetGridPosition(Tiles[tmpPosX, tmpPosY]);
            
        }

        public void SetUnitPosition(IGridActor unit, Tile tile, bool deleteOldGridObject=true, bool moveGameObject=true)
        {
            SetUnitPosition(unit, tile.X, tile.Y, deleteOldGridObject, moveGameObject);
        }
        public void SetUnitPosition(IGridActor unit, int x, int y, bool deleteOldGridObject=true, bool moveGameObject=true)
        {
            if (x != -1 && y != -1 && x < width && y < height)
            {
                if (unit.GridComponent.GridPosition.X != -1 && unit.GridComponent.GridPosition.Y != -1)
                {
                    if(deleteOldGridObject|| (Tiles[unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y].GridObject!=null &&Tiles[unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y].GridObject.Equals(unit)))
                        Tiles[unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y].GridObject = null;
                }
                
                
                unit.SetGridPosition(Tiles[x,y], moveGameObject);
                Tiles[x, y].GridObject = unit;
                
            }
            else
            {
                Debug.LogError("Out of Bounds: "+x+" "+y +" "+unit);
            }
        }
        // public void SetUnitInternPosition(IGridActor unit, int x, int y)
        // {
        //     if (x != -1 && y != -1)
        //     {
        //         if (unit.GridComponent.GridPosition.X != -1 && unit.GridComponent.GridPosition.Y != -1)
        //         {
        //
        //             Tiles[unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y].GridObject = null;
        //         }
        //         Debug.Log("UNIT: "+unit);
        //         Tiles[x, y].GridObject = unit;
        //         unit.GridComponent.SetInternPosition(x, y);
        //     }
        // }

        public List<Tile> GetActiveTiles()
        {
            List<Tile> activeTiles = new List<Tile>();
            foreach (var tile in Tiles)
            {
                if (GridLogic.gridSessionData.IsMoveableAndActive(tile.X, tile.Y))
                {
                    activeTiles.Add(tile);
                }
            }

            return activeTiles;
        }

        private void ShowCastRange(Vector2 pos, FactionId id)
        {
            if (!IsOutOfBounds(pos))
            {
                GridRenderer.SetTileCastMaterial(pos, id);
                GridLogic.gridSessionData.AddValidTargetPosition(pos);
            }
        }
        public void ShowRootedCastRange(IGridActor character, int level, PositionTargetSkillMixin pts)
        {
            HideMoveRange();
            Vector2 characterPos = character.GridComponent.GridPosition.AsVector();
            Debug.Log("CAST RANGE: "+pts.GetRange(level));
            if (pts.GetRange(level) > 0)
            {
                for (int i = 0; i <= pts.GetRange(level); i++)
                {
                    for (int j = 0; j <= pts.GetRange(level); j++)
                    {
                        if(i==0&&j==0)
                            continue;
                        if ((i+j)<=pts.GetRange(level))
                        {
                            var pos = characterPos + new Vector2(i, j);
                            ShowCastRange(pos,character.Faction.Id);
                            pos = characterPos + new Vector2(-i, j);
                            ShowCastRange(pos,character.Faction.Id);
                            pos = characterPos + new Vector2(i, -j);
                            ShowCastRange(pos,character.Faction.Id);
                            pos = characterPos + new Vector2(-i, -j);
                            ShowCastRange(pos,character.Faction.Id);
                        }
                       
                    }
                }
                // if (pts.TargetArea == SkillTargetArea.Block)
                // {
                //     for (int i = 0; i < pts.GetSize(level) + 1; i++)
                //     {
                //         for (int j = 0; j< pts.GetSize(level)+ 1; j++)
                //         {
                //             var pos = characterPos + new Vector2(i, j);
                //             ShowCastRange(pos,character.Faction.Id);
                //             pos = characterPos + new Vector2(-i, j);
                //             ShowCastRange(pos,character.Faction.Id);
                //             pos = characterPos + new Vector2(i, -j);
                //             ShowCastRange(pos,character.Faction.Id);
                //             pos = characterPos + new Vector2(-i, -j);
                //             ShowCastRange(pos,character.Faction.Id);
                //         }
                //     }
                // }
                // else
                // {
                //     for (int i = 1; i < pts.GetSize(level) + 1; i++)
                //     {
                //         if (pts.TargetArea == SkillTargetArea.Line||pts.TargetArea == SkillTargetArea.Star||pts.TargetArea == SkillTargetArea.Cross)
                //         {
                //             var pos = characterPos + new Vector2(-i, 0);
                //             ShowCastRange(pos,character.Faction.Id);
                //             pos = characterPos + new Vector2(i, 0);
                //             ShowCastRange(pos,character.Faction.Id);
                //           
                //         }
                //
                //         if (pts.TargetArea == SkillTargetArea.NormalLine||pts.TargetArea == SkillTargetArea.Star||pts.TargetArea == SkillTargetArea.Cross)
                //         {
                //             var pos = characterPos + new Vector2(0, -i);
                //             ShowCastRange(pos,character.Faction.Id);
                //             pos = characterPos + new Vector2(0, i);
                //             ShowCastRange(pos,character.Faction.Id);
                //         }
                //
                //         
                //     }
                //
                //     if (pts.TargetArea == SkillTargetArea.Star)
                //     {
                //         for (int i = 0; i < pts.GetSize(level); i++)
                //         {
                //             for (int j = 0; j < pts.GetSize(level); j++)
                //             {
                //                 if (i !=0 && j!=0&&(i+j)<=pts.GetSize(level))
                //                 {
                //                     var pos = characterPos + new Vector2(i, j);
                //                     ShowCastRange(pos,character.Faction.Id);
                //                     pos = characterPos + new Vector2(-i, j);
                //                     ShowCastRange(pos,character.Faction.Id);
                //                     pos = characterPos + new Vector2(i, -j);
                //                     ShowCastRange(pos,character.Faction.Id);
                //                     pos = characterPos + new Vector2(-i, -j);
                //                     ShowCastRange(pos,character.Faction.Id);
                //                 }
                //                
                //             }
                //         }
                //     }
            // }
            }
            
                      
                    
                
            
        }
        
        public bool IsTargetAble(int x, int y)
        {
            return GridLogic.IsFieldTargetable(x, y);
        }

        
        public void SetGridObjectPosition(IGridObject dest, int x, int y)
        {
            if (x != -1 && y != -1 && x < width && y < height)
            {
                if (dest.GridComponent.GridPosition.X != -1 && dest.GridComponent.GridPosition.Y != -1)
                {

                    Tiles[dest.GridComponent.GridPosition.X, dest.GridComponent.GridPosition.Y].GridObject = null;
                }

                Tiles[x, y].GridObject = dest;
                dest.GridComponent.GridPosition.SetPosition(x, y);
            }
            else
            {
                Debug.LogError("Out of Bounds: "+x+" "+y +" "+dest);
            }
        }

        public void HideCast()
        {
          
            HideMoveRange();
        }

        public void ShowCast(int radius, SkillTargetArea skillTargetArea, EffectType effectType)
        {
            Debug.Log("Show Cast: "+radius+ " "+skillTargetArea+" "+cursor.GetPosition());
            if (radius > 0)
            {
                if (skillTargetArea==SkillTargetArea.Block)
                {
                    for (int i = 0; i < radius + 1; i++)
                    {
                        for (int j = 0; j< radius + 1; j++)
                        {
                            GridRenderer.SetTileCastCursorMaterial(new Vector2(i,-j)+cursor.GetPosition(),effectType,0);
                            GridRenderer.SetTileCastCursorMaterial(new Vector2(i,j)+cursor.GetPosition(),effectType,0);
                            GridRenderer.SetTileCastCursorMaterial(new Vector2(-i,-j)+cursor.GetPosition(),effectType,0);
                            GridRenderer.SetTileCastCursorMaterial(new Vector2(-i,j)+cursor.GetPosition(),effectType,0);
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < radius + 1; i++)
                    {
                        if (skillTargetArea==SkillTargetArea.Line||skillTargetArea==SkillTargetArea.Cross||skillTargetArea==SkillTargetArea.Star)
                        {
                            GridRenderer.SetTileCastCursorMaterial(new Vector2(i,0)+cursor.GetPosition(),effectType,0);
                            GridRenderer.SetTileCastCursorMaterial(new Vector2(-i,0)+cursor.GetPosition(),effectType,0);
                 
                        }

                        if (skillTargetArea==SkillTargetArea.NormalLine||skillTargetArea==SkillTargetArea.Cross||skillTargetArea==SkillTargetArea.Star)
                        {
                            GridRenderer.SetTileCastCursorMaterial(new Vector2(0,-i)+cursor.GetPosition(),effectType,0);
                            GridRenderer.SetTileCastCursorMaterial(new Vector2(0,i)+cursor.GetPosition(),effectType,0);
                         
                        }

                        
                    }

                    if (skillTargetArea==SkillTargetArea.Star)
                    {
                        for (int i = 0; i < radius; i++)
                        {
                            for (int j = 0; j < radius; j++)
                            {
                                if (i !=0 && j!=0&&(i+j)<=radius)
                                {
                                    GridRenderer.SetTileCastCursorMaterial(new Vector2(i,-j)+cursor.GetPosition(),effectType,0);
                                    GridRenderer.SetTileCastCursorMaterial(new Vector2(i,j)+cursor.GetPosition(),effectType,0);
                                    GridRenderer.SetTileCastCursorMaterial(new Vector2(-i,-j)+cursor.GetPosition(),effectType,0);
                                    GridRenderer.SetTileCastCursorMaterial(new Vector2(-i,j)+cursor.GetPosition(),effectType,0);
                                }
                               
                            }
                        }
                    }

                    
                }
            }
           
        }

        public void ShowRootedCast(Vector2 startPos, int radius, SkillTargetArea skillTargetArea, EffectType effectType, int clickedX, int clickedY)
        {
            Debug.Log("Show Cast: "+radius+ " "+skillTargetArea+" "+startPos);
            if (radius > 0)
            {
                if (skillTargetArea==SkillTargetArea.Block)
                {
                    for (int i = 0; i < radius + 1; i++)
                    {
                        for (int j = 0; j< radius + 1; j++)
                        {
                            GridRenderer.SetTileCastCursorMaterial(new Vector2(i,-j)+startPos,effectType,0);
                            GridRenderer.SetTileCastCursorMaterial(new Vector2(i,j)+startPos,effectType,0);
                            GridRenderer.SetTileCastCursorMaterial(new Vector2(-i,-j)+startPos,effectType,0);
                            GridRenderer.SetTileCastCursorMaterial(new Vector2(-i,j)+startPos,effectType,0);
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < radius + 1; i++)
                    {
                        if (skillTargetArea==SkillTargetArea.Line||skillTargetArea==SkillTargetArea.Cross||skillTargetArea==SkillTargetArea.Star)
                        {
                            // if (Math.Abs(i + startPos.x - clickedX) < 0.01f)
                            // {
                                if (clickedX > startPos.x)
                                {
                                    GridRenderer.SetTileCastCursorMaterial(new Vector2(i, 0) + startPos, effectType, 0);
                                }
                                else if(clickedX< startPos.x)
                                {

                                    GridRenderer.SetTileCastCursorMaterial(new Vector2(-i, 0) + startPos, effectType,
                                        0);
                                }
                            // }

                        }

                        if (skillTargetArea==SkillTargetArea.Line||skillTargetArea==SkillTargetArea.Cross||skillTargetArea==SkillTargetArea.Star)
                        {
                            // if (Math.Abs(i + startPos.y - clickedX) < 0.01f)
                            // {
                                if (clickedY < startPos.y)
                                {
                                    GridRenderer.SetTileCastCursorMaterial(new Vector2(0, -i) + startPos, effectType,
                                        0);
                                }
                                else if(clickedY> startPos.y)
                                {
                                    GridRenderer.SetTileCastCursorMaterial(new Vector2(0, i) + startPos, effectType, 0);
                                }
                            // }

                        }

                        
                    }

                    if (skillTargetArea==SkillTargetArea.Star)
                    {
                        for (int i = 0; i < radius; i++)
                        {
                            for (int j = 0; j < radius; j++)
                            {
                                if (i !=0 && j!=0&&(i+j)<=radius)
                                {
                                    GridRenderer.SetTileCastCursorMaterial(new Vector2(i,-j)+startPos,effectType,0);
                                    GridRenderer.SetTileCastCursorMaterial(new Vector2(i,j)+startPos,effectType,0);
                                    GridRenderer.SetTileCastCursorMaterial(new Vector2(-i,-j)+startPos,effectType,0);
                                    GridRenderer.SetTileCastCursorMaterial(new Vector2(-i,j)+startPos,effectType,0);
                                }
                               
                            }
                        }
                    }

                    
                }
            }
        }
    }
}