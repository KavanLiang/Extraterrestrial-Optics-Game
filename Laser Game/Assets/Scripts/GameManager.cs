using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: end the game when player hp goes to 0
public class GameManager : MonoBehaviour {
	public static GameManager Instance {
		get;
		private set;
	}

	public float MaxHealth;
	public float WorldHeight;
	public float WorldWidth;
	public int NumLanes;

	private float playerHealth;
	private int score;


	public Vector3 GroundScale() {
		return new Vector3(WorldWidth, (2 * WorldHeight / 3), 1);
	}

	public Vector3 GroundOffset() {
		return new Vector3(0, 0, 10);
	}

	public float ClampToLane(int y) {
		return (GroundOffset().y) + (GroundScale().y / 2) - (y * (GroundScale().y) / NumLanes);
	}

	public float GetPlayerHealth() {
		return playerHealth;
	}

	public void DecrementPlayerHealth(float dmg) {
		playerHealth -= dmg;
	}

	public void IncrementScore(int amnt) {
		score += amnt;
	}

	public int GetScore() {
		return score;
	}					   

	private void Awake() {
		if(Instance == null) {
			Instance = this;
			WorldHeight = (float) Camera.main.orthographicSize * 2;
			WorldWidth = WorldHeight / Screen.height * Screen.width;
			playerHealth = MaxHealth;
			score = 0;
			DontDestroyOnLoad(this.gameObject);
		} else {
			Destroy(this.gameObject);
		}
	}
}
