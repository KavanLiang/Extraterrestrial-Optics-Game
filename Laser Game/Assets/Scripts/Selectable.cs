using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public Renderer InteractableArea;
    public Renderer rend;
    private float prevAlpha;
    public float rotation_speed;
    private bool selected;
    private static bool existSelected = false;
    private Vector3 prevPosition;

    private static GameObject myLine;
    

    void Start()
    {
        prevAlpha = 0.60f;
        selected = false;
        prevPosition = new Vector3(0, 0, 0);
        if(!myLine) {
            myLine = new GameObject();
            myLine.AddComponent<LineRenderer>();
        }
        rend.sortingOrder = 1;
    }

    void OnMouseEnter()
    {
        toggleSelected();
    }

    void OnMouseExit()
    {
        toggleSelected();
    }

    void FixedUpdate() {
        if(!Input.GetMouseButton(1)) {
            selected = false;
            existSelected = false;
            myLine.SetActive(false);
        } else {
            if(selected && !Input.GetMouseButton(0)) {
                Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouse_pos.z = 0;
                DrawLine(prevPosition, mouse_pos, new Color(1f, 1f, 1f, 0.2f));
                mouse_pos.x = mouse_pos.x - prevPosition.x;
                mouse_pos.y = mouse_pos.y - prevPosition.y;
                float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
                if(!Laser.isShooting()){
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }
            }
        }
        if(Laser.isShooting()) {
            myLine.SetActive(false);
        }
    }

    void OnMouseOver()
    {
        if(Input.GetMouseButton(1) && !existSelected) {
            selected = true;
            existSelected = true;
            prevPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            prevPosition.z = 0;
        }
    }

    void OnMouseDrag()
    {
        if(!Laser.isShooting()) {
            myLine.SetActive(false);
            Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pz.x = Mathf.Clamp(pz.x, InteractableArea.bounds.min.x, InteractableArea.bounds.max.x);
            pz.y = Mathf.Clamp(pz.y, InteractableArea.bounds.min.y, InteractableArea.bounds.max.y);
            pz.z = 0;
            transform.position = pz;
        }
    }

    void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        myLine.transform.position = start;
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
        lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        myLine.SetActive(true);
    }

    public void toggleSelected()
    {
        Color col = rend.material.color;
        float temp = col.a;
        col.a = prevAlpha;
        prevAlpha = temp;
        rend.material.color = col;
    }
}
