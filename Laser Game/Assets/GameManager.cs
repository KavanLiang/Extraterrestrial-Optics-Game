using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager Instance {
		get;
		private set;
	}

	public float WorldHeight;
	public float WorldWidth;
	public int NumLanes;


	public Vector3 GrassScale() {
		return new Vector3(WorldWidth, (2 * WorldHeight / 3), 1);
	}

	public Vector3 GrassOffset() {
		return new Vector3(0, WorldHeight / 3, -10);
	}

	public float ClampToLane(int y) {
		return (GrassOffset().y) - (GrassScale().y / 2) - (y * (GrassScale().y) / NumLanes);
	}

	private void Awake() {
		if(Instance == null) {
			Instance = this;
			WorldHeight = (float) Camera.main.orthographicSize * 2;
			WorldWidth = WorldHeight / Screen.height * Screen.width;
			DontDestroyOnLoad(this.gameObject);
		} else {
			Destroy(this.gameObject);
		}
	}

}
