using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour {
	private float hp;
	public float speed;
	public float dmg;
	public int ScoreValue;
	public List<GameObject> shields;
	public int max_shields;

	// Start is called before the first frame update
	void Start() {
		hp = 100;
		int numShields = 4;
		while(numShields > max_shields) {
			int randIndex = (int) Random.Range(0, shields.Count - 1);
			Debug.Log(randIndex);
			GameObject removeShield = shields[randIndex];
			shields.RemoveAt(randIndex);
			Destroy(removeShield);
			numShields--;
		}
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
		GameManager.Instance.IncrementScore(ScoreValue);
		Destroy(gameObject);
	}
}
