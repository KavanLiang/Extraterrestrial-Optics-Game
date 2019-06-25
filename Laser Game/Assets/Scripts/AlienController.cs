using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour {
	public float max_hp;
	private float hp;
	public float speed;
	public GameObject particlePrefab;

	// Start is called before the first frame update
	void Start() {
		hp = max_hp;
	}

	public void DecrementHp(float dmg) {
		float dealt = dmg * Time.deltaTime;
		hp -= dealt;
	}

	// Update is called once per frame
	void Update() {
		if(hp <= 0) {
			Die();
		}
	}
	
	void Die() {
		GameObject particles = Instantiate(particlePrefab, transform.position, Quaternion.identity);
		Destroy(gameObject);
		Destroy(particles, 2f);
	}
}
