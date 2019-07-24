using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class SandboxToolPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] prefabs;
    public Button[] buttons;
    public static GameObject activeTool;
    int activeToolIndex;
    public Image SelectedToolPane;

    public Sprite[] PrefabPreviews;
    public Sprite MoveSelected;

    void Start()
    {
        for(int i = 0; i < prefabs.Length; i++) {
            int closure = i;
            buttons[i].GetComponent<Image>().sprite = PrefabPreviews[i];
            buttons[i].onClick.AddListener(() => Select(closure));
        }    
        activeTool = null;
        activeToolIndex = -1;
    }

    void Select(int i) {
        activeTool = prefabs[i];
        activeToolIndex = i;
    }

    public void Deselect() {
        activeTool = null;
        activeToolIndex = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if(activeTool) {
            SelectedToolPane.sprite = PrefabPreviews[activeToolIndex];
            SelectedToolPane.gameObject.SetActive(true);
            SandBoxArea.EnableSelection(true);
            Selectable.ToggleInteractable(false);
        } else {
            SelectedToolPane.sprite = MoveSelected;
            SandBoxArea.EnableSelection(false);
            Selectable.ToggleInteractable(true);
        }
    }
}
