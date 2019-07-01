using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    public float speed = 1f;
    private Vector3 direction = new Vector3(1, 0, 0);
    private float refIndex;
    bool hasDecremented;
    private float offset;
    private bool inMedium;
    private Laser laser;
    public Rigidbody rb;
    public BoxCollider collider;
    public TrailRenderer tr;
    public SpriteRenderer sr;
    private float maxMedDist;
    private float currMedDist;
    private Vector3[] traversePath;
    private int currVertex;
    public float epsilon;


    public void Start()
    {
        offset = speed * collider.bounds.extents.x + collider.bounds.extents.y;
        hasDecremented = false;
        inMedium = false;
        maxMedDist = 0;
        currMedDist = 0;
        currVertex = 0;
    }

    public void SetLaserProperties(Laser las)
    {
        refIndex = las.refIndex;
        direction = las.transform.right;
        laser = las;
        sr.color = las.GetColor();
        tr.startColor = las.GetColor();
        tr.endColor = las.GetColor();
        currVertex = 0;
        traversePath = las.GetVerticies();
    }

    void FixedUpdate() {
        if(currVertex < traversePath.Length) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (traversePath[currVertex] - transform.position).normalized, out hit, offset, ~(1 << 8), QueryTriggerInteraction.UseGlobal)) {
                rb.MovePosition(hit.point);
                string hitTag = hit.transform.gameObject.tag;
                if (hitTag.Equals(Laser.enemyTag))
                {
                    hit.transform.GetComponentInParent<AlienController>().kill();
                }
                else if (hitTag.Equals(Laser.wallTag))
                {
                    removeProjectile();
                    return;
                }
            }
            if(Vector3.Distance(traversePath[currVertex], transform.position) > epsilon) {
                rb.MovePosition(Vector3.MoveTowards(transform.position, traversePath[currVertex], speed * Time.deltaTime));
            } else {
                currVertex++;
            }
        } else {
            removeProjectile();
        }
    }

    // void FixedUpdate()
    // {
    //     RaycastHit hit;
    //     if (!inMedium)
    //     {
    //         if (Physics.Raycast(transform.position, direction, out hit, offset, ~(1 << 8), QueryTriggerInteraction.UseGlobal))
    //         {
    //             rb.MovePosition(hit.point);
    //             string hitTag = hit.transform.gameObject.tag;
    //             if (hitTag.Equals(Laser.enemyTag))
    //             {
    //                 hit.transform.GetComponentInParent<AlienController>().kill();
    //             }
    //             else if (hitTag.Equals(Laser.bounceTag))
    //             {
    //                 direction = Vector3.Reflect(direction, hit.normal);
    //             }
    //             else if (hitTag.Equals(Laser.wallTag))
    //             {
    //                 removeProjectile();
    //             }
    //             else if (hitTag.Equals(Laser.mediumTag))
    //             {
    //                 handleMediumEnter(hit);
    //                 rb.MovePosition(transform.position + offset * direction);
    //             }
    //             else
    //             {
    //                 removeProjectile();
    //             }
    //         }
    //         else
    //         {
    //             rb.MovePosition(transform.position + speed * direction.normalized * Time.deltaTime);
    //         }
    //     }
    //     else
    //     {
    //         if (Physics.Raycast(transform.position + direction.normalized * offset, -direction, out hit, offset))
    //         {
    //             string hitTag = hit.transform.gameObject.tag;
    //             if (hitTag.Equals(Laser.mediumTag))
    //             {
    //                 rb.MovePosition(hit.point);
    //                 handleMediumExit(hit);
    //             }
    //             else
    //             {
    //                 rb.MovePosition(transform.position + speed * direction.normalized * Time.deltaTime);
    //             }
    //             currMedDist += speed * Time.deltaTime;
    //         }
    //         else
    //         {
    //             rb.MovePosition(transform.position + speed * direction.normalized * Time.deltaTime);
    //             currMedDist += speed * Time.deltaTime;
    //         }
    //         if(currMedDist >= maxMedDist) {
    //             inMedium = false;
    //         }
    //     }
    // }

    // void handleMediumEnter(RaycastHit hit)
    // {
    //     float incAngle = Vector3.Angle(direction, -1.0f * (hit.normal));
    //     if (incAngle > 90f)
    //     {
    //         incAngle = incAngle - 90;
    //     }
    //     float snell = Mathf.Sin(Mathf.Deg2Rad * incAngle) / refIndex;
    //     float refAngle;
    //     if(snell <= 1) {
    //         refAngle = Mathf.Asin(snell) * Mathf.Rad2Deg;
    //         if (Vector3.Angle(Quaternion.Euler(0, 0, (refAngle - incAngle)) * direction, -hit.normal) < incAngle)
    //         {
    //             direction = Quaternion.Euler(0, 0, refAngle) * -hit.normal;
    //         }
    //         else
    //         {
    //             direction = Quaternion.Euler(0, 0, -refAngle) * -hit.normal;
    //         }
    //     }
    //     maxMedDist = Mathf.Sqrt(Mathf.Pow(hit.collider.bounds.extents.x, 2) + Mathf.Pow(hit.collider.bounds.extents.y, 2)) * 1.1f;
    //     inMedium = true;
    // }

    // void handleMediumExit(RaycastHit hit)
    // {
    //     float incAngle = Vector3.Angle(direction, hit.normal);
    //     if (incAngle > 90f)
    //     {
    //         incAngle = incAngle - 90;
    //     }
    //     float snell = Mathf.Sin(Mathf.Deg2Rad * incAngle) * refIndex;
    //     Debug.DrawRay(direction, hit.normal);
    //     if (snell > 1)
    //     {
    //         direction = Vector3.Reflect(direction, -1f * hit.normal);
    //         return;
    //     }
    //     else {
    //         float refAngle = Mathf.Asin(snell) * Mathf.Rad2Deg;
    //         if (Vector3.Angle(Quaternion.Euler(0, 0, (refAngle - incAngle)) * direction, hit.normal) < incAngle)
    //         {
    //             direction = Quaternion.Euler(0, 0, -refAngle) * hit.normal;
    //         }
    //         else
    //         {
    //             direction = Quaternion.Euler(0, 0, refAngle) * hit.normal;
    //         }
    //     }
    //     inMedium = false;
    // }

    void OnBecameInvisible()
    {
        removeProjectile();
    }

    void removeProjectile()
    {
        if (!hasDecremented)
        {
            laser.DecrementActiveProjectiles();
            hasDecremented = true;
        }
        Destroy(this.gameObject);
    }
}
