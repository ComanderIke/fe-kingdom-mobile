using Game.GameResources;
using Game.GUI;
using Game.WorldMapStuff.Model;

namespace Game.Menu
{
    public class UICampaignController : UIMenu
    {

   

        public void StartCampaign(int selected)
        {
            Campaign.Instance.LoadConfig(GameData.Instance.campaigns[selected]);
            MainMenuController.Instance.LoadScene(GameData.Instance.campaigns[selected].scene);
        }
    }
}
