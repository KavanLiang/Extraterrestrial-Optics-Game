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
	public GameObject particlePrefab;

	// Start is called before the first frame update
	void Start() {
		hp = max_hp;
		shields.Sort((a, b) => Random.Range(-1, 1));
		int n = shields.Count;
		while(n > 1) {
			n--;
			int k = Random.Range(0, n);
			GameObject swap = shields[k];
			shields[k] = shields[n];
			shields[n] = swap;
		}

		for(int i = 0; i < 4 - max_shields; i++) {
			shields[i].SetActive(false);
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
		GameObject particles = Instantiate(particlePrefab, transform.position, Quaternion.identity);
		Destroy(gameObject);
		Destroy(particles, 2f);
	}
}
