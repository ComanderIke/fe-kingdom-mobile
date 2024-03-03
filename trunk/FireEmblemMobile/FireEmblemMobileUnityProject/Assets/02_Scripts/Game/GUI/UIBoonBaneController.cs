using System;
using Game.GameActors.Player;
using Game.GameActors.Units.Numbers;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class UIBoonBaneController : MonoBehaviour
    {
        [SerializeField] private Image boonImage;
        [SerializeField] private Image baneImage;

        [SerializeField] private GameObject STRObject;
        [SerializeField] private GameObject DEXObject;
        [SerializeField] private GameObject INTObject;
        [SerializeField] private GameObject AGIObject;
        [SerializeField] private GameObject CONObject;
        [SerializeField] private GameObject LCKObject;
        [SerializeField] private GameObject DEFObject;
        [SerializeField] private GameObject RESObject;
        private AttributeType currentBoon;
        private AttributeType currentBane;
        public event Action<AttributeType> OnBoonSelected;
        public event Action<AttributeType> OnBaneSelected;

        void Start()
        {
            currentBoon = AttributeType.NONE;
            currentBane = AttributeType.NONE;
        }

        void Update()
        {
        
        }

        void ApplyRandomBaneExcept()
        {
            int rngIndex = (int)currentBoon;
            while (rngIndex == (int)currentBoon)
            {
                rngIndex = UnityEngine.Random.Range(0, Enum.GetNames(typeof(AttributeType)).Length-2);
            }

            currentBane =(AttributeType) rngIndex;
            OnBaneSelected?.Invoke(currentBane);
        }
        void ApplyRandomBoonExcept()
        {
            int rngIndex = (int)currentBane;
            while (rngIndex == (int)currentBane)
            {
                rngIndex = UnityEngine.Random.Range(0, Enum.GetNames(typeof(AttributeType)).Length-2);
            }

            currentBoon =(AttributeType) rngIndex;
            OnBoonSelected?.Invoke(currentBoon);
        }

        public void SoftResetBoonBane()
        {
            currentBoon = AttributeType.NONE;
            currentBane = AttributeType.NONE;
        }
        private void ResetBoonBane()
        {
            currentBoon = AttributeType.NONE;
            currentBane = AttributeType.NONE;
            OnBaneSelected?.Invoke(currentBane);
            OnBoonSelected?.Invoke(currentBoon);
        }

        [SerializeField] private Vector3 baneOffset;
        [SerializeField] private Vector3 boonOffset;

        public void UpdateUI()
        {
            baneImage.gameObject.SetActive(true);
            boonImage.gameObject.SetActive(true);
            switch (currentBane)
            {
                case AttributeType.STR:
                    baneImage.transform.position = STRObject.transform.position-baneOffset; break;
                case AttributeType.DEX:
                    baneImage.transform.position = DEXObject.transform.position- baneOffset; break;
                case AttributeType.INT:
                    baneImage.transform.position = INTObject.transform.position- baneOffset; break;
                case AttributeType.AGI:
                    baneImage.transform.position = AGIObject.transform.position +baneOffset; break;
                case AttributeType.CON:
                    baneImage.transform.position = CONObject.transform.position- baneOffset; break;
                case AttributeType.LCK:
                    baneImage.transform.position = LCKObject.transform.position+ baneOffset; break;
                case AttributeType.DEF:
                    baneImage.transform.position = DEFObject.transform.position+ baneOffset; break;
                case AttributeType.FTH:
                    baneImage.transform.position = RESObject.transform.position+ baneOffset; break;
                case AttributeType.NONE:
                    baneImage.gameObject.SetActive(false);
                    break;
            }
            switch (currentBoon)
            {
                case AttributeType.STR:
                    boonImage.transform.position = STRObject.transform.position-baneOffset; break;
                case AttributeType.DEX:
                    boonImage.transform.position = DEXObject.transform.position- baneOffset; break;
                case AttributeType.INT:
                    boonImage.transform.position = INTObject.transform.position- baneOffset; break;
                case AttributeType.AGI:
                    boonImage.transform.position = AGIObject.transform.position+ baneOffset; break;
                case AttributeType.CON:
                    boonImage.transform.position = CONObject.transform.position- baneOffset; break;
                case AttributeType.LCK:
                    boonImage.transform.position = LCKObject.transform.position+ baneOffset; break;
                case AttributeType.DEF:
                    boonImage.transform.position = DEFObject.transform.position+ baneOffset; break;
                case AttributeType.FTH:
                    boonImage.transform.position = RESObject.transform.position+ baneOffset; break;
                case AttributeType.NONE:
                    boonImage.gameObject.SetActive(false);
                    break;
            }
        }

        public void AttributeClicked(AttributeType type)
        {
            if (!Player.Instance.Flags.BoonBaneUnlocked)
                return;
            if (currentBoon == type)
            {
                
                currentBane = type;
                OnBaneSelected?.Invoke(currentBane);
               
                ApplyRandomBoonExcept();
            }
            else if (currentBane == type)
            {
                ResetBoonBane();
            }
            else
            {
                currentBoon = type;
                OnBoonSelected?.Invoke(currentBoon);
                if(currentBane == AttributeType.NONE)
                    ApplyRandomBaneExcept();
            }

            UpdateUI();
        }

        public void STR_Clicked()
        {
            AttributeClicked(AttributeType.STR);
        }
        public void DEX_Clicked()
        {
            AttributeClicked(AttributeType.DEX);
        }
        public void INT_Clicked()
        {
            AttributeClicked(AttributeType.INT);
        }
        public void AGI_Clicked()
        {
            AttributeClicked(AttributeType.AGI);
        }
        public void CON_Clicked()
        {
            AttributeClicked(AttributeType.CON);
        }
        public void LCK_Clicked()
        {
            AttributeClicked(AttributeType.LCK);
        }
        public void DEF_Clicked()
        {
            AttributeClicked(AttributeType.DEF);
        }
        public void FTH_Clicked()
        {
            AttributeClicked(AttributeType.FTH);
        }


        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}
