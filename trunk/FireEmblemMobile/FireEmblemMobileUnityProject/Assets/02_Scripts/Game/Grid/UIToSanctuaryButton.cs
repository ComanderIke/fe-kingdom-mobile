using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Controller;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LostGrace
{
    public class UIToSanctuaryButton : MonoBehaviour
    {
        [SerializeField] private OKCancelDialogController okCancelDialogController;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Clicked()
        {
            okCancelDialogController.Show("Return to the Sanctuary?",()=>GameSceneController.Instance.LoadSanctuaryFromCampaign() );
            
        }
    }
}
