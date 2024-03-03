using Game.GameActors.Units;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class ChampionUI : MonoBehaviour
    {

        [SerializeField]  Image characterFace;
        [SerializeField]  Image blessingColor;
        [SerializeField] private GameObject alreadyBlessedGO;
        public void Show(Unit blessedChampion, bool canBeBlessed)
        {
            if(canBeBlessed||blessedChampion!=null)
                gameObject.SetActive(true);
            alreadyBlessedGO.gameObject.SetActive(blessedChampion != null);
            if (blessedChampion != null)
            {
                characterFace.sprite = blessedChampion.FaceSprite;
                blessingColor.color = blessedChampion.Blessing.God.Color;
                
            }
        }
        
    }
}
