using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpticsSelector : MonoBehaviour
{
	public static OpticsSelector Instance {
		get;
		private set;
	}

	public GameObject[] tools;

	private Vector3 mouse_pos;
	private Vector3 object_pos;
	private float angle;
	private GameObject selected;

	private void reset() {
		foreach(GameObject obj in tools) {
			obj.SetActive(false);
		}
	}

	public void EnableTools(GameObject[] tools) {
		reset();
		foreach(GameObject obj in tools) {
			obj.SetActive(true);
		}
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


    // Update is called once per frame
    void Update()
    {
		if(Input.GetMouseButton(0)) {
			Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if(GameManager.Instance) {
				pz.x = Mathf.Clamp(pz.x, -GameManager.Instance.WorldWidth / 2, GameManager.Instance.WorldWidth / 2);
				pz.y = Mathf.Clamp(pz.y, -GameManager.Instance.WorldHeight / 2, GameManager.Instance.WorldHeight / 2);
			}
			pz.z = 0;
			if(selected) {
				selected.transform.position = pz;
			}
		}
		if(Input.GetMouseButton(1)) {
			mouse_pos = Input.mousePosition;
			mouse_pos.z = -20;
			object_pos = Camera.main.WorldToScreenPoint(selected.transform.position);
			mouse_pos.x = mouse_pos.x - object_pos.x;
			mouse_pos.y = mouse_pos.y - object_pos.y;
			angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
			if(selected) {
				selected.transform.rotation = Quaternion.Euler(0, 0, angle);
			}
		}
	}

	public void selectTool(GameObject obj) {
		selected = obj;
	}
}
