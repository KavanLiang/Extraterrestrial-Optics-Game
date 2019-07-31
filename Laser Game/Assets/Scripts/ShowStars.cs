using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStars : MonoBehaviour
{
    public GameObject[] stars;
    public EnergyController enc;
    public float delay;
    void Awake()
    {
        StartCoroutine(show());
    }

    IEnumerator show() {
        for(int i = 0; i <= Mathf.Min(stars.Length, enc.remainingEnergy() + 1); i++) {
            yield return new WaitForSeconds(delay);
            stars[i].SetActive(true);
        }
    }
}
