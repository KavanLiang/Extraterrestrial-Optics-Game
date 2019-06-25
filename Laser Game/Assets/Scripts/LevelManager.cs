using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int TotalEnemies;
    public GameObject winUI;
    private int numEnemies;
    
    // Start is called before the first frame update
    void Start()
    {
        numEnemies = TotalEnemies;
    }

    public void decrementEnemies(){
        numEnemies -= 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (numEnemies <= 0)
        {
            LevelComplete();
        }
    }

    void LevelComplete()
    {
        winUI.SetActive(true);
    }
}
