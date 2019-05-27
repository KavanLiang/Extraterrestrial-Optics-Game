using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerHandler : MonoBehaviour {
    void Update() {
		//TODO: Tweak this after we decide on spawning methods
		this.gameObject.layer = (int) Mathf.Clamp(this.transform.position.y / GameManager.Instance.WorldHeight, 0, GameManager.Instance.NumLanes);
    }
}
