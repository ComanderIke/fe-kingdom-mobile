using Game.GUI.PopUpText;
using Game.Utility;
using UnityEngine;

namespace Game.Graphics.BattleAnimations
{
    public class CombatTextRenderer
    {
        public float MissTextScale = 3f;
        public float TextScale = 5f;
        public float NoDamageTextScale = 2.8f;
        public float BattleTextMovementScale = 6f;
        public float DamageNumberHorizontalMinValue=0.4f;
        public float DamageNumberHorizontalMaxValue=0.65f;
        public float DamageNumberVerticalMinValue=0.4f;
        public float DamageNumberVerticalMaxValue=0.6f;
        public CombatTextRenderer()
        {
            CharacterCombatAnimations.OnDodge -= CreateMiss;
            CharacterCombatAnimations.OnDamaged -= CreateDamageNumbers;
            CharacterCombatAnimations.OnDodge += CreateMiss;
            CharacterCombatAnimations.OnDamaged += CreateDamageNumbers;
        }


        void CreateDamageNumbers(AnimatedCombatCharacter character, int dmg, bool critical)
        {
            TextStyle style = TextStyle.Damage;
            // Debug.Log("Impact Pos from Somewhere else");
            // foreach (var impactPos in GameObject.FindObjectsOfType<ImpactPosition>())
            // {
            //     Debug.Log(impactPos.transform.position);
            // }
            // Debug.Log("create Damage Number: "+character.GameObject+" "+character.GetImpactPosition()+" InstanceId:"+character.GetSpriteController().GetInstanceID());
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
                        dmg, style, dmg==0?NoDamageTextScale:TextScale,
                        new Vector3(
                            floatDirection * Random.Range(DamageNumberHorizontalMinValue, DamageNumberHorizontalMaxValue),
                            Random.Range(DamageNumberVerticalMinValue, DamageNumberVerticalMaxValue)) * BattleTextMovementScale),
                0.05f);

        }
        void CreateMiss(AnimatedCombatCharacter character)
        {
            Debug.Log("create Miss: "+character.GameObject+" "+character.GetImpactPosition()+" InstanceId:"+character.GetSpriteController().GetInstanceID());
            var pos = character.GetImpactPosition();
            CreateMiss(pos, character.IsLeft()?-1:1);
        }
        void CreateMiss(Vector3 position, float floatDirection=1)
        {
            MonoUtility.DelayFunction(() => DamagePopUp.CreateMiss(position, TextStyle.Missed, MissTextScale, 
                new Vector2(floatDirection*Random.Range(DamageNumberHorizontalMinValue,DamageNumberHorizontalMaxValue), Random.Range(DamageNumberVerticalMinValue,DamageNumberVerticalMaxValue))
                * BattleTextMovementScale),0.05f);
        }

        public void Cleanup()
        {
            CharacterCombatAnimations.OnDodge -= CreateMiss;
            CharacterCombatAnimations.OnDamaged -= CreateDamageNumbers;
        }
    }
}