using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mirror2 : MonoBehaviour
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
        if (Input.GetMouseButton(0) && mirrorControll.mirror2)
        {
			Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			pz.x = Mathf.Clamp(pz.x, -GameManager.Instance.WorldWidth / 2, GameManager.Instance.WorldWidth / 2);
			pz.y = Mathf.Clamp(pz.y, -GameManager.Instance.GroundScale().y / 2, GameManager.Instance.GroundScale().y / 2);
			pz.z = 0;
            gameObject.transform.position = pz;

        }
        if (Input.GetMouseButton(1) && mirrorControll.mirror2)
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
        mirrorControll.mirror2 = true;
        mirrorControll.sprism1 = false;
        mirrorControll.concaveMirror = false;
        mirrorControll.convexMirror = false;
    }
}
