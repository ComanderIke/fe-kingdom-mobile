using Game.GUI.Controller;
using UnityEngine;

namespace Game.GUI
{
    public class UIRessourceAmount : MonoBehaviour
    {
        [SerializeField] private UIAnimatedCountingText countingText;
        [SerializeField] private ParticleSystem earnedEffect;
        [SerializeField] private ParticleSystem lostEffect;
        
        private int amount;
        private bool init = true;

        public int Amount
        {
            get
            {
                return amount;
            }
            set
            {
                if (init)
                {
                    amount = value;
                    init = false;
                    countingText.SetText(amount.ToString());
                    return;
                }
                if (value > amount)
                {
                    earnedEffect.Play();
                }
                else if (value < amount)
                {
                    lostEffect.Play();
                }
                countingText.SetTextCounting(amount, value);
                amount = value;
            }
        }

       
        

       

        
    }
}
