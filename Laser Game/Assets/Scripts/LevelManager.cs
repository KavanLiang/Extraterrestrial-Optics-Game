using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public int TotalEnemies;
    public GameObject winUI;
    private int numEnemies;

    public Button cont;
    
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
            if(!cont) {
                LevelComplete();
            } else {
                cont.interactable = true;
                cont.onClick.AddListener(() => LevelComplete());
            }
        }
    }

    void LevelComplete()
    {
        winUI.SetActive(true);
    }
}
