using Game.GameActors.Player;
using Game.GUI;
using UnityEngine;

namespace Game
{
    public class UISanctuaryHud : MonoBehaviour
    {
        [SerializeField] private UIRessourceAmount gracePanel;
        // Start is called before the first frame update
        void Start()
        {
            gracePanel.Amount = Player.Instance.Grace;
            Player.Instance.onGraceValueChanged += GraceValueChanged;
        }

        private void OnDisable()
        {
            Player.Instance.onGraceValueChanged -= GraceValueChanged;
        }

        void GraceValueChanged()
        {
            gracePanel.Amount = Player.Instance.Grace;
        }
        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
