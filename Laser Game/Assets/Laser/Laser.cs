
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    public float updateFrequency = 0.1f;
    public int laserDistance;
    public string bounceTag;
    public int maxBounce;
    private LineRenderer mLineRenderer;

    // Use this for initialization
    void Start()
    {
        mLineRenderer = gameObject.GetComponent<LineRenderer>();
        StartCoroutine(RedrawLaser());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator RedrawLaser()
    {
        int laserSplit = 1; //How many times it got split
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
            Debug.Log(Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance));
            if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance) && ((hit.transform.gameObject.tag == bounceTag) ))
            {
                Debug.Log("Bounce");
                laserReflected++;
                vertexCounter ++;
                mLineRenderer.SetVertexCount(vertexCounter);
                mLineRenderer.SetPosition(vertexCounter - 1, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));

                mLineRenderer.SetWidth(.1f, .1f);
                lastLaserPosition = hit.point;
                Vector3 prevDirection = laserDirection;
                laserDirection = Vector3.Reflect(laserDirection, hit.normal);


            }
            else if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance))
            {
                Debug.Log("Absorb");
                vertexCounter++;
                mLineRenderer.SetVertexCount(vertexCounter);
                mLineRenderer.SetPosition(vertexCounter - 1, hit.point);

                loopActive = false;
            }
            else
            {
                Debug.Log("No Bounce");
                laserReflected++;
                vertexCounter++;
                mLineRenderer.SetVertexCount(vertexCounter);
                Vector3 lastPos = lastLaserPosition + (laserDirection.normalized * laserDistance);
                mLineRenderer.SetPosition(vertexCounter - 1, lastLaserPosition + (laserDirection.normalized * laserDistance));

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