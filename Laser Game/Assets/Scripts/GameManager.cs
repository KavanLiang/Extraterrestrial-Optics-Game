using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public static GameManager Instance {
		get;
		private set;
	}

	public float MaxHealth;
	public float WorldHeight;
	public float WorldWidth;
	public int NumLanes;
	public float slowFactor = 6f;

	private float playerHealth;
	private int score;
	public int ScoreThreshold;
	private int thresholdFactor;
	private bool gameEnded;

	public Text promptText;
	public Image promptImage;

	public Queue<string> promptQueue;
	public Animator animator;
	public LevelHandler levelHandler;

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

	public void EndGame() {
		StartCoroutine(RestartLevel());
	}

	public void displayPrompt(Prompt prompt) {
		animator.SetBool("IsOpen", true);
		promptQueue.Clear();
		promptImage.sprite = prompt.image;
		promptImage.enabled = prompt.hasImage;
		foreach(string sentence in prompt.pText) {
			promptQueue.Enqueue(sentence);
		}
		DisplayNextPrompt();
	}

	public void DisplayNextPrompt() {
		Debug.Log("NEXT");
		if(promptQueue.Count <= 0) {
			EndPrompt();
			return;
		}
		promptText.text = promptQueue.Dequeue();
	}

	public void Update() {
		if(score > ScoreThreshold * thresholdFactor) {
			levelHandler.advanceLevel();
			thresholdFactor++;
		}
	}

	void EndPrompt() {
		animator.SetBool("IsOpen", false);
	}

	private IEnumerator RestartLevel() {
		Time.timeScale = 1f / slowFactor;
		Time.fixedDeltaTime = Time.fixedDeltaTime / slowFactor;
		yield return new WaitForSeconds(3f / slowFactor);
		Time.timeScale = 1f;
		Time.fixedDeltaTime = Time.fixedDeltaTime * slowFactor;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		reset();
	}

	private void reset() {
		WorldHeight = (float)Camera.main.orthographicSize * 2;
		WorldWidth = WorldHeight / Screen.height * Screen.width;
		playerHealth = MaxHealth;
		score = 0;
		promptQueue = new Queue<string>();
		thresholdFactor = 1;
	}

	private void Awake() {
		if(Instance == null) {
			Instance = this;
			reset();
			DontDestroyOnLoad(this.gameObject);
		} else {
			Destroy(this.gameObject);
		}
	}
}
