using UnityEngine;

public class Revealer : MonoBehaviour
{
    public int radius;

    private void Start()
    {
        FogOfWarManager.Instance.RegisterRevealer(this);
    }
}