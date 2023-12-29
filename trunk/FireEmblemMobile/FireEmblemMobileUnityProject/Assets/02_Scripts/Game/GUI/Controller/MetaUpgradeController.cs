using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Players;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class MetaUpgradeController : MonoBehaviour
{
    [SerializeField] private MetaUpgradeBP[] upgradeBPs;
    [SerializeField]private int xSize = 5;
    [SerializeField]private int ySize = 3;
    [SerializeField] private GameObject metaUpgradePrefab;
    private MetaUpgrade[,] upgradeGrid;
    private List<MetaButtonController> instantiatedButtonControllers;
    public float XPosMult = 180;
    public float YPosMult=180;
    public float XOffset=120;
    public float YOffset=170;
    void OnEnable()
    {
        #if UNITY_EDITOR
            transform.DeleteAllChildrenImmediate();
        #else
            transform.DeleteAllChildren();
        #endif
        if (Player.Instance != null)
        {
            Player.Instance.onMetaUpgradesChanged -= CheckDependencies;
            Player.Instance.onMetaUpgradesChanged += CheckDependencies;
        }

        Show();
        
       
    }

    public void Show()
    {
        upgradeGrid = new MetaUpgrade[9,9];
        instantiatedButtonControllers = new List<MetaButtonController>();
        // upgrades = new MetaUpgrade[xSize,ySize];
        foreach (var upg in upgradeBPs)
        {
            var go=Instantiate(metaUpgradePrefab, transform);
            var metaUpgrade=new MetaUpgrade(upg);
            go.GetComponent<MetaButtonController>().SetUpgrade(metaUpgrade);
            upgradeGrid[upg.xPosInTree, upg.yPosInTree] = metaUpgrade;
            instantiatedButtonControllers.Add( go.GetComponent<MetaButtonController>());
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(upg.xPosInTree*XPosMult+XOffset, upg.yPosInTree*YPosMult+YOffset);
            //upgrades[upg.metaSkill.xPosInTree, upg.metaSkill.yPosInTree] = upg.metaSkill;
        }

        CheckDependencies();
    }

    private void OnDisable()
    {
        if(Player.Instance!=null)
            Player.Instance.onMetaUpgradesChanged -= CheckDependencies;
    }

    void UpdateUI()
    {
        foreach (var controller in instantiatedButtonControllers)
        {
            if(Player.Instance!=null&&Player.Instance.HasLearned(controller.metaSkill))
                controller.SetUpgrade(Player.Instance.GetMetaUpgrade(controller.metaSkill));
            else
            {
                controller.UpdateUI();
            }
        }
    }
    void CheckDependencies()
    {
        foreach (var upg in upgradeGrid)
        {
            
                if(upg==null)
                    continue;
                if(Player.Instance!=null&&Player.Instance.HasLearned(upg))
                    continue;
                
                if (CheckNeighborsLearned(upg))
                {
                    upg.locked = false;
                }
                else
                {
                    // if (upg.blueprint.availableAtStart)
                    //     upg.locked = false;
                    // else
                        upg.locked = false;
                }
        }
        UpdateUI();
    }

    bool CheckNeighborsLearned(MetaUpgrade upgradeBp)
    {
        int xPos = upgradeBp.blueprint.xPosInTree;
        int yPos = upgradeBp.blueprint.yPosInTree;
        xPos--;
        if (xPos >= 0)
        {
            var leftNeighbor = upgradeGrid[xPos, yPos];
            if (leftNeighbor != null&&(Player.Instance!=null&&Player.Instance.HasLearned(leftNeighbor)))
            {
                return true;
            }
        }
        xPos++;
        xPos++;
        if (xPos < xSize)
        {
            var rightNeighbor = upgradeGrid[xPos, yPos];
            if (rightNeighbor != null&&(Player.Instance!=null&&Player.Instance.HasLearned(rightNeighbor)))
            {
                return true;
            }
        }

        xPos--;
//Reset
        yPos--;
        if (yPos >= 0)
        {
            var topNeighbor = upgradeGrid[xPos, yPos];
            if (topNeighbor != null&&Player.Instance!=null&&Player.Instance.HasLearned(topNeighbor))
            {
                return true;
            }
        }
        yPos++;
        yPos++;
        if (yPos < ySize)
        {
            var bottomNeighbor = upgradeGrid[xPos, yPos];
            if (bottomNeighbor != null&&Player.Instance!=null&&Player.Instance.HasLearned(bottomNeighbor))
            {
                return true;
            }
        }

        return false;

    }
    
    
}