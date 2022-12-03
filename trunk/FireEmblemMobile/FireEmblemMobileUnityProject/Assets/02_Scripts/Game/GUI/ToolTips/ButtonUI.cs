using System;
using UnityEngine;

public class ButtonUI : MonoBehaviour
{
    public event Action onClicked;

    public void Clicked()
    {
        onClicked?.Invoke();
    }
}