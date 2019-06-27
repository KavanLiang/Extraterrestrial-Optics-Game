using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    public float speed = 1f;
    private Vector3 direction = new Vector3(1, 0, 0);
    public Rigidbody rb;


    public void Start()
    {
        rb.velocity = speed * direction.normalized;
    }

    public void SetDirection(Vector3 vec)
    {
        direction = vec;
    }

    public void SetRefractiveIndex() {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == Laser.enemyTag)
        {
            collision.gameObject.GetComponentInParent<AlienController>().kill();
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.tag == Laser.wallTag)
        {
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.tag == Laser.bounceTag)
        {
            Vector3 refl = Vector3.Reflect(direction, collision.contacts[0].normal);
            direction = refl;
            rb.velocity = speed * direction;
        }
        // else if (collision.gameObject.tag == Laser.prismTag) {
        //     float incAngle = Vector3.Angle(direction, collision.contacts[0].normal);
        //     if(incAngle > 90f) {
        //         incAngle = incAngle - 90;
        //     }
        //     float refAngle = incAngle / 
        // }
    }
}
