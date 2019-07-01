using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyController : MonoBehaviour {
	// Start is called before the first frame update
	public const int MAX_VAL = 5;
	private int currEnergy;
	public Slider slider;

	public int remainingEnergy() {
		return currEnergy;
	}

	public void useEnergy() {
		if(currEnergy > 0) {
			currEnergy -= 1;
		}
		this.slider.SetValueWithoutNotify(currEnergy);
	}

	void Start() {
		this.slider.maxValue = MAX_VAL;
		this.currEnergy = MAX_VAL;
	}
}
