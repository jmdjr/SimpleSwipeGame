using UnityEngine;
using System.Collections;

public class Director : MonoBehaviour {
    private GameObject Shapes = null;
    private Vector3 startPos = Vector3.zero;
    private Vector3 endPos = Vector3.zero;
    private Vector3 ulitmateVelocity = Vector3.zero;

    //[SerializeField]
    private int maxMagnitude = 200;
    private float scaleMagnitude = 0.25f;
    private LineRenderer lineRender = null;
	// Use this for initialization

	void Start () {
        Shapes = GameObject.Find("/Shapes");

        lineRender = Shapes.AddComponent<LineRenderer>();
        lineRender.SetWidth(5f, 5f);
        lineRender.material = new Material(Shader.Find("Particles/Additive"));
        lineRender.SetVertexCount(2);
        lineRender.useWorldSpace = true;
	}
	
	// Update is called once per frame
	void Update () {

        // when mouse is first clicked
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startPos.z = 0;
            lineRender.SetPosition(0, startPos);
            lineRender.enabled = true;
        }
        
        // when mouse is let go
        if (Input.GetMouseButtonUp(0))
        {
            if (Shapes != null)
            {
                GameObject newShape = new GameObject("shape", typeof(PolygonTester), typeof(ShapeRunner));
                ShapeRunner runner = newShape.GetComponent<ShapeRunner>();

                if (runner != null)
                {
                    runner.velocity = ulitmateVelocity;
                    Debug.Log(ulitmateVelocity.magnitude);
                }

                newShape.transform.position = endPos;
                newShape.transform.SetParent(Shapes.transform);
            }
            lineRender.enabled = false;
        }

        // while mouse has the button held down
        if (Input.GetMouseButton(0))
        {
            
            endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endPos.z = 0;
            endPos -= startPos;
            

            if (endPos.magnitude >= maxMagnitude)
            {
                endPos.Normalize();
                endPos *= maxMagnitude;
            }

            ulitmateVelocity = endPos;
            
            float mag = ulitmateVelocity.magnitude;

            ulitmateVelocity.Normalize();

            ulitmateVelocity *= mag * scaleMagnitude;

            endPos += startPos;
            lineRender.SetPosition(1, endPos);
        }
	}
}
