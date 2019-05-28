using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mirror : MonoBehaviour
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
        if (Input.GetMouseButton(0))
        {
            Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pz.z = 0;
            gameObject.transform.position = pz;
        }
        if (Input.GetMouseButton(1))
        {
            /*Vector3 pz = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            Vector3 rot = new Vector3(0f, 0f, (float) (Vector3.MoveTowards(pz,transform.position,0.01f).magnitude));
            transform.Rotate(rot * 50f);*/
            mouse_pos = Input.mousePosition;
            mouse_pos.z = -20;
            object_pos = Camera.main.WorldToScreenPoint(transform.position);
            mouse_pos.x = mouse_pos.x - object_pos.x;
            mouse_pos.y = mouse_pos.y - object_pos.y;
            angle = Mathf.Atan2(-mouse_pos.x, mouse_pos.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
