using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleGndToWorld : MonoBehaviour {
	// Start is called before the first frame update
	void Start() {
		transform.localScale = GameManager.Instance.GroundScale();
		transform.position = GameManager.Instance.GroundOffset();
	}
}
