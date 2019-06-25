using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingEffect : MonoBehaviour
{
    public float bobStr;
    private Vector3 origin;
    
    void Start() {
        origin = transform.position;
    }
    void Update()
    {
        transform.position = origin + new Vector3(Mathf.Cos(3 * Time.time) * bobStr * Time.deltaTime, Mathf.Sin(3 * Time.time) * bobStr * Time.deltaTime, 0);
    }
}
