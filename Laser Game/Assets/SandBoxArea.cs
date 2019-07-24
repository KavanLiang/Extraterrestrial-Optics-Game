using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandBoxArea : MonoBehaviour
{
    static bool selectionEnabled = false;
    GameObject currTool;
    void Start()
    {
        currTool = null;
    }

    public static void EnableSelection(bool enable) {
        selectionEnabled = enable;
    }

    void OnMouseOver() {
        if(selectionEnabled) {
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.PingPong(Time.time, 0.4f));
        }
    }

    void OnMouseDown() {
        if(!currTool) {
            if(SandboxToolPanel.activeTool) {
                currTool = Instantiate(SandboxToolPanel.activeTool, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                currTool.GetComponent<Selectable>().InteractableArea = GetComponent<SpriteRenderer>();
            }
        } else {
            if(SandboxToolPanel.activeTool) {
                Destroy(currTool);
                currTool = Instantiate(SandboxToolPanel.activeTool, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                currTool.GetComponent<Selectable>().InteractableArea = GetComponent<SpriteRenderer>();
            }
        }
    }

    void OnMouseExit() {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
    }
}
