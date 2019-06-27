using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour {
	public GameObject particlePrefab;
	public LevelManager lm;

	// Start is called before the first frame update

	// Update is called once per frame
	
	public void kill() {
		GameObject particles = Instantiate(particlePrefab, transform.position, Quaternion.identity);
		lm.decrementEnemies();
		Destroy(gameObject);
		Destroy(particles, 2f);
	}
}
