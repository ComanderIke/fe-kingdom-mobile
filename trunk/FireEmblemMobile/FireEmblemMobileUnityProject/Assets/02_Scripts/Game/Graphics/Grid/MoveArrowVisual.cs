using System;
using System.Collections.Generic;
using Game.GameInput;
using Game.Manager;
using Game.Mechanics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Object = UnityEngine.Object;

namespace Game.Graphics
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/MoveArrow", fileName =  "MoveArrow")]
    public class MoveArrowVisual : ScriptableObject, IMovePathVisual
    {
        private GameObject moveCursor;
        private GameObject moveCursorStart;
        private SpriteRenderer moveCursorStartSpriteRenderer;
        private List<GameObject> instantiatedMovePath;
        private bool movementPathVisible = false;
        [SerializeField]
        private GameObject moveCursorPrefab;
        [SerializeField]
        private GameObject moveArrowPartPrefab;
        [SerializeField]
        private MoveArrowSprites moveArrowSprites;
        private Transform parent;

        public void Reset()
        {
            instantiatedMovePath = new List<GameObject>();
            // InputPathManager.OnMovementPathUpdated += DrawMovementPath;
            var vfxGo = GameObject.FindWithTag(TagManager.VfxTag);
            if(vfxGo!=null)
                parent = vfxGo.transform;
        }

        public void OnEnable()
        {
            Reset();
           
        }

        private GameObject CreateArrowPart()
        {
            return  Object.Instantiate(moveArrowPartPrefab, parent);
        }
        public void DrawMovementPath(List<Vector2Int> mousePath, int startX, int startY)
        {
            // Debug.Log("================================================");
            // Debug.Log("Draw movement path: "+startX+" "+startY);
            // foreach (var v in mousePath)
            // {
            //     Debug.Log(v);
            // }
            // Debug.Log("================================================");
            HideMovementPath();
            if (startX == -1 || startY == -1)
                return;
            movementPathVisible = true;
        
            
            if (mousePath.Count == 0)
            {
                DrawCursor(startX, startY);
                DrawArrowStart(startX, startY, moveArrowSprites.standOnArrowStartNeutral);
            }
            else
            {
                DrawArrowStart(startX, startY, moveArrowSprites.standOnArrowStart);
                RotateArrowStart(mousePath, startX, startY);
                
            }
            for (var i = 0; i < mousePath.Count; i++)
            {
                var v = mousePath[i];
                GameObject dot = GetOrCreateMoveArrowPart(i);
                dot.transform.localPosition = new Vector3(v.x + 0.5f, v.y + 0.5f, -0.03f);
               
                if (i == mousePath.Count - 1)
                {
                    DrawCursor(v.x, v.y);
                    DrawArrowHead(mousePath, startX, startY, i, v, dot);
                }
                else
                {
                    var vAfter = mousePath[i + 1];
                    Vector2Int vBefore;
                    if (i != 0)
                    {
                        vBefore = mousePath[i - 1];
                        DrawCurvedMovementPathSection(dot, v, vBefore, vAfter);
                    }
                    else
                    {
                        vBefore = new Vector2Int(startX, startY);
                        DrawCurvedMovementPathSection(dot, v, vBefore, vAfter);
                    }
                }
            
            }
        }

        private GameObject GetOrCreateMoveArrowPart(int i)
        {
            GameObject dot;
           
            if (i >= instantiatedMovePath.Count)
            {
                dot = CreateArrowPart();
                instantiatedMovePath.Add(dot);
            }
            else
            {
                if (instantiatedMovePath[i]==null)
                {
                    instantiatedMovePath.Clear();
                    return GetOrCreateMoveArrowPart(i);
                }
                dot = instantiatedMovePath[i];
                dot.SetActive(true);
            }

            return dot;
        }

        private void DrawArrowHead(List<Vector2Int> mousePath, int startX, int startY,  int i,
            Vector2Int v, GameObject dot)
        {
            SpriteRenderer dotSpriteRenderer = dot.GetComponent<SpriteRenderer>();
            dotSpriteRenderer.sprite = moveArrowSprites.moveArrowHead;
            if (i != 0)
            {
                if (v.x - mousePath[i - 1].x > 0)
                    dot.transform.rotation = Quaternion.Euler(0, 0, 180);
                else if (v.x - mousePath[i - 1].x < 0)
                    dot.transform.rotation = Quaternion.Euler(0, 0, 0);
                else if (v.y - mousePath[i - 1].y > 0)
                    dot.transform.rotation = Quaternion.Euler(0, 0, 270);
                else if (v.y - mousePath[i - 1].y < 0)
                    dot.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                if (v.x - startX > 0)
                    dot.transform.rotation = Quaternion.Euler(0, 0, 180);
                else if (v.x - startX < 0)
                    dot.transform.rotation = Quaternion.Euler(0, 0, 0);
                else if (v.y - startY > 0)
                    dot.transform.rotation = Quaternion.Euler(0, 0, 270);
                else if (v.y - startY < 0)
                    dot.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
        }

        private void RotateArrowStart(List<Vector2Int> mousePath, int x, int y)
        {
            var v = new Vector2(x, y);
            if (v.x - mousePath[0].x > 0)
                moveCursorStart.transform.rotation = Quaternion.Euler(0, 0, 180);
            else if (v.x - mousePath[0].x < 0)
                moveCursorStart.transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (v.y - mousePath[0].y > 0)
                moveCursorStart.transform.rotation = Quaternion.Euler(0, 0, 270);
            else if (v.y - mousePath[0].y < 0)
                moveCursorStart.transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        private void DrawArrowStart(int x, int y, Sprite sprite)
        {
            if (moveCursorStart == null)
            {
                moveCursorStart = CreateArrowPart();
                moveCursorStart.name = "MoveCursorStart";
                
            }
            if(moveCursorStartSpriteRenderer == null)
                moveCursorStartSpriteRenderer = moveCursorStart.GetComponent<SpriteRenderer>();

            moveCursorStart.SetActive(true);
            moveCursorStart.transform.localPosition = new Vector3(x + 0.5f,
                y + 0.5f, -0.03f);
            moveCursorStartSpriteRenderer.sprite = sprite;
        }

        private void DrawCursor(int startX, int startY)
        {
            if (moveCursor == null)
            {
                moveCursor = Object.Instantiate(moveCursorPrefab, parent);
                moveCursor.name = "MoveCursor";
            }

            moveCursor.transform.localPosition = new Vector3(startX,
                startY, moveCursor.transform.localPosition.z);
            moveCursor.SetActive(true);
        }

        public void HideMovementPath()
        {
            if (!movementPathVisible)
                return;
            movementPathVisible = false;
            foreach (var moveArrowPart in instantiatedMovePath)
            {
                moveArrowPart.SetActive(false);
            }
            //instantiatedMovePath.Clear();
            if (moveCursor != null)
                moveCursor.SetActive(false);//GameObject.Destroy(moveCursor);
            if (moveCursorStart != null)
                moveCursorStart.SetActive(false);
        }
        private void DrawCurvedMovementPathSection(GameObject dot, Vector2Int v, Vector2Int vBefore, Vector2Int vAfter)
        {
            SpriteRenderer sr = dot.GetComponent<SpriteRenderer>();
            if (vBefore.x == vAfter.x)
            {
                sr.sprite = moveArrowSprites.moveArrowStraight;
                dot.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else if (vBefore.y == vAfter.y)
            {
                sr.sprite = moveArrowSprites.moveArrowStraight;
                dot.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                sr.sprite = moveArrowSprites.moveArrowCurve;
                if (vBefore.x - vAfter.x > 0)
                {
                    if (vBefore.y - vAfter.y > 0)
                    {
                        if (vBefore.x == v.x)
                            dot.transform.rotation = Quaternion.Euler(0, 0, 90);
                        else
                            dot.transform.rotation = Quaternion.Euler(0, 0, 270);
                    }
        
                    else
                    {
                        if (vBefore.x == v.x)
                            dot.transform.rotation = Quaternion.Euler(0, 0, 180);
                        else
                            dot.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                }
                else if (vBefore.x - vAfter.x < 0)
                {
                    if (vBefore.y - vAfter.y > 0)
                    {
                        if (vBefore.x == v.x)
                            dot.transform.rotation = Quaternion.Euler(0, 0, 0);
                        else
                            dot.transform.rotation = Quaternion.Euler(0, 0, 180);
                    }
                    else
                    {
                        if (vBefore.x == v.x)
                            dot.transform.rotation = Quaternion.Euler(0, 0, 270);
                        else
                            dot.transform.rotation = Quaternion.Euler(0, 0, 90);
                    }
                }
            }
        }
        
    }
}