using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contour : MonoBehaviour {
    public int segments;
    public int minRadius;
    public int maxRadius;

    public int numberOfCircles = 4;

    public string pointObjectName = "Point";

    public GameObject pointObject;
    private void Awake()
    {
        float rStep = maxRadius - minRadius / numberOfCircles;
        int indexCounter = 0;
        for (int r = 1; r <= numberOfCircles; r++)
        {
            CreateCircle(r * rStep, indexCounter);
            indexCounter++;
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void CreateCircle(float r, int index)
    {
        GameObject parent = Instantiate(new GameObject());
        List<Vector3> points = SetPoints(r);
        CreatePointsAndGO(points, parent);

    }
    List<Vector3> SetPoints(float r)
    {
        List<Vector3> pts = new List<Vector3>();
        float angleStep = 2 * Mathf.PI / segments;
        for(int i = 0; i < segments; r++)
        {
            pts.Add(new Vector3(r * Mathf.Cos(i * angleStep), 0, r * Mathf.Sin(i * angleStep)));
        }
        return pts;
    }

    void CreatePointsAndGO(List<Vector3> pointsList, GameObject parent)
    {
        for (int i = 0; i < segments; i++)
        {
            GameObject thisPoint = Instantiate(pointObject, parent.transform);
            thisPoint.name = pointObjectName;
            thisPoint.transform.position = pointsList.ToArray()[i];
        }
    }
}
