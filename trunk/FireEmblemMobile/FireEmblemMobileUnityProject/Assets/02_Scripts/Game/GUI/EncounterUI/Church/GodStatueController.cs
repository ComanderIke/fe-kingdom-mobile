using System.Collections.Generic;
using UnityEngine;

namespace LostGrace
{
    public class GodStatueController : MonoBehaviour
    {
        [SerializeField] private List<SpriteRenderer> statues;
        private List<God> gods;
        private int currentGod = 0;
        private int currentStatue = 0;
        public void Previous()
        {
            currentStatue++;
            if (currentStatue >statues.Count-1)
                currentStatue = 0;
            currentGod++;
            if (currentGod > gods.Count-1)
                currentGod = 0;
            UpdateGodStatues();
        }

        public void Next()
        {
            currentStatue--;
            if (currentStatue < 0)
                currentStatue = statues.Count - 1;
           
            currentGod--;
            if (currentGod < 0)
                currentGod = gods.Count - 1;
            UpdateGodStatues();
        }

        void UpdateGodStatues()
        {
            int nextStatueIndex = currentStatue + 1;
            int prevStatueIndex = currentStatue - 1;
            if (nextStatueIndex >statues.Count-1)
                nextStatueIndex = 0;
            if (prevStatueIndex < 0)
                prevStatueIndex = statues.Count - 1;
            int nextGodIndex = currentGod + 1;
            int prevGodIndex = currentGod - 1;
            if (nextGodIndex > gods.Count-1)
                nextGodIndex = 0;
            if (prevGodIndex < 0)
                prevGodIndex = gods.Count - 1;
            MyDebug.LogTest("GodIndex: "+currentGod+" Next: "+nextGodIndex+" Prev: "+prevGodIndex);
            MyDebug.LogTest("StatueIndex: "+currentStatue+" Next: "+nextStatueIndex+" Prev: "+prevStatueIndex);
            statues[currentStatue].sprite = gods[currentGod].StatueSprite;
            statues[nextStatueIndex].sprite = gods[nextGodIndex].StatueSprite;
            statues[prevStatueIndex].sprite = gods[prevGodIndex].StatueSprite;
        }

        public void Reset()
        {
            currentStatue = 0;
            currentGod = 0;
            UpdateGodStatues();
        }

        public void SetGods(List<God> gods)
        {
            this.gods = gods;
          
        }
    }
}