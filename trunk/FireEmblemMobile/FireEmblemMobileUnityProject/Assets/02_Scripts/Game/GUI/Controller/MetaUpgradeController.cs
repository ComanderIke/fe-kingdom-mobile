using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Players;
using LostGrace;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[System.Serializable]
public class UpgradePage
{
    [SerializeField] public List<Transform> upgradeButtonsPositions;
    [SerializeField] public Image backgroundArt;
    [SerializeField] public God god;
}
public class MetaUpgradeController : MonoBehaviour
{
    [SerializeField] private UINavigationBar navigationBar;
    [SerializeField] private MetaUpgradeBP[] upgradeBPs;
    [SerializeField] private List<GameObject> upgradeButtons;
    [SerializeField] private List<UpgradePage> upgradePages;
    [SerializeField] private float pageAnimationTime = .5f;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image statueImage;
    [SerializeField] private Image backgroundImage1;
    [SerializeField] private Image backgroundImage2;
    [SerializeField] private Image detailFrameImage1;
    [SerializeField] private Image detailFrameImage2;
    [SerializeField] private Image titleImage;
    private int currentPage = 0;
    private int previousPage = -1;
    private static readonly int TintColor = Shader.PropertyToID("_TintColor");

    void Start()
    {
        if (Player.Instance != null)
        {
            Player.Instance.onMetaUpgradesChanged -= CheckDependencies;
            Player.Instance.onMetaUpgradesChanged += CheckDependencies;
        }

        navigationBar.Init(upgradePages.Count);
        Show();
        
       
    }

    public void Show()
    {
        
        
        CheckDependencies();
        
    }

    private void OnDestroy()
    {
        if(Player.Instance!=null)
            Player.Instance.onMetaUpgradesChanged -= CheckDependencies;
    }

    void UpdateUI()
    {
       
        titleText.text = upgradePages[currentPage].god.ChronikComponent.Name;
        LeanTween.cancel(upgradePages[currentPage].backgroundArt.gameObject);
        if (previousPage != -1)
        {
            LeanTween.cancel(upgradePages[previousPage].backgroundArt.gameObject);
            LeanTween.alpha(upgradePages[previousPage].backgroundArt.rectTransform, 0, pageAnimationTime).setEaseInOutQuad();
        }
        LeanTween.cancel(statueImage.gameObject);
        LeanTween.alpha(upgradePages[currentPage].backgroundArt.rectTransform, .35f, pageAnimationTime).setEaseInOutQuad();
        LeanTween.alpha(statueImage.rectTransform, 0f, pageAnimationTime/2f).setEaseInOutQuad().setOnComplete(() =>
        {
            statueImage.sprite = upgradePages[currentPage].god.statueSprite;
            LeanTween.alpha(statueImage.rectTransform, 1f, pageAnimationTime / 2f).setEaseInOutQuad();
        });
        LeanTween.cancel(gameObject);
        LeanTween.value(gameObject, 0, 1, pageAnimationTime).setEaseInOutQuad().setOnUpdate((val) =>
        {
            Color color = Color.Lerp( backgroundImage1.material.GetColor(TintColor),upgradePages[currentPage].god.upgradeBGColor, val);
            Color color2 = Color.Lerp( titleImage.material.GetColor(TintColor),upgradePages[currentPage].god.TooltipFrameColor, val);
            backgroundImage1.material.SetColor(TintColor, color);
            backgroundImage2.material.SetColor(TintColor, color);
            detailFrameImage1.material.SetColor(TintColor, color2);
            detailFrameImage2.material.SetColor(TintColor, color2);
            titleImage.material.SetColor(TintColor, color2);
        });
        
       
        for (int i = 0; i < upgradeButtons.Count; i++)
        {
            LeanTween.cancel(upgradeButtons[i]);
            LeanTween.move(upgradeButtons[i], upgradePages[currentPage].upgradeButtonsPositions[i].position,
                pageAnimationTime).setEaseInOutQuad();
            
        }
    }

    public void NextPage()
    {
        previousPage = currentPage;
        currentPage++;
        if (currentPage >= upgradePages.Count)
            currentPage = upgradePages.Count-1;
        
        navigationBar.Next();
        UpdateUI();
    }
    public void PreviousPage()
    {
        previousPage = currentPage;
        currentPage--;
        if (currentPage < 0)
            currentPage =0;
        navigationBar.Previous();
        UpdateUI();
    }
    void CheckDependencies()
    {
        // foreach (var upg in upgradeGrid)
        // {
        //     
        //         if(upg==null)
        //             continue;
        //         if(Player.Instance!=null&&Player.Instance.HasLearned(upg))
        //             continue;
        //         
        //         if (CheckNeighborsLearned(upg))
        //         {
        //             upg.locked = false;
        //         }
        //         else
        //         {
        //             // if (upg.blueprint.availableAtStart)
        //             //     upg.locked = false;
        //             // else
        //                 upg.locked = false;
        //         }
        // }
        UpdateUI();
    }

   
    
    
}