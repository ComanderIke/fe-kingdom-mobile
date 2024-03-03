using System;
using System.Collections.Generic;
using Game.Utility;
using UnityEngine;

namespace Game.GUI
{
    public class UIChronikEntryList : MonoBehaviour
    {
        [SerializeField] private GameObject entryPrefab;
        [SerializeField] private Transform entryContainer;
        private List<IChronikEntry> entries;
        private List<UIChronikEntry> entrieUIs;
        public event Action<IChronikEntry> OnSelectEntry;
        public void Init(List<IChronikEntry> entries)
        {
            this.entries = entries;
            entrieUIs = new List<UIChronikEntry>();
            entryContainer.DeleteChildren();
            foreach (var entry in entries)
            {
                var go = Instantiate(entryPrefab, entryContainer);
                var chronikEntryUI =go.GetComponent<UIChronikEntry>();
                chronikEntryUI.SetEntry(entry);
                chronikEntryUI.OnClicked += EntryClicked;
                entrieUIs.Add(chronikEntryUI);
            }
        }

        void EntryClicked(UIChronikEntry entry)
        {
            OnSelectEntry?.Invoke(entry.entry);
        }
        public void UpdateUI(int currentIndex)
        {
            foreach (var entry in entrieUIs)
            {
                entry.Deselect();
            }

            entrieUIs[currentIndex].Select();
        }
    }
}