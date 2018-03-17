using Assets.Scripts.Characters;
using Assets.Scripts.GameStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Grid
{
    public class GridRenderer
    {

        public GridManager GridManager { get; set; }
        public Tile[,] Tiles { get; set; }

        public GridRenderer (GridManager gridManager)
        {
            GridManager = gridManager;
            Tiles = gridManager.Tiles;
        }
        public void SetBigTileActive(BigTile bigTile, int playerId, bool attack)
        {
            SetFieldTexture(bigTile.BottomLeft(), playerId, attack);
            SetFieldTexture(bigTile.BottomRight(), playerId, attack);
            SetFieldTexture(bigTile.TopLeft(), playerId, attack);
            SetFieldTexture(bigTile.TopRight(), playerId, attack);
        }

        public IEnumerator FieldAnimation()
        {
            float scale = 0.9f;
            List<GameObject> animFields = new List<GameObject>();

            foreach (Tile f in Tiles)
            {
                MeshRenderer m = f.gameObject.GetComponent<MeshRenderer>();
                if (f.isActive || m.material.mainTexture == GridManager.gridRessources.AttackTexture)
                {
                    animFields.Add(f.gameObject);
                }
            }
            foreach (GameObject go in animFields)
            {
                go.transform.localScale = new Vector3(scale, scale, scale);
            }
            yield return new WaitForSeconds(0.15f);
            while (scale <= 1)
            {
                foreach (GameObject go in animFields)
                {
                    go.transform.localScale = new Vector3(scale, scale, scale);
                }
                scale += 0.01f;
                yield return new WaitForSeconds(0.02f);
            }
            scale = 1;
            foreach (GameObject go in animFields)
            {
                go.transform.localScale = new Vector3(scale, scale, scale);
            }
        }

        public void ShowStandOnTexture(LivingObject c)
        {
            if (c.GridPosition is BigTilePosition)
            {
                BigTilePosition bigTile = (BigTilePosition)c.GridPosition;

                Tiles[(int)bigTile.Position.BottomLeft().x, (int)bigTile.Position.BottomLeft().y].gameObject.GetComponent<MeshRenderer>().material.mainTexture = GridManager.gridRessources.StandOnTexture;
                Tiles[(int)bigTile.Position.BottomRight().x, (int)bigTile.Position.BottomRight().y].gameObject.GetComponent<MeshRenderer>().material.mainTexture = GridManager.gridRessources.StandOnTexture;
                Tiles[(int)bigTile.Position.TopLeft().x, (int)bigTile.Position.TopLeft().y].gameObject.GetComponent<MeshRenderer>().material.mainTexture = GridManager.gridRessources.StandOnTexture;
                Tiles[(int)bigTile.Position.TopRight().x, (int)bigTile.Position.TopRight().y].gameObject.GetComponent<MeshRenderer>().material.mainTexture = GridManager.gridRessources.StandOnTexture;
            }
            else
                Tiles[c.GridPosition.x, c.GridPosition.y].gameObject.GetComponent<MeshRenderer>().material.mainTexture = GridManager.gridRessources.StandOnTexture;
        }

        public void SetFieldTexture(Vector2 pos, int playerId, bool attack)
        {
            MeshRenderer m = Tiles[(int)pos.x, (int)pos.y].gameObject.GetComponent<MeshRenderer>();
            if (attack)
            {
                if (m.material.mainTexture == GridManager.gridRessources.MoveTexture || m.material.mainTexture == GridManager.gridRessources.MoveTexture2)
                    return;
                if (MainScript.GetInstance().GetSystem<TurnManager>().ActivePlayer.ID == playerId)
                {
                    m.material.mainTexture = GridManager.gridRessources.AttackTexture;
                    if (Tiles[(int)pos.x, (int)pos.y].character != null&& Tiles[(int)pos.x, (int)pos.y].character.Player.ID!=playerId)
                        MainScript.GetInstance().GetController<UIController>().ShowAttackableEnemy((int)pos.x, (int)pos.y);
                }
                else
                    m.material.mainTexture = GridManager.gridRessources.EnemyAttackTexture;
            }
            else
            {
                if (MainScript.GetInstance().GetSystem<TurnManager>().ActivePlayer.ID == playerId)
                    m.material.mainTexture = GridManager.gridRessources.MoveTexture;
                else
                    m.material.mainTexture = GridManager.gridRessources.MoveTexture2;
                if (Tiles[(int)pos.x, (int)pos.y].character != null && Tiles[(int)pos.x, (int)pos.y].character.Player.ID != playerId)
                {
                    if (MainScript.GetInstance().GetSystem<TurnManager>().ActivePlayer.ID == playerId)
                    {
                        m.material.mainTexture = GridManager.gridRessources.AttackTexture;
                        MainScript.GetInstance().GetController<UIController>().ShowAttackableEnemy((int)pos.x, (int)pos.y);
                    }
                    else
                        m.material.mainTexture = GridManager.gridRessources.EnemyAttackTexture;
                }
                Tiles[(int)pos.x, (int)pos.y].isActive = true;
            }
        }
        
        /*===========*/

        
       
    }
}
