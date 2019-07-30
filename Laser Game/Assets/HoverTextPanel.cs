using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HoverTextPanel : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI textbody;
    public TextMeshProUGUI pageDisp;

    static GameObject hovertext;
    static Vector3 htpos;
    static int selectedText;
    static bool canChange;

    // Start is called before the first frame update
    void Start()
    {
        canChange = true;
        hovertext = null;
        htpos = Vector3.zero;
        selectedText = 0;
    }

    public static void setHoverObject(GameObject ht) {
        if(canChange){
            hovertext = ht;
            selectedText = 0;
        }
    }

    public static void clear() {
        if(canChange) {
            hovertext = null;
            selectedText = 0;
        }
    }

    public void setModify(bool mod) {
        canChange = mod;
    }

    // Update is called once per frame
    void Update()
    {
        if(hovertext) {
            HoverTextBody ht = hovertext.GetComponent<HoverTextBody>();
            if(ht) {
                selectedText %= ht.text.Length; 
                transform.position = Camera.main.WorldToScreenPoint(htpos);
                title.text = ht.label;
                textbody.text = ht.text[selectedText].text;
                pageDisp.text = string.Format("{0}/{1}", selectedText + 1, ht.text.Length);
                transform.position = Camera.main.WorldToScreenPoint(hovertext.transform.position);
                GetComponent<CanvasGroup>().alpha = 1f;
                if(Input.GetKeyDown(KeyCode.Space)) {
                    selectedText++;
                }
            }
        } else {
            GetComponent<CanvasGroup>().alpha = 0f;
        }
    }
}
