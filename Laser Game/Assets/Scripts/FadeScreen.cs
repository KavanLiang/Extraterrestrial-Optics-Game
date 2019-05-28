using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
	public Image img;
	private bool end;

	void Start() {
		end = false;
	}

    // Update is called once per frame
    void Update()
    {
		fade();
    }

	void fade() {
		if(GameManager.Instance.GetPlayerHealth() <= 0) {
			StartCoroutine(fadeToBlack());
			if(!end) {
				GameManager.Instance.EndGame();
				end = true;
			}
		}
	}

	IEnumerator fadeToBlack() {
		for(int i = 0; i < 100; i++) {
			img.color = Color.Lerp(img.color, Color.black, (float)i / 100);
			yield return null;
		}
	}
}
