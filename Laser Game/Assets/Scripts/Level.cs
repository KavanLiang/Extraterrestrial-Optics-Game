using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Level {
	public GameObject[] enabledTools;
	public int SpawnInterval;
	public int EnabledEnemies;
	public Prompt prompt;
}
