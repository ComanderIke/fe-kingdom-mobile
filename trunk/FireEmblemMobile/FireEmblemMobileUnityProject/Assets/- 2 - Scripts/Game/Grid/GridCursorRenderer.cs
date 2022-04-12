using __2___Scripts.Game.Utility;
using UnityEngine;

namespace Game.Map
{
    public class GridCursorRenderer : MonoBehaviour
    {
        public Canvas canvas;
        public GameObject targetSquarePrefab;
        public Transform targetSquareParent;
        public SpriteRenderer cursorSprite;
        public int radius = 0;
        public bool horizontal;
        public bool vertical;
        public bool diagonal;
        public bool fullBox;
        public void Show(Vector2 position)
        {
            //Debug.Log("SHOW");
            gameObject.transform.localPosition = new Vector3(position.x +0.5f, position.y+0.5f,gameObject.transform.localPosition.z);
            gameObject.SetActive(true);
            //ShowTargetRange(radius, horizontal, vertical, diagonal);

        }

        public void HideTargetRange()
        {
            cursorSprite.enabled = true;
        }
        public void ShowTargetRange(int radius, bool horizontal, bool vertical, bool diagonal, bool fullBox)
        {
            cursorSprite.enabled = false;
            targetSquareParent.DeleteAllChildren();
            GameObject go;
            if (radius > 0)
            {
                if (fullBox)
                {
                    for (int i = 0; i < radius + 1; i++)
                    {
                        for (int j = 0; j< radius + 1; j++)
                        {
                            go = Instantiate(targetSquarePrefab, targetSquareParent, false);
                            go.transform.localPosition = new Vector3(i, -j, 0);
                            go = Instantiate(targetSquarePrefab, targetSquareParent, false);
                            go.transform.localPosition = new Vector3(i, j, 0);
                            go = Instantiate(targetSquarePrefab, targetSquareParent, false);
                            go.transform.localPosition = new Vector3(-i, -j, 0);
                            go = Instantiate(targetSquarePrefab, targetSquareParent, false);
                            go.transform.localPosition = new Vector3(-i, j, 0);
                        }
                    }
                }
                else
                {
                    go=Instantiate(targetSquarePrefab, targetSquareParent, false);
                    go.transform.localPosition = Vector3.zero;
                    for (int i = 1; i < radius + 1; i++)
                    {
                        if (horizontal)
                        {
                            go = Instantiate(targetSquarePrefab, targetSquareParent, false);
                            go.transform.localPosition = new Vector3(-i, 0, 0);
                            go = Instantiate(targetSquarePrefab, targetSquareParent, false);
                            go.transform.localPosition = new Vector3(i, 0, 0);
                        }

                        if (vertical)
                        {
                            go = Instantiate(targetSquarePrefab, targetSquareParent, false);
                            go.transform.localPosition = new Vector3(0, -i, 0);
                            go = Instantiate(targetSquarePrefab, targetSquareParent, false);
                            go.transform.localPosition = new Vector3(0, i, 0);
                        }

                        
                    }

                    if (diagonal)
                    {
                        for (int i = 0; i < radius; i++)
                        {
                            for (int j = 0; j < radius; j++)
                            {
                                if (i !=0 && j!=0&&(i+j)<=radius)
                                {
                                    go = Instantiate(targetSquarePrefab, targetSquareParent, false);
                                    go.transform.localPosition = new Vector3(-i, -j, 0);
                                    go = Instantiate(targetSquarePrefab, targetSquareParent, false);
                                    go.transform.localPosition = new Vector3(i, j, 0);
                                    go = Instantiate(targetSquarePrefab, targetSquareParent, false);
                                    go.transform.localPosition = new Vector3(-i, j, 0);
                                    go = Instantiate(targetSquarePrefab, targetSquareParent, false);
                                    go.transform.localPosition = new Vector3(i, -j, 0);
                                }
                               
                            }
                        }
                    }

                    
                }
            }
            else
            {
                go=Instantiate(targetSquarePrefab, targetSquareParent, false);
                go.transform.localPosition = Vector3.zero;
            }
        }
        public void ShowTileInfo()
        {
            canvas.enabled = true;
        }
        public void HideTileInfo()
        {
            canvas.enabled = false;
        }
        public void Hide()
        {
            //Debug.Log("Hide");
            gameObject.SetActive(false);
        }
    }
}