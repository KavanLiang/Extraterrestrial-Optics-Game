using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancePrompt : MonoBehaviour {
	void OnMouseDown() {
		GameManager.Instance.DisplayNextPrompt();
	}
}
