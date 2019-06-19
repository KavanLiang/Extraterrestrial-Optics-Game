using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class LevelHandler : MonoBehaviour {
	// Start is called before the first frame update
	public GameObject[] enemies;
	private float elapsedTime;
	public float WallSpawnFactor;
	public Level[] levels;
	private int currentLevel;
	private int si;
	private int ee;

	void Start() {
		elapsedTime = 0;
		currentLevel = 0;
		advanceLevel();
		//coroutine = SpawnFromFile(someFileNameHere);
	}

	// Update is called once per frame
	void Update() {
		//StartCoroutine(coroutine);
		elapsedTime += Time.deltaTime;
		if(elapsedTime > si) {
			elapsedTime = 0;
			SpawnRandom(UnityEngine.Random.Range(0, ee));
		}
	}

	private void TriggerPrompt(Prompt p) {
		GameManager.Instance.displayPrompt(p);
	}

	public void advanceLevel() {
		if(currentLevel < levels.Length) {
			Level next = levels[currentLevel];
			OpticsSelector.Instance.EnableTools(next.enabledTools);
			TriggerPrompt(next.prompt);
			si = levels[currentLevel].SpawnInterval;
			ee = levels[currentLevel].EnabledEnemies - 1;
			currentLevel++;
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
		SpawnInLane(UnityEngine.Random.Range(0, GameManager.Instance.NumLanes), enemies[level]);
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
