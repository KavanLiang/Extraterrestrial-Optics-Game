﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{

    public string nextLevel;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(loadNext());
    }

    private IEnumerator loadNext() {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(nextLevel);
    }
}