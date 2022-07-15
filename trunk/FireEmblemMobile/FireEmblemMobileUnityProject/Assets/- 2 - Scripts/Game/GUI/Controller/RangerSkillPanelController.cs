using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RangerSkillPanelController : MonoBehaviour
{
    private MetaUpgrade[,] upgrades;
    private int xSize = 5;
    private int ySize = 3;
    void OnEnable()
    {
        upgrades = new MetaUpgrade[xSize,ySize];
        foreach (var upg in GetComponentsInChildren<MetaButtonController>())
        {
            upgrades[upg.metaSkill.xPosInTree, upg.metaSkill.yPosInTree] = upg.metaSkill;
        }

        CheckDependencies();
    }

    void CheckDependencies()
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if(upgrades[x,y]==null)
                    continue;
                if(upgrades[x, y].state == UpgradeState.Maxed||upgrades[x, y].state == UpgradeState.Learned)
                    continue;
                
                if (CheckNeighborsLearned(upgrades[x, y]))
                {
                    upgrades[x, y].state = UpgradeState.NotLearned;
                }
                else
                {
                    upgrades[x, y].state = UpgradeState.Locked;
                }
            }
        }
    }

    bool CheckNeighborsLearned(MetaUpgrade upgrade)
    {
        int xPos = upgrade.xPosInTree;
        int yPos = upgrade.yPosInTree;
        xPos--;
        if (xPos >= 0)
        {
            var leftNeighbor = upgrades[xPos, yPos];
            if (leftNeighbor != null&&(leftNeighbor.state == UpgradeState.Learned || leftNeighbor.state == UpgradeState.Maxed))
            {
                return true;
            }
        }
        xPos++;
        xPos++;
        if (xPos < xSize)
        {
            var rightNeighbor = upgrades[xPos, yPos];
            if (rightNeighbor != null&&(rightNeighbor.state == UpgradeState.Learned || rightNeighbor.state == UpgradeState.Maxed))
            {
                return true;
            }
        }

        xPos--;
//Reset
        yPos--;
        if (yPos >= 0)
        {
            var topNeighbor = upgrades[xPos, yPos];
            if (topNeighbor != null&&(topNeighbor.state == UpgradeState.Learned || topNeighbor.state == UpgradeState.Maxed))
            {
                return true;
            }
        }
        yPos++;
        yPos++;
        if (yPos < ySize)
        {
            var bottomNeighbor = upgrades[xPos, yPos];
            if (bottomNeighbor != null&&(bottomNeighbor.state == UpgradeState.Learned || bottomNeighbor.state == UpgradeState.Maxed))
            {
                return true;
            }
        }

        return false;

    }
    
    
}