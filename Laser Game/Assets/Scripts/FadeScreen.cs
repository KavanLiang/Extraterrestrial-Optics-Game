using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
	Image img;

    // Update is called once per frame
    void Update()
    {
        
    }

	void fade() {
		if(GameManager.Instance.GetPlayerHealth() <= 0) {

		}
	}

	IEnumerator fadeToBlack() {

	}
}
