using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverTextBody : MonoBehaviour
{
    public string label;
    public TextBody[] text;

    public void OnMouseEnter() {
        HoverTextPanel.setHoverObject(this.gameObject);
    }

    public void OnMouseExit(){
        HoverTextPanel.clear();
    }
}
