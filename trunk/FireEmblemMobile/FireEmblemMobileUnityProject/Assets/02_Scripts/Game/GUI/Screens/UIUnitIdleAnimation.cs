using Game.GameActors.Units;
using UnityEngine;

public class UIUnitIdleAnimation : MonoBehaviour{
    [SerializeField] Animator IdleAnimation;

    public void Show(Unit unit)
    {
        IdleAnimation.runtimeAnimatorController = unit.visuals.Prefabs.UIAnimatorController;
    }
}