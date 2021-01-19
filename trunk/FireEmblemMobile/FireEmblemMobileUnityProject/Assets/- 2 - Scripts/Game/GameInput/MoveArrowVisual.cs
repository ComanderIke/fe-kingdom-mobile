using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GameInput
{
    [Serializable]
    public class MoveArrowVisual
    {
        private GameObject moveCursor;
        private GameObject moveCursorStart;
        private MoveArrowVisual moveArrowVisual;
        private List<GameObject> instantiatedMovePath;
        private bool movementPathVisible = false;
        [SerializeField]
        private GameObject moveCursorPrefab;
        [SerializeField]
        private MoveArrowSprites moveArrowSprites;
        [SerializeField]
        private Transform parent;
        public MoveArrowVisual()
        {
            instantiatedMovePath = new List<GameObject>();

        }
        public void DrawMovementPath(List<Vector2> mousePath, int startX, int startY)
        {
            HideMovementPath();
            movementPathVisible = true;
        
            if (mousePath.Count == 0)
            {
                if (moveCursor == null)
                {
                    moveCursor = GameObject.Instantiate(moveCursorPrefab, parent);
                    moveCursor.name = "MoveCursor";
                }
                moveCursor.transform.localPosition = new Vector3(startX,
                    startY, moveCursor.transform.localPosition.z);
                moveCursor.SetActive(true);
                if (moveCursorStart == null)
                {
                    
                    moveCursorStart = GameObject.Instantiate(new GameObject(), parent);
                    moveCursorStart.AddComponent<SpriteRenderer>();
                    moveCursorStart.name = "MoveCursorStart";
                }
                moveCursorStart.SetActive(true);
                moveCursorStart.transform.localPosition = new Vector3(startX + 0.5f,
                    startY + 0.5f, -0.03f);
                moveCursorStart.GetComponent<SpriteRenderer>().sprite = moveArrowSprites.standOnArrowStartNeutral;
            }
            else
            {
                if (moveCursorStart == null)
                {
                    moveCursorStart = GameObject.Instantiate(new GameObject(), parent);
                    moveCursorStart.AddComponent<SpriteRenderer>();
                    moveCursorStart.name = "MoveCursorStart";
                }
          
                moveCursorStart.SetActive(true);
                moveCursorStart.transform.localPosition = new Vector3(startX + 0.5f,
                    startY + 0.5f, -0.03f);
                moveCursorStart.GetComponent<SpriteRenderer>().sprite = moveArrowSprites.standOnArrowStart;
                var v = new Vector2(startX, startY);
                if (v.x - mousePath[0].x > 0)
                    moveCursorStart.transform.rotation = Quaternion.Euler(0, 0, 180);
                else if (v.x - mousePath[0].x < 0)
                    moveCursorStart.transform.rotation = Quaternion.Euler(0, 0, 0);
                else if (v.y - mousePath[0].y > 0)
                    moveCursorStart.transform.rotation = Quaternion.Euler(0, 0, 270);
                else if (v.y - mousePath[0].y < 0)
                    moveCursorStart.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            for (var i = 0; i < mousePath.Count; i++)
            {
                var v = mousePath[i];
                GameObject dot = null;
                if (i >= instantiatedMovePath.Count)
                {
                    dot = GameObject.Instantiate(new GameObject(), parent);
                    dot.AddComponent<SpriteRenderer>();
                    instantiatedMovePath.Add(dot);
                }
                else
                {
                    dot = instantiatedMovePath[i];
                    dot.SetActive(true);
                }
                dot.transform.localPosition = new Vector3(v.x + 0.5f, v.y + 0.5f, -0.03f);
                SpriteRenderer dotSpriteRenderer = dot.GetComponent<SpriteRenderer>();
                if (i == mousePath.Count - 1)
                {
                    if (moveCursor == null)
                    {
                        moveCursor = GameObject.Instantiate(moveCursorPrefab, parent);
                        moveCursor.name = "MoveCursor";
                    }
                    moveCursor.SetActive(true);
                    moveCursor.transform.localPosition = new Vector3(v.x, v.y, moveCursor.transform.localPosition.z);
                
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
                else
                {
                    var vAfter = mousePath[i + 1];
                    Vector2 vBefore;
                    if (i != 0)
                    {
                        vBefore = mousePath[i - 1];
                        DrawCurvedMovementPathSection(dot, dotSpriteRenderer, v, vBefore, vAfter);
                    }
                    else
                    {
                        vBefore = new Vector2(startX, startY);
                        DrawCurvedMovementPathSection(dot, dotSpriteRenderer, v, vBefore, vAfter);
                    }
                }
            
            }
        }
        public void HideMovementPath()
        {
            if (!movementPathVisible)
                return;
            movementPathVisible = false;
            for(int i=0; i< instantiatedMovePath.Count; i++)
            {
                instantiatedMovePath[i].SetActive(false);
            }
            //instantiatedMovePath.Clear();
            if (moveCursor != null)
                moveCursor.SetActive(false);//GameObject.Destroy(moveCursor);
            if (moveCursorStart != null)
                moveCursorStart.SetActive(false);
        }
        public void DrawCurvedMovementPathSection(GameObject dot, SpriteRenderer sr, Vector2 v, Vector2 vBefore, Vector2 vAfter)
        {
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