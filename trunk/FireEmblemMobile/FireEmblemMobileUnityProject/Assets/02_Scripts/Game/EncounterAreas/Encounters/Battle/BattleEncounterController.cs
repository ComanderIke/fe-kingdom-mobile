using UnityEngine;

namespace Game.EncounterAreas.Encounters.Battle
{
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
}