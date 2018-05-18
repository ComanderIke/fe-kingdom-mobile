using Assets.Scripts.Characters;
using Assets.Scripts.GameStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Grid
{
    public class GridRenderer
    {

        public GridSystem GridManager { get; set; }
        public Tile[,] Tiles { get; set; }

        public GridRenderer (GridSystem gridManager)
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
            if (c.GridPosition is BigTilePosition)
            {
                BigTilePosition bigTile = (BigTilePosition)c.GridPosition;

                Tiles[(int)bigTile.Position.BottomLeft().x, (int)bigTile.Position.BottomLeft().y].gameObject.GetComponent<MeshRenderer>().material = GridManager.gridRessources.cellMaterialStandOn;
                Tiles[(int)bigTile.Position.BottomRight().x, (int)bigTile.Position.BottomRight().y].gameObject.GetComponent<MeshRenderer>().material = GridManager.gridRessources.cellMaterialStandOn;
                Tiles[(int)bigTile.Position.TopLeft().x, (int)bigTile.Position.TopLeft().y].gameObject.GetComponent<MeshRenderer>().material = GridManager.gridRessources.cellMaterialStandOn;
                Tiles[(int)bigTile.Position.TopRight().x, (int)bigTile.Position.TopRight().y].gameObject.GetComponent<MeshRenderer>().material = GridManager.gridRessources.cellMaterialStandOn;
            }
            else
                Tiles[c.GridPosition.x, c.GridPosition.y].gameObject.GetComponent<MeshRenderer>().material = GridManager.gridRessources.cellMaterialStandOn;
        }

        public void SetFieldMaterial(Vector2 pos, int playerId, bool attack)
        {
            MeshRenderer m = Tiles[(int)pos.x, (int)pos.y].gameObject.GetComponent<MeshRenderer>();
            if (attack)
            {
                //not using sharedMaterial here create Material instances which will cause much higher baches
                if (m.sharedMaterial==GridManager.gridRessources.cellMaterialMovement)
                    return;
                if (MainScript.instance.GetSystem<TurnSystem>().ActivePlayer.ID == playerId)
                {
                    m.material = GridManager.gridRessources.cellMaterialAttack;
                    if (Tiles[(int)pos.x, (int)pos.y].character != null&& Tiles[(int)pos.x, (int)pos.y].character.Player.ID!=playerId)
                        MainScript.instance.GetSystem<UISystem>().ShowAttackableField((int)pos.x, (int)pos.y);
                    // MainScript.GetInstance().GetController<UIController>().ShowAttackableEnemy((int)pos.x, (int)pos.y);
                }
                else
                    m.material = GridManager.gridRessources.cellMaterialEnemyAttack;
            }
            else
            {
                m.material = GridManager.gridRessources.cellMaterialMovement;
 
                if (Tiles[(int)pos.x, (int)pos.y].character != null && Tiles[(int)pos.x, (int)pos.y].character.Player.ID != playerId)
                {
                    if (MainScript.instance.GetSystem<TurnSystem>().ActivePlayer.ID == playerId)
                    {
                        m.material = GridManager.gridRessources.cellMaterialAttack;
                        MainScript.instance.GetSystem<UISystem>().ShowAttackableField((int)pos.x, (int)pos.y);
                        // MainScript.GetInstance().GetController<UIController>().ShowAttackableEnemy((int)pos.x, (int)pos.y);
                    }
                    else
                        m.material = GridManager.gridRessources.cellMaterialEnemyAttack;
                }
                Tiles[(int)pos.x, (int)pos.y].isActive = true;
            }
        }
        
        /*===========*/

        
       
    }
}
