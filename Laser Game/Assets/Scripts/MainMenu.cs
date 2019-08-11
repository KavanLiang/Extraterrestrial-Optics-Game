using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	public void PlayGame() {
		Selectable.resetInteractable();
		SceneManager.LoadScene("Level Select");
	}
}
