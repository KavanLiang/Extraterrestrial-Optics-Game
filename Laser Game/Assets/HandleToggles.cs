using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HandleToggles : MonoBehaviour
{
    // Start is called before the first frame update
    public string[] levelNames;
    public Toggle[] toggles;
    bool loadingLevel;
    public Sprite[] levelPreviews;
    public Image levelPreview;
    int current;
    void Start()
    {
        for(int i = 0; i < levelNames.Length; i++) {
            toggles[i].GetComponentInChildren<Text>().text = levelNames[i];
        }
        current = 0;
        levelPreview.sprite = levelPreviews[current];
        loadingLevel = false;
    }

    public void StartLevel() {
        if(!loadingLevel){
            loadingLevel = true;
            StartCoroutine(loadNext());
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < toggles.Length; i++) {
            if(toggles[i].isOn) {
                current = i;
                levelPreview.sprite = levelPreviews[current];
            }
        }
    }

    private IEnumerator loadNext() {
        yield return new WaitForSeconds(0.2f);
        AsyncOperation async = SceneManager.LoadSceneAsync(levelNames[current]);
        while(!async.isDone) {
            yield return null;
        }
    }
}
