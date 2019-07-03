using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveThisAfterClick : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) || Input.GetMouseButton(1)){
            Destroy(this.gameObject);
        }
    }
}
