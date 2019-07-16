using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolSelect : MonoBehaviour
{
    // Start is called before the first frame update
    public Selectable[] tools;
    public Vector3[] scaling;
    public Scrollbar sb;

    private float timeRemaining;

    private Selectable sel;

    void Start()
    {
        timeRemaining = 0;
        sb.size = 1f / tools.Length;
        sb.numberOfSteps = tools.Length;
    }
    public void DisplayGUI(Selectable s)
    {
        sb.transform.position = Camera.main.WorldToScreenPoint(s.transform.position + new Vector3(0, Vector3.Magnitude(s.transform.lossyScale), 0));
        timeRemaining = 3;
        sb.gameObject.SetActive(true);
        sb.value = (float)s.GetCurrentTool() / (sb.numberOfSteps - 1);
        sel = s;
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            sb.gameObject.SetActive(false);
            sel = null;
        }
        if (sel)
        {
            sb.transform.position = Camera.main.WorldToScreenPoint(sel.transform.position + new Vector3(0, Vector3.Magnitude(sel.transform.lossyScale), 0));
        }
    }
}
