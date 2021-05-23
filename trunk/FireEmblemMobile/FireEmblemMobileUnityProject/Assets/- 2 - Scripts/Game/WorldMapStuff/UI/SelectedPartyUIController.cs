using Game.WorldMapStuff.Interfaces;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.WorldMapStuff.UI
{
    public class SelectedPartyUIController : IPartySelectionRenderer
    {

        public TextMeshProUGUI currentPartyText;

        public IPartyScreenUI CharacterScreen;
        public Image sprite;
        public IPartyActionRenderer partyActionRenderer;


        private WM_Actor current;
        // Start is called before the first frame update

        public override void Show(WM_Actor actor)
        {
            current = actor;
            if (actor is Party party)
            {
                sprite.sprite = party.members[0].visuals.CharacterSpriteSet.FaceSprite;
                currentPartyText.SetText(party.name);
            }
            gameObject.SetActive(true);
        }
    

        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    

    
        public void JoinSplitClicked()
        {
            Debug.Log("JoinSplit clicked!");
            partyActionRenderer.Show((Party)current);
        }

        public void CharacterScreenClicked()
        {
            Debug.Log("CharacterScreen clicked!");
            CharacterScreen.Show((Party)current);
        }
    }
}