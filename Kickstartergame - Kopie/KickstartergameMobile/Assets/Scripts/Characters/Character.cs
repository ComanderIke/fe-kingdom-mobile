
using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Items;
using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Skills;
using Assets.Scripts.Characters.Debuffs;
using Assets.Scripts.Characters.Attributes;
using Assets.Scripts.Characters.Classes;
using Assets.Scripts.Battle;
using AssemblyCSharp;


[System.Serializable]
public class Character : LivingObject{
    
    #region const
    public const int MAX_ITEMS = 6;
    private const int POINTS_PER_LVL = 3;
    private const int CRIT_DMG_MULT = 3;
    private const int MAX_EXP = 100;
    private const int EXP_PER_KILL = 40;
    private const int EXP_PER_BATTLE = 0;
    private const float GUI_ROTATION = 80;

    #endregion
    
    #region fields

	public Sprite activeSpriteObject;
	public Sprite passiveSprite_selected;
    [HideInInspector]
    public List<Item> items = new List<Item>();
	public CharacterClassType characterClassType;
	public CharClass charclass;
    public bool hasAttacked = false;
	public bool isActive;
	bool isWaiting;
    public Vector2 OldPosition;
    public Sprite passiveSpriteObject;
    public Sprite passiveSpriteMOObject;
    public bool hasMoved=false;
	private Weapon equipedWeapon;
	public GameObject weaponPosition;
    public int exp;
    private bool selected = false;
    public int x;
    public int y;
    public int attributepoints = 0;
    public Character lastdamagedealer;
    public int weaponIndex = 0;
    GameObject instantiatedWeapon;
    GameObject effectGO;
    #endregion

    
    #region Getter/Setter
    public Weapon EquipedWeapon {
		get{
			return equipedWeapon;
		}
		set{
			foreach (WeaponCategory w in charclass.weaponType) {
				if (value.weaponType.type == w.type) {
					equipedWeapon = value;
					if (gameObject != null) {
						if (instantiatedWeapon != null) {
							GameObject.Destroy (instantiatedWeapon);
						}
						instantiatedWeapon = GameObject.Instantiate<GameObject> (equipedWeapon.gameobject);
						instantiatedWeapon.transform.SetParent (gameObject.GetComponentInChildren<WeaponTransform> ().transform);
						instantiatedWeapon.transform.localRotation = equipedWeapon.gameobject.transform.localRotation;//Quaternion.Euler(2, 190, 290);
						instantiatedWeapon.transform.localScale = equipedWeapon.gameobject.transform.localScale;//= new Vector3(0.006f, 0.006f, 0.006f) 
						instantiatedWeapon.transform.localPosition = equipedWeapon.gameobject.transform.localPosition;
					}
					return;
				} 
			}
			Debug.Log (value);
			Debug.Log (value.weaponType);
			throw new Exception ("Falscher Waffentyp" + value.weaponType);
		}
	}
    public bool IsWaiting
    {
        get{
            return isWaiting;
        }
        set{
            isWaiting = value;
        }
    }
    public void SetPosition(int x, int y)
    {
        OldPosition = new Vector2(this.x, this.y);
        MainScript m = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>();
        m.gridScript.fields[(int)GetPositionOnGrid().x, (int)GetPositionOnGrid().y].character = null;
        this.x = x;
        this.y = y;
        gameObject.transform.localPosition = new Vector3(x, y, 0);
        m.gridScript.fields[x, y].character = this;
    }

    public int GetMaxAttackRange()
    {
        int max = 0;
        foreach(int attack in charclass.AttackRanges)
        {
            if (attack > max)
                max = attack;
        }
        return max;
    }
    public void SetInternPosition(int x, int y)
    {
        MainScript m = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>();      
        m.gridScript.fields[(int)GetPositionOnGrid().x, (int)GetPositionOnGrid().y].character = null;
        m.gridScript.fields[x, y].character = this;
        this.x = x;
        this.y = y;
    }
    private void ShowFightText(string damage, Character attacker, Character defender, FightTextType type)
    {
        FightText t = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponentInChildren<FightText>();
        Vector3 textPosition = new Vector3(gameObject.transform.position.x, 1, gameObject.transform.position.z);
        t.setText(textPosition,damage,type,attacker, defender);
    }

    public bool Selected{
		get{return selected;}
		set{
			if(gameObject!=null){
			    HighlightSelected h=gameObject.GetComponentInChildren<HighlightSelected> ();

			}
			selected = value;
		}
	}

   
    private void WaitAnimation()
    {
        ActiveUnitEffect a = gameObject.GetComponentInChildren<ActiveUnitEffect>();
        if (a != null)
        {
            GameObject.Destroy(a.transform.parent.gameObject);
        }
    }

    public void Heal(int heal)
    {
        FightText t = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponentInChildren<FightText>();
        Vector3 textPosition = new Vector3(this.gameObject.transform.position.x, 1, this.gameObject.transform.position.z);
        string text = "";
        FightTextType type;
        text = "" + (heal);
        type = FightTextType.Heal;
        t.setText(textPosition, text, type, null, this);
        HealEffect();
        this.HP += heal;
    }
    private void HealEffect()
    {
        if (this.gameObject != null)
        {
            Vector3 pos = this.gameObject.transform.position;
            effectGO = GameObject.Instantiate(GameObject.Find("RessourceScript").GetComponent<EffectsScript>().HealOnChar, pos, Quaternion.identity) as GameObject;
        }
    }
    
    #endregion


    public Character (string name, CharacterClassType type)
    {
        this.name = name;
        items.Capacity = MAX_ITEMS;
        level = 1;
        this.characterClassType = type;
        if (characterClassType == CharacterClassType.SwordFighter)
        {
			activeSpriteObject = GameObject.FindObjectOfType<SpriteScript>().swordActiveSprite;
            charclass = new Tank();
        }
        if (characterClassType == CharacterClassType.Hellebardier)
        {
			activeSpriteObject = GameObject.FindObjectOfType<SpriteScript>().lancerActiveSprite;
            charclass = new Hellebardier();
        }
        if (characterClassType == CharacterClassType.Mage)
        {
			activeSpriteObject = GameObject.FindObjectOfType<SpriteScript>().axeActiveSprite;
            charclass = new Mage();
           
        }
        if (characterClassType == CharacterClassType.Archer)
        {
			activeSpriteObject = GameObject.FindObjectOfType<SpriteScript>().archerActiveSprite;
            charclass = new Archer();
        }
        stats = new Stats(charclass.stats.maxHP, charclass.stats.attack, charclass.stats.speed, charclass.stats.defense, charclass.stats.accuracy, charclass.stats.spirit);
        HP = stats.maxHP;
    }

    public void AutomaticLevelUp(int targetLevel)
    {
        for (int i= this.level; i < targetLevel; i++)
        {
            AutomaticLevelUp();
        }
    }
	public int GetMaxGrowth(List<int> contains){
		int sum = 0;
		for (int i=0; i<7; i++) {
			if (!contains.Contains (i)) {
				switch (i) {
				case 0:sum += charclass.hpgrowth;
					break;
				case 1:sum += charclass.attackgrowth;
					break;
				case 2:sum += charclass.speedgrowth;
					break;
				case 3:sum += charclass.defensegrowth;
					break;
				case 4:sum += charclass.accuracygrowth;
					break;
				case 5:sum += charclass.spiritgrowth;
					break;

				}
			}
				
		}
		return sum;
	}

    public void AutomaticLevelUp()
    {
        level++;
		List<int> contains = new List<int> ();
		int points = 4;
		if (TransferData.difficulty == Difficulty.Hard) {
			points = 5;
		}
		if (TransferData.difficulty == Difficulty.Easy) {
			points = 3;
		}
        for(int i=0; i < points; i++)
        {
			int max = GetMaxGrowth (contains);
            int rng = UnityEngine.Random.Range(1, max);
            int border = 0;
            if (!contains.Contains(0))
            {
                if (rng <= (border += charclass.hpgrowth))
                {
                    stats.maxHP += 2;
                    contains.Add(0);
                    continue;
                }
			} if (!contains.Contains (1)) {
				if (rng <= (border += charclass.attackgrowth) && !contains.Contains (1)) {
					stats.attack += 1;
					contains.Add (1);
					continue;
				}
			} if (!contains.Contains (2)) {
				if (rng <= (border += charclass.speedgrowth) && !contains.Contains (2)) {
					stats.speed += 1;
					contains.Add (2);
					continue;
				}
			} if (!contains.Contains (3)) {
				if (rng <= (border += charclass.defensegrowth) && !contains.Contains (3)) {
					stats.defense += 1;
					contains.Add (3);
					continue;
				}
			} if (!contains.Contains (4)) {
				if (rng <= (border += charclass.accuracygrowth) && !contains.Contains (4)) {
					stats.accuracy += 1;
					contains.Add (4);
					continue;
				}
			} if (!contains.Contains (5)) {
				if (rng <= (border += charclass.spiritgrowth) && !contains.Contains (5)) {
					stats.accuracy += 1;
					contains.Add (5);
					continue;
				}
			}
        }
    }

    public override void DeathAnimation (){

        GameObject.FindObjectOfType<MainScript>().gridScript.fields[x, y].character = null;
        lastdamagedealer.GetExpForKill(this);
        if (instantiatedWeapon != null)
        {
            GameObject.Destroy(instantiatedWeapon);
        }
        //gameObject.GetComponent<CharacterScript>().PlayDeath();
       
    }

    public void addItem(Item i){
		items.Add (i);
	}

	public void dropItem(Item i){
		items.Remove (i);
	}

	public void useItem(Item i){
		i.use(this);
		if (i.Usage <= 0) {
			items.Remove (i);
		}
	}

    public void addExp(int exp) {
		this.exp += exp;
        //gameObject.GetComponent<CharacterScript>().StartAnimatedText("+ "+exp+ " Exp", GameObject.FindObjectOfType<MaterialScript>().yellow, 0, 50);
        if (this.exp >= MAX_EXP){
            this.exp -= MAX_EXP;
			levelUp();
		}
	}

    public Vector2 GetPositionOnGrid(){
		return new Vector2 (this.x, this.y);
	}
    public void UpdateOnWholeTurn()
    {
        List<Debuff> debuffEnd = new List<Debuff>();
        List<Buff> buffEnd = new List<Buff>();
        foreach (Debuff d in Debuffs)
        {
            if (d.TakeEffect(this))
                debuffEnd.Add(d);
        }
        foreach (Buff b in buffs)
        {
            if (b.TakeEffect(this))
                buffEnd.Add(b);
        }
        foreach (Debuff d in debuffEnd)
        {
            d.End(this);
        }
        foreach (Buff b in buffEnd)
        {
            b.End(this);
        }
    }
    public void UpdateTurn()
    {
        hasMoved = false;
        hasAttacked = false;
    }

    
	public void levelUp(){
        level++;
	}

    
    public void AddAttribute(StatAttribute attr, int value, float delay)
    {
        string text = "";
        switch (attr)
        {
            case StatAttribute.Defense:stats.defense += value; text += "Defense + " + value; break;
            case StatAttribute.Attack: stats.attack += value; text += "Attack + " + value; break;
            case StatAttribute.Accuracy: stats.accuracy += value; text += "Accuracy + " + value; break;
            case StatAttribute.HP: stats.maxHP += value; text += "HP + " + value; break;
            case StatAttribute.Speed: stats.speed += value; text += "Speed + " + value; break;
            case StatAttribute.Spirit: stats.spirit += value; text += "Spirit + " + value; break;
        }
        //gameObject.GetComponent<CharacterScript>().StartAnimatedText(text, GameObject.FindObjectOfType<MaterialScript>().orange,delay,80);
    }
    public int inflictMagicDamage(int dmg, Character damagedealer)
    {
        lastdamagedealer = damagedealer;
        
        if (Battle.IsEffectiveAgainst(this.equipedWeapon.weaponType, damagedealer.EquipedWeapon.weaponType))
        {
            dmg = dmg - Battle.BonusDamage;
        }
        else if (Battle.IsEffectiveAgainst(damagedealer.equipedWeapon.weaponType, this.EquipedWeapon.weaponType))
        {
            dmg = dmg + Battle.BonusDamage;
        }
        float multiplier = 1.0f;
        FightTextType textType = FightTextType.Damage;
        HitEffect();
        dmg =(int)( dmg-stats.spirit * multiplier);

        if (dmg <= 0)
            dmg = 1;

            HP -= dmg;
		if (HP > 0) {
            if (dmg <= 5)
            {
                CameraShake.Shake(0.3f, 0.02f);
                //gameObject.GetComponent<CharacterScript>().GetHitAnimation();
            }
            else if (dmg < 10)
            {
                CameraShake.Shake(0.35f, 0.03f);
                //gameObject.GetComponent<CharacterScript>().GetHit2Animation();
            }
            else
            {
                CameraShake.Shake(0.35f, 0.1f);
                //gameObject.GetComponent<CharacterScript>().GetHit2Animation();
            }
		}
        else
        {
            CameraShake.Shake(0.35f, 0.12f);
        }
        ShowFightText("" + dmg, damagedealer,this, textType);
        BloodEffect();
       
        return dmg;
	}
    public override int GetHitRate()
    {
        int hit = 0;
        if (equipedWeapon != null)
            hit = equipedWeapon.hit;
        return base.GetHitRate()+hit;
    }
    public bool HasSkill(Type type)
    {
        foreach(Skill s in this.charclass.skills)
        {
            if(s.GetType() == type)
            {
                return true;
            }
        }
        return false;
    }
    public Skill GetSkill(Type type)
    {

        foreach (Skill s in this.charclass.skills)
        {
            if (s.GetType() == type)
            {
                return s;
            }
        }
        return null;
    }

    public int InflictDamage(int dmg, Character damagedealer)
    {
        lastdamagedealer = damagedealer;
        float multiplier = 1.0f;
        if(Battle.IsEffectiveAgainst(this.equipedWeapon.weaponType, damagedealer.EquipedWeapon.weaponType))
        {
            dmg = dmg - Battle.BonusDamage;
        }
        else if (Battle.IsEffectiveAgainst(damagedealer.equipedWeapon.weaponType, this.EquipedWeapon.weaponType))
        {
            dmg = dmg + Battle.BonusDamage;
        }
         FightTextType textType = FightTextType.Damage;
        int inflictedDmg = (int)((dmg - stats.defense)*multiplier);

		if (inflictedDmg <= 0)
			inflictedDmg = 1;
	

        HitEffect();
		BloodEffect();
        
		HP -= inflictedDmg;
		if (HP > 0) {
            if (inflictedDmg <= 5)
            {
                //gameObject.GetComponent<CharacterScript>().GetHitAnimation();
                CameraShake.Shake(0.3f, 0.02f);
            }
            else if(inflictedDmg <10)
            {
                CameraShake.Shake(0.35f, 0.03f);
                //gameObject.GetComponent<CharacterScript>().GetHit2Animation();
            }
            else
            {
                CameraShake.Shake(0.35f, 0.1f);
                //gameObject.GetComponent<CharacterScript>().GetHit2Animation();
            }
		}
        else
        {
            CameraShake.Shake(0.35f, 0.12f);
        }
        ShowFightText(""+inflictedDmg, damagedealer, this, textType);
		return inflictedDmg;
	}
    private void BloodEffect()
    {
        BloodPosition BloodPos = this.gameObject.GetComponentInChildren<BloodPosition>();
        GameObject.Instantiate(GameObject.Find("RessourceScript").GetComponent<EffectsScript>().GetRandomBloodEffect(),BloodPos.transform.position, Quaternion.identity);
    }
    private void HitEffect()
    {
        BloodPosition BloodPos = this.gameObject.GetComponentInChildren<BloodPosition>();
        GameObject.Instantiate(GameObject.Find("RessourceScript").GetComponent<EffectsScript>().ClashOnChar, BloodPos.transform.position, Quaternion.identity);
    }

    public int GetDamage(bool justToShow){
		int weaponDamage = 0;
		if(EquipedWeapon!=null)
			weaponDamage = EquipedWeapon.dmg;
		return (int)Mathf.Clamp ((stats.attack + weaponDamage), 0, Mathf.Infinity);
	}

    public int GetDamageAgainstTarget(Character target){
		int weaponDamage = 0;
		if(EquipedWeapon!=null)
			weaponDamage = EquipedWeapon.dmg;
        if (Battle.IsEffectiveAgainst(this.equipedWeapon.weaponType, target.EquipedWeapon.weaponType))
        {
            weaponDamage = weaponDamage + Battle.BonusDamage;
        }
        else if (Battle.IsEffectiveAgainst(target.equipedWeapon.weaponType, this.EquipedWeapon.weaponType))
        {
            weaponDamage = weaponDamage - Battle.BonusDamage;
        }
        float multiplier = 1.0f;
		if (equipedWeapon.weaponType.type == WeaponType.Magic)
            
            return (int)(multiplier*Mathf.Clamp(stats.attack + weaponDamage-target.stats.spirit, 1, Mathf.Infinity));
        else
            return (int)(multiplier * Mathf.Clamp(stats.attack + weaponDamage - target.stats.defense, 1, Mathf.Infinity));

	}
    float getRotation(global::Character a, Character b)
    {
        int xa = (int)a.gameObject.transform.localPosition.x;
        int za = (int)a.gameObject.transform.localPosition.z;
        int xb =(int)b.gameObject.transform.localPosition.x;
        int zb = (int)b.gameObject.transform.localPosition.z;
        int deltax = Mathf.Abs(xa - xb);
        int deltaz = Mathf.Abs(za - zb);
        float value = 0;
        if (xa > xb)
        {
            if (za > zb)
            {
                if (deltax > deltaz)
                {
                    value = 45 + 22.5f;
                }
                else if (deltax < deltaz)
                {
                    value = 45 - 22.5f;
                }
                else
                {
                    value = 45;
                }

            }
            else if (za < zb)
            {

                if (deltax > deltaz)
                {
                    value = 315 - 22.5f;
                }
                else if (deltax < deltaz)
                {
                    value = 315 + 22.5f;
                }
                else
                {
                    value = 315;
                }

            }
            else
            {
                value = 270;
            }

        }
        else if (xa < xb)
        {
            if (za > zb)
            {

                if (deltax > deltaz)
                {

                    value = 225 - 22.5f;
                }
                else if (deltax < deltaz)
                {
                    value = 225 - 22.5f;
                }
                else
                {
                    value = 225;
                }
            }
            else if (za < zb)
            {

                if (deltax > deltaz)
                {
                    value = 135 - 22.5f;
                }
                else if (deltax < deltaz)
                {
                    value = 135 + 22.5f;
                }
                else
                {
                    value = 135;
                }
            }
            else
            {
                value = 90;
            }

        }
        if (za > zb)
        {
            if (xa > xb)
            {
                if (deltax > deltaz)
                {
                    value = 225 + 22.5f;
                }
                else if (deltax < deltaz)
                {
                    value = 225 - 22.5f;
                }
                else
                {
                    value = 225;
                }
            }
            else if (xa < xb)
            {

                if (deltax > deltaz)
                {

                    value = 135 - 22.5f;

                }
                else if (deltax < deltaz)
                {
                    value = 135 + 22.5f;

                }
                else
                {
                    value = 135;
                }
            }
            else
            {
                value = 180;
            }

        }
        else if (za < zb)
        {
            if (xa > xb)
            {
                if (deltax > deltaz)
                {
                    value = 315 - 22.5f;
                }
                else if (deltax < deltaz)
                {
                    value = 315 + 22.5f;
                }
                else
                {
                    value = 315;
                }
            }
            else if (xa < xb)
            {

                if (deltax > deltaz)
                {
                    value = 45 + 22.5f;
                }
                else if (deltax < deltaz)
                {
                    value = 45 - 22.5f;
                }
                else
                {
                    value = 45;
                }
            }
            else
            {
                value = 0;
            }
        }

        return value;
    }
    public int GetTotalDamageAgainstTarget(Character target)
    {
        int weaponDamage = 0;
        int attacks = 1;
        if (EquipedWeapon != null)
            weaponDamage = EquipedWeapon.dmg;
        if (Battle.IsEffectiveAgainst(this.equipedWeapon.weaponType, target.EquipedWeapon.weaponType))
        {
            weaponDamage = weaponDamage + Battle.BonusDamage;
        }
        else if (Battle.IsEffectiveAgainst(target.equipedWeapon.weaponType, this.EquipedWeapon.weaponType))
        {
            weaponDamage = weaponDamage - Battle.BonusDamage;
        }
        float multiplier = 1.0f;
        if (CanDoubleAttack(target))
            attacks = 2;
        if (equipedWeapon.weaponType.type == WeaponType.Magic)
            return (int)(multiplier *attacks* Mathf.Clamp(stats.attack + weaponDamage - target.stats.spirit, 0, Mathf.Infinity));
        else
            return (int)(multiplier *attacks* Mathf.Clamp(stats.attack + weaponDamage - target.stats.defense, 0, Mathf.Infinity));

    }

    public override int GetHitAgainstTarget(LivingObject target){
		int weaponHit = 0;
		if(EquipedWeapon!=null)
			weaponHit = EquipedWeapon.hit;
        return  (int)Mathf.Clamp(weaponHit + base.GetHitAgainstTarget(target), 0, 100);
	}

    public bool CanAttack(int range)
    {
        return this.charclass.AttackRanges.Contains(range);
    }
    public bool CanKillTarget(Character target)
    {
        return GetDamageAgainstTarget(target) >= target.HP;
    }
    public Weapon GetWeapon()
    {
        if(EquipedWeapon == null)
        Debug.Log("NO WEAPON!");
        return EquipedWeapon;
    }

    public override int getCrit(){
		int weaponCrit  = 0;
		if(EquipedWeapon!=null)
			weaponCrit = EquipedWeapon.crit;
		return  base.getCrit() + weaponCrit;
	}

	public void GetExpForKill(LivingObject enemy)
    {
        if (player.isPlayerControlled)
        {
            Debug.Log(enemy.name+" Killed by: " + name + " " + charclass);
			addExp(EXP_PER_KILL);
        }
    }
	public void PlayDogueAnimation (){
		//gameObject.GetComponent<CharacterScript>().PlayDogueAnimation ();
	}

	public void DespawnWeapon(){
		GameObject.Destroy (instantiatedWeapon);
	}
    public void InstantiateWeapon()
    {
        if (equipedWeapon != null)
        {
			instantiatedWeapon = GameObject.Instantiate<GameObject>(equipedWeapon.gameobject);
            instantiatedWeapon.transform.SetParent(gameObject.GetComponentInChildren<WeaponTransform>().transform);
			instantiatedWeapon.transform.localRotation = equipedWeapon.gameobject.transform.localRotation;//Quaternion.Euler(2, 190, 290);
			instantiatedWeapon.transform.localScale = equipedWeapon.gameobject.transform.localScale;//= new Vector3(0.006f, 0.006f, 0.006f) 
            instantiatedWeapon.transform.localPosition = equipedWeapon.gameobject.transform.localPosition;
        }
    }
   
    
}



