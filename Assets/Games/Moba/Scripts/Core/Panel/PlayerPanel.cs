using UnityEngine;
using System.Collections;


public class PlayerPanel : MonoBehaviour {

	public GameObject root;
	public UIProgressBar healthProgressBar;
	public UIProgressBar expProgressBar;
	public UILabel healthLabel;
	public UILabel expLabel;
	public UILabel levelLabel;
	public UILabel damageLabel;
	public UILabel cornLabel;

	UnitAttribute mUnitAttribute;

	UnitAttribute mPreUnitAttribute;

	void Update()
	{
		if (mPreUnitAttribute == null || mUnitAttribute == null)
			return;
		if(mPreUnitAttribute.currentDamage != mUnitAttribute.currentDamage)
		{
			UpdateCurrentDamage();
		}
		if(mPreUnitAttribute.currentHealth != mUnitAttribute.currentHealth)
		{
			UpdateCurrentHealth ();
		}
		if(mPreUnitAttribute.maxHealth != mUnitAttribute.maxHealth)
		{
			UpdateMaxHealth ();
		}
		if(mPreUnitAttribute.corn != mUnitAttribute.corn)
		{
			UpdateCorn();
		}
		if(mPreUnitAttribute.level != mUnitAttribute.level)
		{
			UpdateLevel();
		}
		if(mPreUnitAttribute.exp != mUnitAttribute.exp)
		{
			UpdateExp();
		}
		if(mPreUnitAttribute.levelUpExp != mUnitAttribute.levelUpExp)
		{
			UpdateLevelUpExp();
		}

	}


	public void SetUnitAttribute(UnitAttribute ua)
	{
		mPreUnitAttribute = gameObject.AddMissingComponent<UnitAttribute>();
		mUnitAttribute = ua;
	}

	void UpdateLevelUpExp(){
		mPreUnitAttribute.levelUpExp = mUnitAttribute.levelUpExp;
		expLabel.text = mUnitAttribute.exp + " / " + mUnitAttribute.levelUpExp;
		expProgressBar.value = (float)mUnitAttribute.exp / mUnitAttribute.levelUpExp;
	}

	void UpdateExp(){
		mPreUnitAttribute.exp = mUnitAttribute.exp;
		expLabel.text = mUnitAttribute.exp + " / " + mUnitAttribute.levelUpExp;
		expProgressBar.value = (float)mUnitAttribute.exp / mUnitAttribute.levelUpExp;
	}

	void UpdateCorn(){
		mPreUnitAttribute.corn = mUnitAttribute.corn;
		cornLabel.text = "Corn " + mUnitAttribute.corn;
	}

	void UpdateLevel(){
		mPreUnitAttribute.level = mUnitAttribute.level;
		levelLabel.text = "Level " + mUnitAttribute.level;
	}

	void UpdateMaxHealth(){
		mPreUnitAttribute.maxHealth = mUnitAttribute.maxHealth;
		healthLabel.text = mUnitAttribute.currentHealth + " / " + mUnitAttribute.maxHealth;
		healthProgressBar.value = (float)mUnitAttribute.currentHealth / mUnitAttribute.maxHealth;
	}

	void UpdateCurrentHealth(){
		mPreUnitAttribute.currentHealth = mUnitAttribute.currentHealth;
		healthLabel.text = mUnitAttribute.currentHealth + " / " + mUnitAttribute.maxHealth;
		healthProgressBar.value = (float)mUnitAttribute.currentHealth / mUnitAttribute.maxHealth;
	}

	void UpdateCurrentDamage()
	{
		mPreUnitAttribute.currentDamage = mUnitAttribute.currentDamage;
		damageLabel.text = "ATK " + mUnitAttribute.currentDamage;
	}



}
