using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//TODO: decide on spawning locations and finish methods
public class SpawnEnemies : MonoBehaviour {
	// Start is called before the first frame update
	public int SpawnInterval;
	public GameObject[] enemies;

	//IEnumerator coroutine;

	void Start() {
		//coroutine = SpawnFromFile(someFileNameHere);
	}

	// Update is called once per frame
	void Update() {
		//StartCoroutine(coroutine);
		//SpawnRandom();
	}

	/// <summary>
	/// Spawn the given enemy in the lane
	/// </summary>
	/// <param name="lane">The lane to spawn the enemy in.</param>
	/// <param name="enemy">The prefab pertaining to the enemy to be spawned</param>
	void SpawnInLane(int lane, GameObject enemy) {
		//TODO
	}

	/// <summary>
	/// Spawns a basic enemy in some random lane.
	/// 
	/// For infinite levels.
	/// </summary>
	void SpawnRandom() {
		SpawnInLane(Random.Range(0, GameManager.Instance.NumLanes), enemies[0]);
	}

	/// <summary>
	/// Spawns enemies from a file
	/// </summary>
	/// <param name="path">The path pertaining to the enemy spawning file</param>
	IEnumerator SpawnFromFile(string path) {
		StreamReader reader = new StreamReader(path);
		string line;
		while((line = reader.ReadLine()) != null) {
			SpawnHelper(line);
			yield return null;
		}
	}

	/// <summary>
	/// Spawns a wave of enemies.
	/// 
	/// Each integer represents a unique enemy. 
	/// e.g. 10000 would spawn a single basicEnemy in the top row.
	/// </summary>
	/// <param name="line">A string of integers that corresponds to the next wave of enemies</param>
	public void SpawnHelper(string line) {
		for(int i = 0; i < line.Length; i++) {
			SpawnInLane(i, enemies[i]);
		}
	}
}
