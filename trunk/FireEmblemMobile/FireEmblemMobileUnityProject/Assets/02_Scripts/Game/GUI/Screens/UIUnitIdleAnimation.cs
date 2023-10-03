using Game.GameActors.Units;
using LostGrace;
using UnityEngine;

public class UIUnitIdleAnimation : MonoBehaviour{
    [SerializeField] Animator IdleAnimation;
    [SerializeField] private UIAnimationSpriteSwapper uiAnimationSpriteSwapper;
    public void Show(Unit unit)
    {
        IdleAnimation.runtimeAnimatorController = unit.visuals.Prefabs.UIAnimatorController;
        uiAnimationSpriteSwapper.Init(unit.visuals.CharacterSpriteSet);
    }
}