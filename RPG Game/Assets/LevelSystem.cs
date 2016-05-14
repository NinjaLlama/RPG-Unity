using UnityEngine;
using System.Collections;

public class LevelSystem : MonoBehaviour {

	public int level;
	public int exp;
	public int maxExp;
	public Fighter player;
	public float expPercentage;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		maxExp = (int)(Mathf.Pow (level, 2) + 100);
		LevelUp ();
		expPercentage = (float)exp / (float)maxExp;
	}

	void LevelUp()
	{
		if (exp >= maxExp) {
			exp = exp - (int)(Mathf.Pow (level, 2) + 100);
			level = level + 1;
			LevelEffect();
		}
	}

	void LevelEffect()
	{
		player.maxHealth = player.maxHealth + (int)Mathf.Pow (level, 3);
		player.maxMana = player.maxMana + (int)Mathf.Pow (level, 2);
		player.damage = player.damage + 10;
		player.health = player.maxHealth;
		player.mana = player.maxMana;
	}
}
