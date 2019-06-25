using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererTest : MonoBehaviour
{
    public Renderer rend;

    // Update is called once per frame
    void start()
    {
        Debug.Log(rend.bounds.max);
        Debug.Log(rend.bounds.min);
    }
}
