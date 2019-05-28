using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
	public Image img;

    // Update is called once per frame
    void Update()
    {
		fade();
    }

	void fade() {
		if(GameManager.Instance.GetPlayerHealth() <= 0) {
			StartCoroutine(fadeToBlack());
		}
	}

	IEnumerator fadeToBlack() {
		for(int i = 0; i < 100; i++) {
			img.color = Color.Lerp(img.color, Color.black, (float)i / 100);
			yield return null;
		}
	}
}
