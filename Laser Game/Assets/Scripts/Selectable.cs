using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour {
	void OnMouseDown() {
		OpticsSelector.Instance.selectTool(this.gameObject);
	}
}
