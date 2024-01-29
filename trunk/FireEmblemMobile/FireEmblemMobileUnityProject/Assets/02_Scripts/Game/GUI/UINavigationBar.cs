using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class UINavigationBar : MonoBehaviour
    {
        [SerializeField] private GameObject pageCountPrefab;
        [SerializeField] int MaxIndicators = 9;
        [SerializeField] int indicatorCount = 15;
        [SerializeField] private float scaleGone = 0;
        [SerializeField] private float scaleSmallest = 0.33f;
        [SerializeField] private float scaleSmall = 0.66f;
        [SerializeField] private float scaleNormal = 1f;
        [SerializeField] private float scaleSelected = 1.38f;
        private List<UIPageCountPointController> pageIndicators;
        private List<float> pageIndicatorScaleValues;
        int lastSelected = -1;
        private Vector3 startPosition;
        private Vector3 resetPosition;

        private bool isOverflowState = false;

        public Color selectedColor;

        public Color normalColor;
        // Start is called before the first frame update
        void Start()
        {
            resetPosition = transform.localPosition;
            // CreateIndicators(indicatorCount);
            // PositionTransform();
            // startPosition = transform.localPosition;
            // OnPageUpdated(0);
        }

        void ResetPosition()
        {
            transform.localPosition = resetPosition;
        }
        public void Init(int articlesCount)
        {
            gameObject.SetActive(true);
            Debug.Log("Init Pager: "+articlesCount);
            pageIndicators = new List<UIPageCountPointController>();
            indicatorCount = articlesCount;
            lastSelected = -1;
            currentPageIndex = 0;
            ResetPosition();
            CreateIndicators();
            PositionTransform();
            startPosition = transform.localPosition; 
            MonoUtility.InvokeNextFrame(()=>OnPageUpdated(currentPageIndex));
        }
        // Update is called once per frame
        void Update()
        {

        }

        void CreateIndicators()
        {
            transform.DeleteAllChildren();
            isOverflowState = indicatorCount > MaxIndicators;
            if (indicatorCount <= 1) return;
            for (int i = 0; i < indicatorCount; i++)
            {
                var go = Instantiate(pageCountPrefab, transform);
                var UIcomponent = go.GetComponent<UIPageCountPointController>();
                UIcomponent.SetColor(normalColor);
                AnimateScale(UIcomponent, isOverflowState ? scaleSmallest : scaleNormal);
                pageIndicators.Add(UIcomponent);
            }
        }

        void UpdateIndicators()
        {
            for (int i = 0; i < indicatorCount; i++)
            {
                if (i == currentPageIndex)
                    AnimateColor(pageIndicators[currentPageIndex], selectedColor);
                else
                {
                    AnimateColor(pageIndicators[i], normalColor);
                }

                AnimateScale(pageIndicators[i], pageIndicatorScaleValues[i]);
            }
        }

        void OnPageUpdated(int currentPageIndex)
        {
            if (isOverflowState)
            {
                UpdateOverflowState(currentPageIndex);
            }
            else
            {
                UpdateSimpleState(currentPageIndex);
            }
        }

        void PositionTransform()
        {
            int indicatorVisible = Math.Min(MaxIndicators, indicatorCount);
            float extraOffset = indicatorVisible % 2 == 0 ? 0 : (pageCountWidth / 2f);
            transform.localPosition =
                new Vector3(-((pageCountWidth + layoutGroup.spacing) * ((int)(indicatorVisible / 2))) - extraOffset,
                    transform.localPosition.y, transform.localPosition.z);
            startPosition = transform.localPosition;
        }

        
        void UpdateOverflowState(int currentPageIndex)
        {
            if (indicatorCount == 0)
                return;
            if (currentPageIndex < 0 || currentPageIndex > indicatorCount)
                return;

            pageIndicatorScaleValues = new List<float>();
            for (int i = 0; i < indicatorCount + 1; i++)
            {
                pageIndicatorScaleValues.Add(scaleGone);
            }

            var start = currentPageIndex - MaxIndicators + 4;
            var realStart = Math.Max(0, start);

            if (realStart + MaxIndicators > indicatorCount) // if we are at the end
            {
                realStart = indicatorCount - MaxIndicators;
                pageIndicatorScaleValues[indicatorCount - 1] = scaleNormal;
                pageIndicatorScaleValues[indicatorCount - 2] = scaleNormal;
            }
            else // not at the end
            {
                if (realStart + MaxIndicators - 2 < indicatorCount) //show overflow of second last
                {
                    pageIndicatorScaleValues[realStart + MaxIndicators - 2] = scaleSmall;
                }

                if (realStart + MaxIndicators - 1 < indicatorCount) //show overflow of last
                {
                    pageIndicatorScaleValues[realStart + MaxIndicators - 1] = scaleSmallest;
                }
            }

            for (int i = realStart; i < realStart + MaxIndicators - 2; i++)
            {
                pageIndicatorScaleValues[i] = scaleNormal;
            }

            if (this.currentPageIndex > 5) //show BeginningOverflow
            {
                pageIndicatorScaleValues[realStart] = scaleSmallest;
                pageIndicatorScaleValues[realStart + 1] = scaleSmall;
            }
            else if (currentPageIndex == 5)
            {
                pageIndicatorScaleValues[realStart] = scaleSmall;
            }
            if (this.currentPageIndex >= 5 && this.currentPageIndex <= indicatorCount - 4) //OverflowWithMovement
            {

                bool leftMovement = currentPageIndex > lastSelected;
                if (!leftMovement && this.currentPageIndex == indicatorCount - 4)
                {
                    //ignore
                }
                else
                {
                    int entries = Math.Max(0, this.currentPageIndex - 5);
                    // if (!leftMovement)
                    //     entries = Math.Max(0, entries - 1);
                    float width = pageCountWidth + layoutGroup.spacing;
                    // Debug.Log("Left: " + leftMovement + " " + entries + " " + currentPageIndex);
                    LeanTween.moveLocalX(gameObject,
                            leftMovement ? startPosition.x - width * entries : startPosition.x - width * entries,
                            scaleTime).setEaseInOutQuad();
                }
            }

            if (lastSelected != -1)
                pageIndicators[lastSelected].Deselect();
            pageIndicatorScaleValues[currentPageIndex] = scaleSelected;
            pageIndicators[currentPageIndex].Select(); 
            UpdateIndicators();
            lastSelected = this.currentPageIndex;
        }

        //
        void UpdateSimpleState(int currentPageIndex)
        {
            if (lastSelected != -1)
            {
                AnimateScale(pageIndicators[lastSelected], scaleNormal);
                AnimateColor(pageIndicators[lastSelected], normalColor);
                pageIndicators[lastSelected].Deselect();
            }

            AnimateScale(pageIndicators[currentPageIndex], scaleSelected);
            AnimateColor(pageIndicators[currentPageIndex], selectedColor);
            pageIndicators[currentPageIndex].Select();
            lastSelected = currentPageIndex;
        }

        [SerializeField] float scaleTime = .7f;

        public void AnimateColor(UIPageCountPointController pagerPoint, Color newColor)
        {
            
            LeanTween.color(pagerPoint.GetComponent<RectTransform>(), newColor, scaleTime).setEaseInOutQuad();
        }

        [SerializeField] int pageCountWidth = 18;
        [SerializeField] HorizontalLayoutGroup layoutGroup;

        public void AnimateScale(UIPageCountPointController pagerPoint, float scale)
        {
            LeanTween.scale(pagerPoint.gameObject, new Vector3(scale, scale, scale), scaleTime).setEaseInOutQuad();
        }


        int currentPageIndex = 0;

        public void Previous()
        {

            currentPageIndex--;
            if (currentPageIndex < 0)
                currentPageIndex = 0;
            OnPageUpdated(currentPageIndex);
        }

        public void Next()
        {

            currentPageIndex++;
            if (currentPageIndex >= indicatorCount)
                currentPageIndex = indicatorCount - 1;
            OnPageUpdated(currentPageIndex);
        }


        public void Hide()
        {
            gameObject.SetActive(false);
        }

        
    }
}
