using System;
using System.Collections.Generic;
using Game.GameResources;
using Game.GUI;
using Game.WorldMapStuff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Menu
{
    public class CampaingSelectionController : UIMenu
    {
        // Start is called before the first frame update
        private List<CampaignConfig> campaignConfigs;
        private List<RectTransform> campaignImages;
        [SerializeField]private GameObject campaignImagePrefab;
        private int selected = 0;
        [SerializeField]private RectTransform selectedPosition;
        [SerializeField]private RectTransform leftPosition;
        [SerializeField]private RectTransform rightPosition;
        [SerializeField] private Transform campaignParent;
        private int xOffset=50;
        public GameObject nextCampaignButton;
        public GameObject prevCampaignButton;
        private Vector2 sizeOffset = new Vector2(25, 25);
        public event Action<int> LoadCampaignClicked;
        void Start()
        {
            campaignConfigs=GameData.Instance.campaigns;
            campaignImages = new List<RectTransform>();
           
           
            for (int i = 0; i < campaignConfigs.Count; i++)
            {
                var go = Instantiate(campaignImagePrefab, campaignParent);
                var rectT = go.GetComponent<RectTransform>();
                go.GetComponent<Image>().sprite = campaignConfigs[i].sprite;
                campaignImages.Add(rectT);
                if (i == selected)
                {
                    rectT.anchoredPosition = selectedPosition.anchoredPosition;
                    rectT.sizeDelta = selectedPosition.sizeDelta;
                }

                if (i < selected)
                {
                    rectT.anchoredPosition = leftPosition.anchoredPosition-new Vector2((selected-i)*xOffset,0);
                    rectT.sizeDelta = leftPosition.sizeDelta-((selected-i)*sizeOffset);
                    go.transform.SetSiblingIndex(selected-(selected-i));
                }
                else if (i > selected)
                {
                    rectT.anchoredPosition = rightPosition.anchoredPosition+new Vector2((i-selected)*xOffset,0);
                    rectT.sizeDelta = rightPosition.sizeDelta-((i-selected)*sizeOffset);
                    go.transform.SetSiblingIndex(go.transform.parent.childCount-1-(i-selected));
                }
            }
            nextCampaignButton.SetActive(selected != campaignImages.Count - 1);
            prevCampaignButton.SetActive(selected != 0);
        }

        public void StartClicked()
        {
            LoadCampaignClicked?.Invoke(selected);
        }
        public void UpdatePosition()
        {
            for (int i = 0; i < campaignImages.Count; i++)
            {
                if (i == selected)
                {
                    LeanTween.move(campaignImages[i], selectedPosition.anchoredPosition, .4f).setEaseOutQuad();
                    LeanTween.size(campaignImages[i],selectedPosition.sizeDelta,.4f).setEaseOutQuad();
                

                }

                if (i < selected)
                {
                    LeanTween.move(campaignImages[i], leftPosition.anchoredPosition-new Vector2((selected-i)*xOffset,0), .4f).setEaseOutQuad();
                    LeanTween.size(campaignImages[i],leftPosition.sizeDelta-((selected-i)*sizeOffset),.4f).setEaseOutQuad();
                    campaignImages[i].transform.SetSiblingIndex(selected-(selected-i));
                }
                else if (i > selected)
                {
                    LeanTween.move(campaignImages[i], rightPosition.anchoredPosition+new Vector2((i-selected)*xOffset,0), .4f).setEaseOutQuad();
                    LeanTween.size(campaignImages[i],rightPosition.sizeDelta-((i-selected)*sizeOffset),.4f).setEaseOutQuad();
                    campaignImages[i].transform.SetSiblingIndex(campaignImages[i].transform.parent.childCount-1-(i-selected));
                }
            }
        }

        // Update is called once per frame
        public void NextCampaignClicked()
        {
            selected++;
            if (selected >= campaignImages.Count)
                selected = campaignImages.Count - 1;
            nextCampaignButton.SetActive(selected != campaignImages.Count - 1);
            prevCampaignButton.SetActive(selected != 0);
            UpdatePosition();
        }
        public void PreviousCampaignClicked()
        {
            selected--;
            if (selected < 0)
                selected = 0;
            nextCampaignButton.SetActive(selected != campaignImages.Count - 1);
            prevCampaignButton.SetActive(selected != 0);
            UpdatePosition();
        }
        void Update()
        {
        
        }
    }
}
