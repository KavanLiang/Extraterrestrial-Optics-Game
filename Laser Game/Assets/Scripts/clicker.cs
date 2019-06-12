using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clicker : MonoBehaviour
{
    private Vector3 mouse_pos;
    private Vector3 object_pos;
    private float angle;
    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && mirrorControll.sprism1)
        {
            Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pz.z = 0;
            gameObject.transform.position = pz;

        }
        else if (Input.GetMouseButton(1) && mirrorControll.sprism1)
        {
            mouse_pos = Input.mousePosition;
            mouse_pos.z = -20;
            object_pos = Camera.main.WorldToScreenPoint(transform.position);
            mouse_pos.x = mouse_pos.x - object_pos.x;
            mouse_pos.y = mouse_pos.y - object_pos.y;
            angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void OnMouseDown()
    {
        mirrorControll.mirror1 = false;
        mirrorControll.mirror2 = false;
        mirrorControll.sprism1 = true;
        mirrorControll.concaveMirror = false;
        mirrorControll.convexMirror = false;
    }
}
