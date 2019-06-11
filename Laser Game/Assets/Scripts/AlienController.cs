using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour {
	public float max_hp;
	private float hp;
	public float speed;
	public float dmg;
	public int ScoreValue;
	public List<GameObject> shields;
	public int max_shields;

	// Start is called before the first frame update
	void Start() {
		hp = max_hp;
		shields.Sort((a, b) => Random.Range(-1, 1));
		int numShields = 4;
		while(numShields > max_shields) {
			GameObject removeShield = shields[0];
			shields.RemoveAt(0);
			Destroy(removeShield);
			numShields--;
		}
	}

	public void DecrementHp(float dmg) {
		float dealt = dmg * Time.deltaTime;
		hp -= dealt;
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
	/// Kills this Alien
	/// </summary>
	void Die() {
		GameManager.Instance.IncrementScore(ScoreValue);
		Destroy(gameObject);
	}
}
