using System.Collections;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Manager;
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

   

    public EnemyArmyData enemyArmyData;
    

    public GameObject enemySprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Activate()
    {
        GameObject.Destroy(enemySprite);
        //GameSceneController.Instance.LoadBattleLevel(sceneIndex, enemyArmyData);
    }
}
