using Assets.Core;
using Assets.GameActors.Units;
using Assets.GUI;
using Assets.Map;
using UnityEngine;

namespace Assets.Grid
{
    public class GridRenderer
    {
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
            if (c.GridPosition is BigTilePosition bigTile)
            {
                Tiles[(int) bigTile.Position.BottomLeft().x, (int) bigTile.Position.BottomLeft().y].GameObject
                    .GetComponent<MeshRenderer>().material = GridManager.GridResources.CellMaterialStandOn;
                Tiles[(int) bigTile.Position.BottomRight().x, (int) bigTile.Position.BottomRight().y].GameObject
                    .GetComponent<MeshRenderer>().material = GridManager.GridResources.CellMaterialStandOn;
                Tiles[(int) bigTile.Position.TopLeft().x, (int) bigTile.Position.TopLeft().y].GameObject
                    .GetComponent<MeshRenderer>().material = GridManager.GridResources.CellMaterialStandOn;
                Tiles[(int) bigTile.Position.TopRight().x, (int) bigTile.Position.TopRight().y].GameObject
                    .GetComponent<MeshRenderer>().material = GridManager.GridResources.CellMaterialStandOn;
            }
            else
                Tiles[c.GridPosition.X, c.GridPosition.Y].GameObject.GetComponent<MeshRenderer>().material =
                    GridManager.GridResources.CellMaterialStandOn;
        }

        public void SetFieldMaterial(Vector2 pos, int playerId, bool attack)
        {
            var meshRenderer = Tiles[(int) pos.x, (int) pos.y].GameObject.GetComponent<MeshRenderer>();
            if (attack)
            {
                //not using sharedMaterial here create Material instances which will cause much higher baches
                if (meshRenderer.sharedMaterial == GridManager.GridResources.CellMaterialMovement)
                    return;
                if (MainScript.Instance.PlayerManager.ActivePlayer.Id == playerId)
                {
                    meshRenderer.material = GridManager.GridResources.CellMaterialAttack;
                    if (Tiles[(int) pos.x, (int) pos.y].Unit != null &&
                        Tiles[(int) pos.x, (int) pos.y].Unit.Player.Id != playerId)
                        MainScript.Instance.GetSystem<UiSystem>().ShowAttackableField((int) pos.x, (int) pos.y);
                    // MainScript.GetInstance().GetController<UIController>().ShowAttackableEnemy((int)pos.x, (int)pos.y);
                }
                else
                    meshRenderer.material = GridManager.GridResources.CellMaterialEnemyAttack;
            }
            else
            {
                meshRenderer.material = GridManager.GridResources.CellMaterialMovement;

                if (Tiles[(int) pos.x, (int) pos.y].Unit != null &&
                    Tiles[(int) pos.x, (int) pos.y].Unit.Player.Id != playerId)
                {
                    if (MainScript.Instance.PlayerManager.ActivePlayer.Id == playerId)
                    {
                        meshRenderer.material = GridManager.GridResources.CellMaterialAttack;
                        MainScript.Instance.GetSystem<UiSystem>().ShowAttackableField((int) pos.x, (int) pos.y);
                        // MainScript.GetInstance().GetController<UIController>().ShowAttackableEnemy((int)pos.x, (int)pos.y);
                    }
                    else
                        meshRenderer.material = GridManager.GridResources.CellMaterialEnemyAttack;
                }

                Tiles[(int) pos.x, (int) pos.y].IsActive = true;
            }
        }
    }
}