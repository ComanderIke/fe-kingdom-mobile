using System.Collections;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EncounterController : MonoBehaviour
{
    public virtual void Activate()
    {
        
    }
}
public class BattleEncounterController : EncounterController
{

    public GameObject enemySprite;

    public override void Activate()
    {
        GameObject.Destroy(enemySprite);
        //GameSceneController.Instance.LoadBattleLevel(sceneIndex, enemyArmyData);
    }

    public void HideSprite()
    {
        enemySprite.gameObject.SetActive(false);
    }
}
