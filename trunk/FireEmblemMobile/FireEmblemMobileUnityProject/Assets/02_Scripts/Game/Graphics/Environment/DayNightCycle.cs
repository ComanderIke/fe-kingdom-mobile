using UnityEngine;

namespace Game.Graphics.Environment
{
    public interface DayNightInterface
    {
        void SetParameter(float time);

    }
    [ExecuteInEditMode]
    public class DayNightCycle : MonoBehaviour
    {
        [Range(0,1)]
        public float time;

        public DayNightInterface[] dayNightInfluencers;
        public bool day;
        // Start is called before the first frame update
        void OnEnable()
        {
            dayNightInfluencers = GetComponentsInChildren<DayNightInterface>();
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var influencer in dayNightInfluencers)
                influencer.SetParameter(time);
        }
    }
}