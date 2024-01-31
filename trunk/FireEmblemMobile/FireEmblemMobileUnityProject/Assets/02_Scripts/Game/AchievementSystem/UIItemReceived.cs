using System.Collections;
using Game.GameActors.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the display of achievements on the screen
/// </summary>
///
///
public class UIItemReceived : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private Image icon;
    [SerializeField] private ItemStackUI itemStack;
    public void Set(Item item, ItemStackUI itemStackUI)
    {
        this.itemStack = itemStackUI;
        name.text = item.Name;
        icon.sprite = item.Sprite;
    }

    public void StartDeathTimer ()
    {
        StartCoroutine(Wait());
    }
    private IEnumerator Wait ()
    {
        yield return new WaitForSeconds(AchievementManager.instance.DisplayTime);
        GetComponent<Animator>().SetTrigger("ScaleDown");
        yield return new WaitForSeconds(0.4f);
        itemStack.CheckBackLog();
        Destroy(gameObject);
    }
}