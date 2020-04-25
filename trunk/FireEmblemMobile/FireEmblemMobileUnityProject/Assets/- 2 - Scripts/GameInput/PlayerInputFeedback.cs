using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Assets.GameResources;
using Assets.Grid;
using Assets.GameActors.Units.Monsters;
using Assets.Mechanics;
using Assets.GameInput;

public class PlayerInputFeedback 
{
    private Transform gameWorld;
    private ResourceScript resources;
    private GameObject moveCursor;
    private GameObject moveCursorStart;
    private List<GameObject> instantiatedMovePath;
    public PlayerInputFeedback()
    {
        instantiatedMovePath = new List<GameObject>();
        gameWorld = GameObject.FindGameObjectWithTag("World").transform;
        resources = GameObject.FindObjectOfType<ResourceScript>();
        InputSystem.OnDragReset += HideMoventPath;
    }
    public void DrawMovementPath(List<Vector2> mousePath, int startX, int startY)
    {
        HideMoventPath();
        instantiatedMovePath = new List<GameObject>();
        if (moveCursor != null)
            GameObject.Destroy(moveCursor);
        if (moveCursorStart != null)
            GameObject.Destroy(moveCursorStart);
        if (mousePath.Count == 0)
        {
            moveCursor = GameObject.Instantiate(resources.Prefabs.MoveCursor, gameWorld);
            moveCursor.transform.localPosition = new Vector3(startX,
                startY, moveCursor.transform.localPosition.z);
        }
        else
        {
            moveCursorStart = GameObject.Instantiate(resources.Prefabs.MoveArrowDot, gameWorld);
            moveCursorStart.transform.localPosition = new Vector3(startX + 0.5f,
                startY + 0.5f, -0.03f);
            moveCursorStart.GetComponent<SpriteRenderer>().sprite = resources.Sprites.StandOnArrowStart;
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

            var dot = GameObject.Instantiate(resources.Prefabs.MoveArrowDot, gameWorld);
            dot.transform.localPosition = new Vector3(v.x + 0.5f, v.y + 0.5f, -0.03f);
            if (i == mousePath.Count - 1)
            {
                moveCursor = GameObject.Instantiate(resources.Prefabs.MoveCursor, gameWorld);
                moveCursor.transform.localPosition = new Vector3(v.x, v.y, moveCursor.transform.localPosition.z);
                dot.GetComponent<SpriteRenderer>().sprite = resources.Sprites.MoveArrowHead;
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
                    DrawCurvedMovementPathSection(dot, v, vBefore, vAfter);
                }
                else
                {
                    vBefore = new Vector2(startX, startY);
                    DrawCurvedMovementPathSection(dot, v, vBefore, vAfter);
                }
            }
            instantiatedMovePath.Add(dot);
        }
    }

    public void HideMoventPath()
    {
        for(int i=0; i< instantiatedMovePath.Count; i++)
        {
            GameObject.Destroy(instantiatedMovePath[i]);
        }
        instantiatedMovePath.Clear();
        if (moveCursor != null)
            GameObject.Destroy(moveCursor);
        if (moveCursorStart != null)
            GameObject.Destroy(moveCursorStart);
    }
    public void DrawCurvedMovementPathSection(GameObject dot, Vector2 v, Vector2 vBefore, Vector2 vAfter)
    {
        if (vBefore.x == vAfter.x)
        {
            dot.GetComponent<SpriteRenderer>().sprite = resources.Sprites.MoveArrowStraight;
            dot.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (vBefore.y == vAfter.y)
        {
            dot.GetComponent<SpriteRenderer>().sprite = resources.Sprites.MoveArrowStraight;
            dot.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            dot.GetComponent<SpriteRenderer>().sprite = resources.Sprites.MoveArrowCurve;
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