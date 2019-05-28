using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour {
	private float hp;
	public float speed;
	public float dmg;
	// Start is called before the first frame update
	void Start() {
		hp = 100;
	}

	public void DecrementHp(float dmg) {
		hp -= dmg;
	}

	// Update is called once per frame
	void Update() {
		transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
		if(transform.position.x < -GameManager.Instance.WorldWidth / 2) {
			GameManager.Instance.DecrementPlayerHealth(dmg);
			Destroy(gameObject);
		}
		if(hp <= 0) {
			Die();
		}
	}

	/// <summary>
	/// Kills this Vampire
	/// </summary>
	void Die() {
		Destroy(gameObject);
	}
}
