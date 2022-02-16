using UnityEngine;

public class UIConvoyController:MonoBehaviour{
    public void Show()
    {
        GetComponent<Canvas>().enabled = true;
    }
    public void Hide()
    {
        GetComponent<Canvas>().enabled = false;
    }
}