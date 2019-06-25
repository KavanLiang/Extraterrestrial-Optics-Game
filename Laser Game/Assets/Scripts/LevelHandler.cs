using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class LevelHandler : MonoBehaviour {
	// Start is called before the first frame update
	public GameObject[] enemies;
	private float elapsedTime;
	public Level[] levels;
	private int currentLevel;
	void Start() {
		elapsedTime = 0;
		currentLevel = 0;
		advanceLevel();
		//coroutine = SpawnFromFile(someFileNameHere);
	}

	// Update is called once per frame
	void Update() {
	}

	public void ToggleSpawn() {
	}

	private void TriggerPrompt(Prompt p) {
        // GameManager.Instance.displayPrompt(p);
	}

	public void advanceLevel() {

	}
}
