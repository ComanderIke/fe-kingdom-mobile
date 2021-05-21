using Game.WorldMapStuff.Model;

namespace Game.WorldMapStuff.Systems
{
    public interface IPartyActionRenderer
    {
        public void Hide();
        public void Show(Party selected);
        void ShowJoinButton();


        void HideJoinButton();


        void ShowSplitButton();
        void HideSplitButton();
    }
}