using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour {
	public GameObject particlePrefab;
	public LevelManager lm;
	private Transform initialTransform;
	private Quaternion initialRot;
	public bool toRespawn;

	public void Start() {
		initialTransform = transform;
	}
	
	public void kill() {
		if(toRespawn) {
			Respawn.RespawnEnemy(gameObject);
		}
		GameObject particles = Instantiate(particlePrefab, transform.position, Quaternion.identity);
		lm.decrementEnemies();
		Destroy(gameObject);
		Destroy(particles, 2f);
	}
}
