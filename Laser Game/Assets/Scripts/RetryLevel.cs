using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryLevel : MonoBehaviour
{
    // Start is called before the first frame update
    public void retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
