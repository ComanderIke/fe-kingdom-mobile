using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LostGrace
{
    public class LoadBattleButtonController : MonoBehaviour
    {
        [SerializeField] private EnemyArmyData armyData;
        public void Click()
        {
            //GameSceneController.Instance.LoadBattleLevel(Scenes.Battle1,armyData);
            SceneManager.LoadScene((int)Scenes.Battle1);
        }
    }
}
