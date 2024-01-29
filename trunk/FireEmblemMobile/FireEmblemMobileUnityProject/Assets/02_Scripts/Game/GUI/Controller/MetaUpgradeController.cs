using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Players;
using LostGrace;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class UpgradePage
{
    [SerializeField] public List<Transform> upgradeButtonsPositions;
}
public class MetaUpgradeController : MonoBehaviour
{
    [SerializeField] private UINavigationBar navigationBar;
    [SerializeField] private MetaUpgradeBP[] upgradeBPs;
    [SerializeField] private List<GameObject> upgradeButtons;
    [SerializeField] private List<UpgradePage> upgradePages;
    [SerializeField] private float pageAnimationTime = .5f;
    private int currentPage = 0;
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
        for (int i = 0; i < upgradeButtons.Count; i++)
        {
            LeanTween.cancel(upgradeButtons[i]);
            LeanTween.move(upgradeButtons[i], upgradePages[currentPage].upgradeButtonsPositions[i].position,
                pageAnimationTime).setEaseInOutQuad();
        }
    }

    public void NextPage()
    {
        currentPage++;
        if (currentPage >= upgradePages.Count)
            currentPage = 0;
        navigationBar.Next();
        UpdateUI();
    }
    public void PreviousPage()
    {
        currentPage--;
        if (currentPage < 0)
            currentPage = upgradePages.Count-1;
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