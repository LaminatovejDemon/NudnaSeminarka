using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private float health = 100f;
	private float playerDamage = 25f;

	public void GetHit (float damage) {
		health -= damage;
	}

	public float GetPlayerDamage () {
		return playerDamage;
	}

	public float GetHealth () {
		return health;
	}
}
