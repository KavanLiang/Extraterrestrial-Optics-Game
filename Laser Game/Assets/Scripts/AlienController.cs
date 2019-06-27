using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour {
	public GameObject particlePrefab;

	// Start is called before the first frame update

	// Update is called once per frame
	
	public void kill() {
		GameObject particles = Instantiate(particlePrefab, transform.position, Quaternion.identity);
		Destroy(gameObject);
		Destroy(particles, 2f);
	}
}
