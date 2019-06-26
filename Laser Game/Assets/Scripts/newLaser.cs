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
    public float laserWidth;
    public string enemyTag;
    public int maxBounce = 10;
    private LineRenderer mLineRenderer;
    private float dmg;
    private Color traceColor = new Color(5, 5, 3, 0);
    private Color laserColor = new Color(2.2f, 10, 0.8f);

    private bool startShooting = false;

    // variables for the shooting
    private Vector3 lastShotPos;
    private float distanceCounter = 0f;
    private int shotVertexCounter = 0;
    private LineRenderer shooting;
    public float shotLength = 3f;
    private float distanceRemain;
    private Vector3 lastShotDirection;
    public float laserSpeed = 0.1f;

    // Use this for initialization
    void Start()
    {
        mLineRenderer = gameObject.GetComponent<LineRenderer>();
        StartCoroutine(RedrawLaser());
    }

    // Update is called once per frame
    void Update()
    {
        ///Shooting is controlled by the bool "startShooting"
        ///It's modified by pressing space right now
        ///You can link it to other static bool (press the "start" in game)

        if (Input.GetKeyDown("space"))
        {
            distanceCounter = 0f;
            shotVertexCounter = 0;
            startShooting = !startShooting;
        }
        if (!startShooting)
        {
            StartCoroutine(RedrawLaser());
        }
        else
        {
            shot();
        }
    }

    void shot()
    {


        int laserReflected = 1;
        int vertexCounter = 1;
        bool loopActive = true;

        Vector3 laserDirection = transform.right;
        Vector3 lastLaserPosition = transform.localPosition;

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

            else if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance) && ((hit.transform.gameObject.tag == prismTag)))
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


                Physics.Raycast((lastLaserPosition + testDistance * laserDirection), -1f * laserDirection, out hit, laserDistance);

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

            else if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance))
            {
                vertexCounter++;
                mLineRenderer.SetVertexCount(vertexCounter);
                mLineRenderer.SetPosition(vertexCounter - 1, hit.point);
                mLineRenderer.SetWidth(laserWidth, laserWidth);
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
                mLineRenderer.SetWidth(laserWidth, laserWidth);
                loopActive = false;
            }
            if (laserReflected > maxBounce)
            {
                loopActive = false;
            }
        }

        mLineRenderer.material.color = Color.yellow;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (distanceCounter == 0)
        {
            lastShotPos = mLineRenderer.GetPosition(0);
            lastShotDirection = Vector3.Normalize(mLineRenderer.GetPosition(1) - mLineRenderer.GetPosition(0));
        }
        if (distanceCounter < shotLength)
        {
            distanceCounter += laserSpeed;
        }
        distanceRemain = distanceCounter;

        if (distanceCounter >= shotLength)
        {
            if (Mathf.Abs((lastShotPos).x - mLineRenderer.GetPosition(shotVertexCounter + 1).x) > laserSpeed || Mathf.Abs((lastShotPos).y - mLineRenderer.GetPosition(shotVertexCounter + 1).y) > laserSpeed)
            {
                mLineRenderer.SetPosition(0, lastShotPos + laserSpeed * Vector3.Normalize(mLineRenderer.GetPosition(shotVertexCounter + 1) - mLineRenderer.GetPosition(shotVertexCounter)));
                lastShotPos = mLineRenderer.GetPosition(0);
            }
            else
            {

                mLineRenderer.SetPosition(0, lastShotPos + laserSpeed * Vector3.Normalize(mLineRenderer.GetPosition(shotVertexCounter + 1) - mLineRenderer.GetPosition(shotVertexCounter)));
                lastShotPos = mLineRenderer.GetPosition(0);
                lastShotDirection = Vector3.Normalize(mLineRenderer.GetPosition(shotVertexCounter + 1) - mLineRenderer.GetPosition(shotVertexCounter));
                shotVertexCounter++;
            }
        }


        float accumulator = 0;
        int temp = 1;
        int vertexAcc = 2;
        while (accumulator <= distanceRemain && shotVertexCounter < (mLineRenderer.positionCount - 2))
        {
            if (accumulator == 0)
            {
                accumulator += Vector3.Distance(lastShotPos, mLineRenderer.GetPosition(shotVertexCounter + 1));
            }
            else
            {
                accumulator += Vector3.Distance(mLineRenderer.GetPosition(shotVertexCounter + temp), mLineRenderer.GetPosition(shotVertexCounter + temp + 1));
                vertexAcc++;
                temp++;
            }
        }

        mLineRenderer.SetVertexCount(20);

        for (int i = 1; i < vertexAcc; i++)
        {
            mLineRenderer.SetPosition(i, mLineRenderer.GetPosition(shotVertexCounter + i));
        }


        mLineRenderer.SetVertexCount(vertexAcc);

        if (vertexAcc > 2)
        {
            if (vertexAcc > 3)
            {
                Debug.Log("star ----- positionCount:  " + mLineRenderer.positionCount + "   vertexAcc: " + vertexAcc);

                for (int i = 1; i <= vertexAcc; i++)
                {
                    Debug.Log(mLineRenderer.GetPosition(i - 1));
                }
            }

            for (int i = 1; i < (vertexAcc - 1); i++)
            {
                distanceRemain -= Vector3.Distance(mLineRenderer.GetPosition(i - 1), mLineRenderer.GetPosition(i));
            }
        }
        mLineRenderer.SetPosition(vertexAcc - 1, mLineRenderer.GetPosition(vertexAcc - 2) + distanceRemain * Vector3.Normalize(mLineRenderer.GetPosition(vertexAcc - 1) - mLineRenderer.GetPosition(vertexAcc - 2)));

    }

    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator RedrawLaser()
    {
        int laserReflected = 1;
        int vertexCounter = 1;
        bool loopActive = true;

        Vector3 laserDirection = transform.right;
        Vector3 lastLaserPosition = transform.localPosition;

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

            else if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance) && ((hit.transform.gameObject.tag == prismTag)))
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

                Physics.Raycast((lastLaserPosition + testDistance * laserDirection), -1f * laserDirection, out hit, laserDistance);

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

            else if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance))
            {
                vertexCounter++;
                mLineRenderer.SetVertexCount(vertexCounter);
                mLineRenderer.SetPosition(vertexCounter - 1, hit.point);
                mLineRenderer.SetWidth(laserWidth, laserWidth);
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
                mLineRenderer.SetWidth(laserWidth, laserWidth);
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