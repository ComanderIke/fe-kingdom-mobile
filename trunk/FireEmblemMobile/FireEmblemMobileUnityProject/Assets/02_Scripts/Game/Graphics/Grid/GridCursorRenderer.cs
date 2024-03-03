using Game.GameActors.Units.Skills.Enums;
using Game.Utility;
using UnityEngine;

namespace Game.Graphics.Grid
{
    public class GridCursorRenderer : MonoBehaviour
    {
        public Canvas canvas;
        public GameObject targetSquarePrefab;
        public Transform targetSquareParent;
        public SpriteRenderer cursorSprite;
        public int radius = 0;
        public SkillTargetArea skillTargetArea;
   
        public void Show(Vector2 position)
        {
            //Debug.Log("SHOW");
            gameObject.transform.localPosition = new Vector3(position.x +0.5f, position.y+0.5f,gameObject.transform.localPosition.z);
            gameObject.SetActive(true);
            //ShowTargetRange(radius, horizontal, vertical, diagonal);

        }

        public void HideTargetRange()
        {
            targetSquareParent.DeleteAllChildren();
            cursorSprite.enabled = true;
        }
        public void ShowRootedTargetRange(Vector2 rootedPos,Vector2 cursorPos,  int radius, SkillTargetArea targetArea)
        {
           //Difference between castRange Properties and ACtual Attack (
           if(targetArea==SkillTargetArea.Line)
               ShowRootedLine(rootedPos,cursorPos,radius);
        }

        private void ShowRootedLine(Vector2 rootedPos,Vector2 clickedPos, int length)
        {
            cursorSprite.enabled = false;
            targetSquareParent.DeleteAllChildren();
            GameObject go;
            if (radius > 0)
            {
                for (int i = 1; i < length + 1; i++)
                {
                    go = Instantiate(targetSquarePrefab, targetSquareParent, false);
                    if ((int)clickedPos.x == (int)rootedPos.x)
                    {
                        //vertical
                        if (clickedPos.y > rootedPos.y)//up
                        {
                            
                            go.transform.localPosition = new Vector3(0, i, 0);
                        }
                        else
                        {
                            go.transform.localPosition = new Vector3(0, -i, 0);
                        }
                    }
                    else
                    {
                        if (clickedPos.x > rootedPos.x)//right
                        {
                            
                            go.transform.localPosition = new Vector3(i, 0, 0);
                        }
                        else
                        {
                            go.transform.localPosition = new Vector3(-i, 0, 0);
                        }
              
                        
                    }
                }
            }
        }
        public void ShowTargetRange(int radius, SkillTargetArea skillTargetArea)
        {
            cursorSprite.enabled = false;
            targetSquareParent.DeleteAllChildren();
            GameObject go;
            if (radius > 0)
            {
                if (skillTargetArea==SkillTargetArea.Block)
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
                        if (skillTargetArea==SkillTargetArea.Line||skillTargetArea==SkillTargetArea.Cross||skillTargetArea==SkillTargetArea.Star)
                        {
                            go = Instantiate(targetSquarePrefab, targetSquareParent, false);
                            go.transform.localPosition = new Vector3(-i, 0, 0);
                            go = Instantiate(targetSquarePrefab, targetSquareParent, false);
                            go.transform.localPosition = new Vector3(i, 0, 0);
                        }

                        if (skillTargetArea==SkillTargetArea.NormalLine||skillTargetArea==SkillTargetArea.Cross||skillTargetArea==SkillTargetArea.Star)
                        {
                            go = Instantiate(targetSquarePrefab, targetSquareParent, false);
                            go.transform.localPosition = new Vector3(0, -i, 0);
                            go = Instantiate(targetSquarePrefab, targetSquareParent, false);
                            go.transform.localPosition = new Vector3(0, i, 0);
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