using MoreMountains.Feedbacks;
using UnityEngine;

public class HitFeedbackController : MonoBehaviour
{
    private DamagedState state = DamagedState.Damage;
    [SerializeField] private MMF_Player hitFeedback;
    [SerializeField] private MMF_Player strongHitFeedback;
    [SerializeField] private MMF_Player noDmgFeedback;
    public void SetState(DamagedState state)
    {
        this.state = state;
    }

    public void PlayHitFeedback()
    {
        switch (state)
        {
            case DamagedState.Damage:
                hitFeedback.PlayFeedbacks();break;
            case DamagedState.NoDamage:
                noDmgFeedback.PlayFeedbacks();break;
            case DamagedState.HighDmg:
                strongHitFeedback.PlayFeedbacks();break;
            
        }
    }
}