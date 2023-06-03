using Game.GUI.PopUpText;
using UnityEngine;

public class CombatTextRenderer
{
    public float BattleTextMovementScale = 8f;
    public float DamageNumberHorizontalMinValue=0.8f;
    public float DamageNumberHorizontalMaxValue=1.6f;
    public float DamageNumberVerticalMinValue=0.4f;
    public float DamageNumberVerticalMaxValue=0.7f;
    public CombatTextRenderer()
    {
        CharacterCombatAnimations.OnDodge += CreateMiss;
        CharacterCombatAnimations.OnDamaged += CreateDamageNumbers;
    }


    void CreateDamageNumbers(AnimatedCombatCharacter character, int dmg, bool critical)
    {
        TextStyle style = TextStyle.Damage;
        Debug.Log("create Damage Number: "+character.GameObject+" "+character.GetImpactPosition());
        var pos = character.GetImpactPosition();
        if (dmg == 0)
            style = TextStyle.NoDamage;
        if(critical)
            style = TextStyle.Critical;
        CreateDamageText(pos, dmg, style, character.IsLeft()?-1:1);
    
        
    }
    void CreateDamageText(Vector3 position, int dmg, TextStyle style, float floatDirection=1)
    {
        MonoUtility.DelayFunction(() =>
            DamagePopUp.CreateForBattleView(position,
                dmg, style, 5.0f,
                new Vector3(
                    floatDirection * Random.Range(DamageNumberHorizontalMinValue, DamageNumberHorizontalMaxValue),
                    Random.Range(DamageNumberVerticalMinValue, DamageNumberVerticalMaxValue)) * BattleTextMovementScale),
                0.05f);

    }
    void CreateMiss(AnimatedCombatCharacter character)
    {
        var pos = character.GetImpactPosition();
        CreateMiss(pos, character.IsLeft()?-1:1);
    }
    void CreateMiss(Vector3 position, float floatDirection=1)
    {
        MonoUtility.DelayFunction(() => DamagePopUp.CreateMiss(position, TextStyle.Missed, 4.0f, 
            new Vector2(floatDirection*Random.Range(DamageNumberHorizontalMinValue,DamageNumberHorizontalMaxValue), Random.Range(DamageNumberVerticalMinValue,DamageNumberVerticalMaxValue))
            * BattleTextMovementScale),0.05f);
    }
}