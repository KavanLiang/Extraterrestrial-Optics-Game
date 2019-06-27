using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    public float speed = 1f;
    private Vector3 direction = new Vector3(1, 0, 0);
    private float refIndex;
    private bool flip;
    public Rigidbody rb;


    public void Start()
    {
        rb.velocity = speed * direction.normalized;
    }

    public void SetDirection(Vector3 vec)
    {
        direction = vec;
    }

    public void SetRefractiveIndex(float index) {
        refIndex = index;
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
    }
}
