using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public Renderer InteractableArea;
    public Renderer rend;
    private float prevAlpha;
    public float rotation_speed;

    void Start()
    {
        prevAlpha = 0.60f;
    }

    void OnMouseEnter()
    {
        toggleSelected();
    }

    void OnMouseExit()
    {
        toggleSelected();
    }

    void OnMouseOver()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            float delta = Input.mouseScrollDelta.y;
            float angle = transform.rotation.eulerAngles.z + delta * Time.deltaTime * rotation_speed;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void OnMouseDrag()
    {
        Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pz.x = Mathf.Clamp(pz.x, InteractableArea.bounds.min.x, InteractableArea.bounds.max.x);
        pz.y = Mathf.Clamp(pz.y, InteractableArea.bounds.min.y, InteractableArea.bounds.max.y);
        pz.z = 0;
        transform.position = pz;
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
