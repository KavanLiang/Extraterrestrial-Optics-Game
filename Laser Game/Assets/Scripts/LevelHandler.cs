using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class LevelHandler : MonoBehaviour {
	// Start is called before the first frame update
	public int SpawnInterval;
	public GameObject[] enemies;
	private float elapsedTime;
	public float WallSpawnFactor;
	public int level;
	public Prompt[] prompts;
	private int currentPrompt;

	//IEnumerator coroutine;

	void Start() {
		elapsedTime = 0;
		currentPrompt = 0;
		advanceLevel();
		//coroutine = SpawnFromFile(someFileNameHere);
	}

	// Update is called once per frame
	void Update() {
		//StartCoroutine(coroutine);
		elapsedTime += Time.deltaTime;
		if(elapsedTime > SpawnInterval) {
			elapsedTime = 0;
			SpawnRandom(level);
		}
	}

	private void TriggerPrompt(Prompt p) {
		GameManager.Instance.displayPrompt(p);
	}

	void advanceLevel() {
		if(currentPrompt < prompts.Length) {
			TriggerPrompt(prompts[currentPrompt++]);
		}
	}

	/// <summary>
	/// Spawn the given enemy in the lane
	/// </summary>
	/// <param name="lane">The lane to spawn the enemy in.</param>
	/// <param name="enemy">The prefab pertaining to the enemy to be spawned</param>
	void SpawnInLane(int lane, GameObject enemy) {
		Instantiate(enemy, new Vector3(GameManager.Instance.WorldWidth / 2, GameManager.Instance.ClampToLane(lane) * WallSpawnFactor, 0), Quaternion.Euler(0, 0, 90));
	}

	/// <summary>
	/// Spawns a basic enemy in some random lane.
	///
	/// For infinite levels.
	/// </summary>
	void SpawnRandom(int level) {
		SpawnInLane(UnityEngine.Random.Range(0, GameManager.Instance.NumLanes), enemies[(int) Mathf.Clamp(level, 0, enemies.Length - 1)]);
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
	/// e.g. 00000 would spawn a single basicEnemy in the top row.
	/// </summary>
	/// <param name="line">A string of integers that corresponds to the next wave of enemies</param>
	public void SpawnHelper(string line) {
		for(int i = 0; i < line.Length; i++) {
			if(Char.IsDigit(line[i])) {
				SpawnInLane(i, enemies[line[i] - '0']);
			}
		}
	}
}
