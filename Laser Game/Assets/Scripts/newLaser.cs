using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class newLaser : MonoBehaviour
{
    public int laserDistance = 100;
    public string bounceTag = "mirror";
    public string prismTag = "squarePrism";
    public float refIndex = 1.5f;
    public string enemyTag;
    public int maxBounce = 10;
    private LineRenderer mLineRenderer;
    public float dmg;

    // Use this for initialization
    void Start()
    {
        mLineRenderer = gameObject.GetComponent<LineRenderer>();
        StartCoroutine(RedrawLaser());
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(RedrawLaser());
    }

    IEnumerator RedrawLaser()
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

                mLineRenderer.SetWidth(.2f, .2f);
                lastLaserPosition = hit.point;
                Vector3 prevDirection = laserDirection;
                laserDirection = Vector3.Reflect(laserDirection, hit.normal);


            }

            else if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance) && ((hit.transform.gameObject.tag == prismTag)) )
            {
                Debug.Log("refract");
                vertexCounter++;
                mLineRenderer.SetVertexCount(vertexCounter);
                mLineRenderer.SetPosition(vertexCounter - 1, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));

                mLineRenderer.SetWidth(.2f, .2f);
                lastLaserPosition = hit.point;

                Debug.Log(hit.point);

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

                //else if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance) && ((hit.transform.gameObject.tag == prismTag)))
                //{
                

                float testDistance = 0f;
                while (true)
                {
                    Debug.Log("loop");
                    if(Physics.Linecast((lastLaserPosition + testDistance * laserDirection),lastLaserPosition))
                    {
                        Debug.Log("success");
                        break;
                    }
                    else
                    {
                        testDistance = testDistance + 1f;
                    }
                    if (testDistance > 100f)
                    {
                        Debug.Log("force break");
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
                Debug.Log("refract end");
                vertexCounter++;
                mLineRenderer.SetVertexCount(vertexCounter);
                mLineRenderer.SetPosition(vertexCounter - 1, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));

                mLineRenderer.SetWidth(.2f, .2f);
                lastLaserPosition = hit.point;

                Debug.Log(hit.point);

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
                Debug.Log("in medium");

                vertexCounter++;
                mLineRenderer.SetVertexCount(vertexCounter);
                mLineRenderer.SetPosition(vertexCounter - 1, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));

                mLineRenderer.SetWidth(.2f, .2f);
                lastLaserPosition = hit.point;
            }

            else if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance))
            {
                vertexCounter++;
                mLineRenderer.SetVertexCount(vertexCounter);
                mLineRenderer.SetPosition(vertexCounter - 1, hit.point);
                mLineRenderer.SetWidth(.2f, .2f);
                loopActive = false;
                if (hit.transform.gameObject.tag == enemyTag)
                {
                    hit.transform.gameObject.GetComponent<AlienController>().DecrementHp(dmg);
                }
            }

            else
            {
                laserReflected++;
                vertexCounter++;
                mLineRenderer.SetVertexCount(vertexCounter);
                Vector3 lastPos = lastLaserPosition + (laserDirection.normalized * laserDistance);
                mLineRenderer.SetPosition(vertexCounter - 1, lastLaserPosition + (laserDirection.normalized * laserDistance));
                mLineRenderer.SetWidth(.2f, .2f);
                loopActive = false;
            }
            if (laserReflected > maxBounce)
            {
                loopActive = false;
            }
        }

        yield return new WaitForEndOfFrame();
    }
}