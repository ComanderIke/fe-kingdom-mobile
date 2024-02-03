using Game.GameActors.Units;
using LostGrace;
using UnityEngine;

public class UIUnitIdleAnimation : MonoBehaviour{
    [SerializeField] Animator IdleAnimation;
    [SerializeField] private UIAnimationSpriteSwapper uiAnimationSpriteSwapper;
    private static readonly int Run = Animator.StringToHash("Run");

    public void Show(Unit unit)
    {
        IdleAnimation.runtimeAnimatorController = unit.visuals.Prefabs.UIAnimatorController;
        uiAnimationSpriteSwapper.Init(unit.visuals.CharacterSpriteSet);
    }

    public void PlayRunning(bool left)
    {
        MyDebug.LogTest("PlayRunning");
        transform.localScale = new Vector3(left?-1:1, 1, 1);
        IdleAnimation.SetTrigger(Run);
    }
}