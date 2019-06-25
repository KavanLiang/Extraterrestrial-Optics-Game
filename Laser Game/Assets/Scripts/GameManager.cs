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

	public float MaxEnergy;
	public float slowFactor = 6f;

	private float playerEnergy;
	private int thresholdFactor;
	private bool gameEnded;

	public Text promptText;
	public Image promptImage;

	public Queue<string> promptQueue;
	public Animator animator;
	public LevelHandler levelHandler;

	public float GetPlayerEnergy() {
		return playerEnergy;
	}

	public void DecrementPlayerEnergy(float dmg) {
		playerEnergy -= dmg;
	}

	public void EndGame() {
		StartCoroutine(RestartLevel());
	}

    // public void displayPrompt(Prompt prompt) {
    // 	animator.SetBool("IsOpen", true);
    // 	promptQueue.Clear();
    // 	promptImage.sprite = prompt.image;
    // 	promptImage.enabled = prompt.hasImage;
    // 	foreach(string sentence in prompt.pText) {
    // 		promptQueue.Enqueue(sentence);
    // 	}
    // 	DisplayNextPrompt();
    // }

    // public void DisplayNextPrompt() {
    // 	Debug.Log("NEXT");
    // 	if(promptQueue.Count <= 0) {
    // 		EndPrompt();
    // 		return;
    // 	}
    // 	promptText.text = promptQueue.Dequeue();
    // }

    // void EndPrompt() {
    // 	animator.SetBool("IsOpen", false);
    // 	levelHandler.ToggleSpawn();
    // }

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
		playerEnergy = MaxEnergy;
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
