using Assets.Core;
using Assets.GameActors.Players;
using Assets.GameActors.Units;
using Assets.GUI;
using Assets.Map;
using System;
using UnityEngine;

namespace Assets.Grid
{
    public class GridRenderer
    {
        public delegate void OnRenderEnemyTileEvent(int x, int y, Unit enemy, int playerId);
        public static event OnRenderEnemyTileEvent OnRenderEnemyTile;
        public MapSystem GridManager { get; set; }
        public Tile[,] Tiles { get; set; }

        public GridRenderer(MapSystem gridManager)
        {
            GridManager = gridManager;
            Tiles = gridManager.Tiles;
        }

        public void SetBigTileActive(BigTile bigTile, int playerId, bool attack)
        {
            SetFieldMaterial(bigTile.BottomLeft(), playerId, attack);
            SetFieldMaterial(bigTile.BottomRight(), playerId, attack);
            SetFieldMaterial(bigTile.TopLeft(), playerId, attack);
            SetFieldMaterial(bigTile.TopRight(), playerId, attack);
        }

        public void ShowStandOnTexture(Unit c)
        {
            Tiles[c.GridPosition.X, c.GridPosition.Y].spriteRenderer.sprite =
                    GridManager.GridResources.GridSpriteStandOn;
        }
        private void SetAttackFieldMaterial(Vector2 pos, int playerId, bool soft)
        {
            var meshRenderer = Tiles[(int)pos.x, (int)pos.y].spriteRenderer;
            if (soft)
                meshRenderer.sprite = GridManager.GridResources.GridAttackSpriteEnemy;
            else
            {
                meshRenderer.sprite = GridManager.GridResources.GridAttackSprite;
                OnRenderEnemyTile?.Invoke((int)pos.x, (int)pos.y, Tiles[(int)pos.x, (int)pos.y].Unit, playerId);
            }
            Tiles[(int)pos.x, (int)pos.y].IsAttackable = true;

        }
        public void SetFieldMaterial(Vector2 pos, int playerId, bool attack, bool soft=false)
        {
            var meshRenderer = Tiles[(int) pos.x, (int) pos.y].spriteRenderer;
            if (attack)
            {
                //not using sharedMaterial here create Material instances which will cause much higher batches
                if (Tiles[(int)pos.x, (int)pos.y].IsAttackable||meshRenderer.sprite == GridManager.GridResources.GridMoveSpriteEnemy || Tiles[(int)pos.x, (int)pos.y].IsActive)
                    return;
                SetAttackFieldMaterial(pos, playerId, soft);
            }
            else
            {
                if (Tiles[(int)pos.x, (int)pos.y].IsAttackable || Tiles[(int)pos.x, (int)pos.y].IsActive || meshRenderer.sprite == GridManager.GridResources.GridMoveSpriteEnemy)
                    return;
                if (Tiles[(int)pos.x, (int)pos.y].Unit != null && Tiles[(int)pos.x, (int)pos.y].Unit.Faction.Id == playerId)
                {
                    meshRenderer.sprite = GridManager.GridResources.GridSpriteAlly;
                    Tiles[(int)pos.x, (int)pos.y].IsActive = true;
                }
                else
                {
                    if (soft)
                        meshRenderer.sprite = GridManager.GridResources.GridMoveSpriteEnemy;
                    else
                    {
                        meshRenderer.sprite = GridManager.GridResources.GridMoveSprite;
                        Tiles[(int)pos.x, (int)pos.y].IsActive = true;
                    }

                }
                if (Tiles[(int) pos.x, (int) pos.y].Unit != null &&
                    Tiles[(int) pos.x, (int) pos.y].Unit.Faction.Id != playerId&&!Tiles[(int)pos.x, (int)pos.y].IsAttackable)
                {
                    SetAttackFieldMaterial(pos, playerId,soft);
                }

                
            }
        }
    }
}