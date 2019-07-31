using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour {
	public GameObject particlePrefab;
	public LevelManager lm;
	private Transform initialTransform;
	private Quaternion initialRot;
	public bool toRespawn;
	private bool spawning;

	public void Start() {
		initialTransform = transform;
		spawning = false;
		if(toRespawn) {
			lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		}
	}
	
	public void kill() {
		if(toRespawn && !spawning) {
			Respawn.RespawnEnemy(gameObject);
			spawning = true;
		}
		GameObject particles = Instantiate(particlePrefab, transform.position, Quaternion.identity);
		lm.decrementEnemies();
		Destroy(gameObject);
		Destroy(particles, 2f);
	}
}
