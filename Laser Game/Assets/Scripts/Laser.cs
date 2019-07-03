using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    public int laserDistance = 100;
    public static string bounceTag = "mirror";
    public static string mediumTag = "squarePrism";

    public static string enemyTag = "enemyTag";

    public static string wallTag = "wall";

    public GameObject proj;
    public float refIndex = 1.5f;
    private LineRenderer mLineRenderer;
    public Color LaserColor;
    public AudioSource audio;

    private static int numActiveProjectiles = 0;
    public int MaxCollisions;

    public float offset;

    void Start()
    {
        mLineRenderer = gameObject.GetComponent<LineRenderer>();
        DrawLaser();
    }

    public Color GetColor()
    {
        return LaserColor;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (numActiveProjectiles <= 0)
        {
            mLineRenderer.enabled = true;
            DrawLaser();
        }
        else
        {
            mLineRenderer.enabled = false;
        }
    }

    public static bool isShooting()
    {
        return numActiveProjectiles > 0;
    }
    public void DecrementActiveProjectiles()
    {
        numActiveProjectiles--;
    }

    public void shoot()
    {
        GameObject lp = Instantiate(proj, this.transform.position, transform.rotation);
        lp.GetComponent<LaserProjectile>().SetLaserProperties(this);
        numActiveProjectiles++;
        audio.Play();
    }

    public Vector3[] GetVerticies()
    {
        Vector3[] ret = new Vector3[mLineRenderer.positionCount];
        mLineRenderer.GetPositions(ret);
        return ret;
    }

    void DrawLaser()
    {
        int vertexCounter = 0;
        mLineRenderer.positionCount = 1;
        mLineRenderer.SetPosition(0, transform.position);
        mLineRenderer.material.color = LaserColor;
        mLineRenderer.SetColors(LaserColor, LaserColor);
        Vector3 direction = transform.right;
        bool inMedium = false;
        RaycastHit forward;
        RaycastHit backward;
        float traverseMediumDist = 0;
        float maxMedDist = 0;
        while (vertexCounter < MaxCollisions)
        {
            if (!inMedium)
            {
                if (Physics.Raycast(mLineRenderer.GetPosition(vertexCounter), direction, out forward, laserDistance))
                {
                    Debug.DrawLine(forward.point, direction);
                    mLineRenderer.positionCount++;
                    vertexCounter++;
                    mLineRenderer.SetPosition(vertexCounter, forward.point);
                    Debug.DrawRay(mLineRenderer.GetPosition(vertexCounter), forward.normal, Color.green, 0);
                    string hitTag = forward.transform.gameObject.tag;
                    if (hitTag.Equals(Laser.bounceTag))
                    {
                        direction = Vector3.Reflect(direction, forward.normal);
                    }
                    else if (hitTag.Equals(Laser.mediumTag))
                    {
                        float incAngle = Vector3.Angle(direction, -1.0f * (forward.normal));
                        if (incAngle > 90f)
                        {
                            incAngle = incAngle - 90;
                        }
                        float snell = Mathf.Sin(Mathf.Deg2Rad * incAngle) / refIndex;
                        float refAngle;
                        if (snell <= 1)
                        {
                            refAngle = Mathf.Asin(snell) * Mathf.Rad2Deg;
                            if (refAngle > 90)
                            {
                                refAngle -= 90;
                            }
                            if (Vector3.Angle(Quaternion.Euler(0, 0, (refAngle - incAngle)) * direction, -forward.normal) < incAngle)
                            {
                                direction = Quaternion.Euler(0, 0, refAngle) * -forward.normal;
                            }
                            else
                            {
                                direction = Quaternion.Euler(0, 0, -refAngle) * -forward.normal;
                            }
                        }
                        inMedium = true;
                        maxMedDist = Mathf.Sqrt(Mathf.Pow(forward.collider.bounds.extents.x, 2) + Mathf.Pow(forward.collider.bounds.extents.y, 2)) * 1.1f;
                        traverseMediumDist = 0;
                        mLineRenderer.positionCount++;
                        vertexCounter++;
                        mLineRenderer.SetPosition(vertexCounter, mLineRenderer.GetPosition(vertexCounter - 1) + direction.normalized * offset);
                    }
                    else if (hitTag.Equals(wallTag))
                    {
                        break;
                    }
                    else
                    {
                        mLineRenderer.positionCount++;
                        vertexCounter++;
                        mLineRenderer.SetPosition(vertexCounter, mLineRenderer.GetPosition(vertexCounter - 1) + direction.normalized * offset);
                    }
                }
                else
                {
                    mLineRenderer.positionCount++;
                    vertexCounter++;
                    mLineRenderer.SetPosition(vertexCounter, mLineRenderer.GetPosition(vertexCounter - 1) + direction.normalized * laserDistance);
                }
            }
            else
            {
                if (Physics.Raycast(mLineRenderer.GetPosition(vertexCounter) + direction.normalized * offset, -direction, out backward, offset))
                {
                    if (backward.transform.gameObject.tag.Equals(mediumTag))
                    {
                        traverseMediumDist = 0;
                        mLineRenderer.positionCount++;
                        vertexCounter++;
                        mLineRenderer.SetPosition(vertexCounter, backward.point);
                        Debug.DrawRay(mLineRenderer.GetPosition(vertexCounter), backward.normal, Color.green, 0);
                        float incAngle = Vector3.Angle(direction, backward.normal);
                        if (incAngle > 90f)
                        {
                            incAngle = incAngle - 90;
                        }
                        float snell = Mathf.Sin(Mathf.Deg2Rad * incAngle) * refIndex;
                        if (snell > 1)
                        {
                            Debug.DrawRay(mLineRenderer.GetPosition(vertexCounter), direction);
                            direction = Vector3.Reflect(direction, -1f * backward.normal);
                            mLineRenderer.positionCount++;
                            vertexCounter++;
                            mLineRenderer.SetPosition(vertexCounter, mLineRenderer.GetPosition(vertexCounter - 1) + direction.normalized * offset);
                        }
                        else
                        {
                            float refAngle = Mathf.Asin(snell) * Mathf.Rad2Deg;
                            if (refAngle > 90)
                            {
                                refAngle -= 90;
                            }
                            if (Vector3.Angle(Quaternion.Euler(0, 0, (refAngle - incAngle)) * direction, (backward.normal)) < incAngle)
                            {
                                direction = Quaternion.Euler(0, 0, -refAngle) * backward.normal;
                            }
                            else
                            {
                                direction = Quaternion.Euler(0, 0, refAngle) * backward.normal;
                            }
                            inMedium = false;
                        }
                    }
                    else
                    {
                        if (traverseMediumDist < maxMedDist)
                        {
                            mLineRenderer.SetPosition(vertexCounter, mLineRenderer.GetPosition(vertexCounter) + direction.normalized * offset);
                            traverseMediumDist += offset;
                        }
                        else
                        {
                            inMedium = false;
                        }
                    }
                }
                else
                {
                    if (traverseMediumDist < maxMedDist)
                    {
                        mLineRenderer.SetPosition(vertexCounter, mLineRenderer.GetPosition(vertexCounter) + direction.normalized * offset);
                        traverseMediumDist += offset;
                    }
                    else
                    {
                        inMedium = false;
                    }
                }
            }
        }
    }
}