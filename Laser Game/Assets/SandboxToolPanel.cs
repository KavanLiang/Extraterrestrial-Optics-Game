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
    public Image SelectedToolPane;

    void Start()
    {
        for(int i = 0; i < prefabs.Length; i++) {
            int closure = i;
            buttons[i].GetComponent<Image>().sprite = GetSprite(prefabs[i]);
            buttons[i].onClick.AddListener(() => Select(closure));
        }    
        activeTool = null;
    }

    void Select(int i) {
        activeTool = prefabs[i];
    }

    public void Deselect() {
        activeTool = null;
    }

    Sprite GetSprite(GameObject obj) {
        Texture2D previewTexture = AssetPreview.GetAssetPreview(obj);
        return Sprite.Create(previewTexture, new Rect(0.0f, 0.0f, previewTexture.width, previewTexture.height), new Vector2(0.5f, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        if(activeTool) {
            SelectedToolPane.sprite = GetSprite(activeTool);
            SelectedToolPane.gameObject.SetActive(true);
            SandBoxArea.EnableSelection(true);
            Selectable.ToggleInteractable(false);
        } else {
            SelectedToolPane.gameObject.SetActive(false);
            SandBoxArea.EnableSelection(false);
            Selectable.ToggleInteractable(true);
        }
    }
}
