using Assets.Scripts.Characters;
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
        public void SetBigTileActive(BigTile bigTile)
        {
            SetFieldTexture(bigTile.BottomLeft());
            SetFieldTexture(bigTile.BottomRight());
            SetFieldTexture(bigTile.TopLeft());
            SetFieldTexture(bigTile.TopRight());
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
            if (c is Monster)
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

        private void SetFieldTexture(Vector2 pos)
        {
            MeshRenderer m = Tiles[(int)pos.x, (int)pos.y].gameObject.GetComponent<MeshRenderer>();
            m.material.mainTexture = GridManager.gridRessources.MoveTexture;
            Tiles[(int)pos.x, (int)pos.y].isActive = true;
        }
        
        /*===========*/

        
       
    }
}
