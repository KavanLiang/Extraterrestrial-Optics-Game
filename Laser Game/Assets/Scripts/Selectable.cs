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

    void Start()
    {
        prevAlpha = 0.60f;
        selected = false;
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
        } else {
            if(selected) {
                Vector3 mouse_pos = Input.mousePosition;
                mouse_pos.z = 0;
                Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
                mouse_pos.x = mouse_pos.x - object_pos.x;
                mouse_pos.y = mouse_pos.y - object_pos.y;
                float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
                if(!Laser.isShooting()){
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }
            }
        }
    }

    void OnMouseOver()
    {
        if(Input.GetMouseButton(1) && !existSelected) {
            selected = true;
            existSelected = true;
        }
    }

    void OnMouseDrag()
    {
        if(!Laser.isShooting()) {
            Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pz.x = Mathf.Clamp(pz.x, InteractableArea.bounds.min.x, InteractableArea.bounds.max.x);
            pz.y = Mathf.Clamp(pz.y, InteractableArea.bounds.min.y, InteractableArea.bounds.max.y);
            pz.z = 0;
            transform.position = pz;
        }
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
