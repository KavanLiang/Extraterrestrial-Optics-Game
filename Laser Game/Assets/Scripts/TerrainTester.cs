using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class TerrainTester : MonoBehaviour
{
	public Tilemap tm;
    // Update is called once per frame
    void Start()
    {
		tm.CompressBounds();
		Debug.Log(String.Format("Cell Bounds: {0}", tm.cellBounds));
		Debug.Log(String.Format("Origin: {0}", tm.origin));
		Debug.Log(String.Format("Anchors: {0}", tm.tileAnchor));
		Debug.Log(String.Format("Local To World: {0}", tm.LocalToWorld(new Vector3(0, 0, 0))));
	}
}
