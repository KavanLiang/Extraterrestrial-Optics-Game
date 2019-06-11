
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    public float updateFrequency = 0.1f;
    public int laserDistance;
    public string bounceTag;
    public string prismTag;
    public string enemyTag;
    public int maxBounce;
    private LineRenderer mLineRenderer;
    public float dmg;
    private bool inMedium = false;

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

            else if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance) && ((hit.transform.gameObject.tag == prismTag)) && !inMedium)
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
                float refAngle = incAngle / 1.5f;
                if (Vector3.Angle(Quaternion.Euler(0, 0, (refAngle - incAngle)) * laserDirection, -1.0f * (hit.normal)) < incAngle)
                {
                    laserDirection = Quaternion.Euler(0, 0, (refAngle - incAngle)) * laserDirection;
                }
                else
                {
                    laserDirection = Quaternion.Euler(0, 0, -(refAngle - incAngle)) * laserDirection;
                }
                inMedium = !inMedium;
                Debug.Log(inMedium + " " + refAngle);
            }

            else if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance) && ((hit.transform.gameObject.tag == prismTag)) && inMedium)
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
                float refAngle = incAngle * 1.5f;
                if (refAngle >= 90)
                {
                    laserDirection = Vector3.Reflect(laserDirection, hit.normal);
                }
                else if (Vector3.Angle(Quaternion.Euler(0, 0, (refAngle - incAngle)) * laserDirection, -1.0f * (hit.normal)) > incAngle)
                {
                    laserDirection = Quaternion.Euler(0, 0, (refAngle - incAngle)) * laserDirection;
                }
                else
                {
                    laserDirection = Quaternion.Euler(0, 0, -(refAngle - incAngle)) * laserDirection;
                }
                inMedium = !inMedium;
                Debug.Log(inMedium +" "+ refAngle);
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