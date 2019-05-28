using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour {
	// Start is called before the first frame update
	public Slider slider;

	void Start() {
		slider.maxValue = GameManager.Instance.MaxHealth;
		slider.minValue = 0;
		slider.SetValueWithoutNotify(GameManager.Instance.MaxHealth);
	}

	// Update is called once per frame
	void Update() {
		slider.value = GameManager.Instance.GetPlayerHealth();
	}
}
