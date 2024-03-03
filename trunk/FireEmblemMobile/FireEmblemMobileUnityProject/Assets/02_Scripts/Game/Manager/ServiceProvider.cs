using System.Collections;
using Game.EncounterAreas.Management;

namespace Game.Manager
{
    public class ServiceProvider
    {
        public static IServiceProvider Instance
        {
            get
            {
                if(GridGameManager.Instance!=null)
                    return GridGameManager.Instance;
                if(AreaGameManager.Instance!=null)
                    return AreaGameManager.Instance;
                return null;
            }
        }

        
    }
}