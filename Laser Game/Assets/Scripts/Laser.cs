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
    public float laserWidth;
    public int maxBounce = 10;
    private LineRenderer mLineRenderer;
    public float dmg;
    private Color traceColor = new Color(5, 5, 3, 0);
    private Color laserColor = new Color(2.2f, 10, 0.8f);

    private int numActiveProjectiles;

    // Use this for initialization

    void Start()
    {
        mLineRenderer = gameObject.GetComponent<LineRenderer>();
        RedrawLaser();
        numActiveProjectiles = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space)) {
            shoot();
        }
        if (numActiveProjectiles <= 0)
        {
            mLineRenderer.enabled = true;
            RedrawLaser();
        } else {
            mLineRenderer.enabled = false;
        }
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
    }

    void RedrawLaser()
    {
        int laserReflected = 1; //How many times it got reflected
        int vertexCounter = 1; //How many line segments are there
        bool loopActive = true; //Is the reflecting loop active?

        Vector3 laserDirection = transform.right; //direction of the next laser
        Vector3 lastLaserPosition = transform.localPosition; //origin of the next laser

        mLineRenderer.SetVertexCount(1);
        mLineRenderer.SetPosition(0, transform.position);
        RaycastHit hit;

        while (loopActive)
        {
            if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance) && ((hit.transform.gameObject.tag == bounceTag)))
            {
                laserReflected++;
                vertexCounter++;
                mLineRenderer.SetVertexCount(vertexCounter);
                mLineRenderer.SetPosition(vertexCounter - 1, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));

                mLineRenderer.SetWidth(laserWidth, laserWidth);
                mLineRenderer.material.color = traceColor;
                lastLaserPosition = hit.point;
                Vector3 prevDirection = laserDirection;
                laserDirection = Vector3.Reflect(laserDirection, hit.normal);


            }

            else if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance) && ((hit.transform.gameObject.tag == mediumTag)))
            {
                vertexCounter++;
                mLineRenderer.SetVertexCount(vertexCounter);
                mLineRenderer.SetPosition(vertexCounter - 1, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));

                mLineRenderer.SetWidth(.2f, .2f);
                lastLaserPosition = hit.point;


                Vector3 prevDirection = laserDirection;
                float incAngle = Vector3.Angle(prevDirection, -1.0f * (hit.normal));
                if (incAngle > 90f)
                {
                    incAngle = incAngle - 90;
                }
                float refAngle = incAngle / refIndex;
                if (Vector3.Angle(Quaternion.Euler(0, 0, (refAngle - incAngle)) * laserDirection, -1.0f * (hit.normal)) < incAngle)
                {
                    laserDirection = Quaternion.Euler(0, 0, (refAngle - incAngle)) * laserDirection;
                }
                else
                {
                    laserDirection = Quaternion.Euler(0, 0, -(refAngle - incAngle)) * laserDirection;
                }
                //}

                //else if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance) && ((hit.transform.gameObject.tag == mediumTag)))
                //{


                float testDistance = 0f;
                while (true)
                {
                    if (Physics.Linecast((lastLaserPosition + testDistance * laserDirection), lastLaserPosition))
                    {
                        break;
                    }
                    else
                    {
                        testDistance = testDistance + 1f;
                    }
                    if (testDistance > 100f)
                    {
                        break;
                    }
                }

                //Physics.Linecast((lastLaserPosition + testDistance * laserDirection), lastLaserPosition);

                Physics.Raycast((lastLaserPosition + testDistance * laserDirection), -1f * laserDirection, out hit, laserDistance);

                /*
                if (Physics.Linecast(lastLaserPosition, (lastLaserPosition + 100f * laserDirection)))
                {
                    Debug.Log("success");
                }
                */
                vertexCounter++;
                mLineRenderer.SetVertexCount(vertexCounter);
                mLineRenderer.SetPosition(vertexCounter - 1, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));

                mLineRenderer.SetWidth(.2f, .2f);
                lastLaserPosition = hit.point;


                prevDirection = laserDirection;
                incAngle = Vector3.Angle(prevDirection, hit.normal);
                if (incAngle > 90f)
                {
                    incAngle = incAngle - 90;
                }
                refAngle = incAngle * refIndex;
                if (refAngle >= 90)
                {
                    laserDirection = Vector3.Reflect(laserDirection, -1f * hit.normal);
                }
                else if (Vector3.Angle(Quaternion.Euler(0, 0, (refAngle - incAngle)) * laserDirection, hit.normal) > incAngle)
                {
                    laserDirection = Quaternion.Euler(0, 0, (refAngle - incAngle)) * laserDirection;
                }
                else
                {
                    laserDirection = Quaternion.Euler(0, 0, -(refAngle - incAngle)) * laserDirection;
                }
            }

            else if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance) && hit.transform.gameObject.tag == "medium")
            {

                vertexCounter++;
                mLineRenderer.SetVertexCount(vertexCounter);
                mLineRenderer.SetPosition(vertexCounter - 1, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));

                mLineRenderer.SetWidth(laserWidth, laserWidth);
                lastLaserPosition = hit.point;
            }

            else if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance) && hit.transform.gameObject.tag == "wall")
            {
                vertexCounter++;
                mLineRenderer.SetVertexCount(vertexCounter);
                mLineRenderer.SetPosition(vertexCounter - 1, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));

                mLineRenderer.SetWidth(laserWidth, laserWidth);
                lastLaserPosition = hit.point;
                loopActive = false;
            }

            else if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance))
            {
                vertexCounter++;
                mLineRenderer.SetVertexCount(vertexCounter);
                mLineRenderer.SetPosition(vertexCounter - 1, hit.point);
                mLineRenderer.SetWidth(laserWidth, laserWidth);
                loopActive = false;
            }

            else
            {
                laserReflected++;
                vertexCounter++;
                mLineRenderer.SetVertexCount(vertexCounter);
                Vector3 lastPos = lastLaserPosition + (laserDirection.normalized * laserDistance);
                mLineRenderer.SetPosition(vertexCounter - 1, lastLaserPosition + (laserDirection.normalized * laserDistance));
                mLineRenderer.SetWidth(laserWidth, laserWidth);
                loopActive = false;
            }
            if (laserReflected > maxBounce)
            {
                loopActive = false;
            }
        }
    }
}