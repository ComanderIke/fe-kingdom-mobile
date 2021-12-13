using System.Collections;
using System.Collections.Generic;
using Game.GUI;
using UnityEngine;

public class UIDragableManager : MonoBehaviour
{

    public Canvas canvas;

    private UIDragable[] dragables;
    // Start is called before the first frame update
    void Start()
    {
        UpdateChildren();
    }

    private void UpdateChildren()
    {
        dragables = GetComponentsInChildren<UIDragable>();
        foreach (var dragable in dragables)
        {
            dragable.SetCanvas(canvas);
        }
    }
}
