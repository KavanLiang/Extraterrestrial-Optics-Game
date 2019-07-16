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
        refIndex = las.GetRefIndex();
        direction = las.transform.right;
        laser = las;
        sr.color = las.GetColor();
        tr.startColor = las.GetColor();
        tr.endColor = las.GetColor();
        currVertex = 0;
        traversePath = las.GetVerticies();
    }

    void FixedUpdate()
    {
        if (currVertex < traversePath.Length)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (traversePath[currVertex] - transform.position).normalized, out hit, offset, ~(1 << 8), QueryTriggerInteraction.UseGlobal))
            {
                rb.MovePosition(hit.point);
                string hitTag = hit.transform.gameObject.tag;
                if (hitTag.Equals(Laser.enemyTag))
                {
                    hit.transform.GetComponentInParent<AlienController>().kill();
                }
            }
            if (Vector3.Distance(traversePath[currVertex], transform.position) > epsilon)
            {
                rb.MovePosition(Vector3.MoveTowards(transform.position, traversePath[currVertex], speed * Time.deltaTime));
            }
            else
            {
                currVertex++;
            }
        }
        else
        {
            removeProjectile();
        }
    }
    void OnBecameInvisible()
    {
        if(this.gameObject.activeInHierarchy){
            StartCoroutine(removeOnExit());
        }
    }

    IEnumerator removeOnExit()
    {
        yield return new WaitForSeconds(2f);
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
