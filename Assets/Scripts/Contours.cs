using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]

public struct Circle
{
    public Vector3[] points;
    public GameObject[] pointObjects;
    public int height;
}

public class Contours : MonoBehaviour
{
    public int segments;
    public int minRadius;
    public int radiusDiff;
    public int heightConstant = 8;
    public int numberOfCircles = 4;

    public string pointObjectName = "Point";

    public GameObject pointObject;
    public GameObject empty;
    public Circle[] circles;
    public Vector3[] allPoints;

    public Mesh mesh;

    private void Start()
    {
        allPoints = new Vector3[segments * numberOfCircles + 1];
        
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = mesh = new Mesh();

        circles = new Circle[numberOfCircles];
        float currentRadius = minRadius;
        for (int i = 0; i < numberOfCircles; i++)//init and population control
        {
            GameObject parent = new GameObject("Circle" + (i + 1));
            circles[i].points = new Vector3[segments];
            circles[i].pointObjects = new GameObject[segments];
            circles[i].height = heightConstant * (numberOfCircles-i-1);
            circles[i].points = MakeCircle(currentRadius, parent, circles[i].height, i);
            currentRadius += radiusDiff;
        }

    }

    public Vector3[] MakeCircle(float r, GameObject parent, int height, int j)
    {
        Vector3[] pts = new Vector3[segments];
        GameObject[] ptsObject = new GameObject[segments];
        float angleStep = 2 * Mathf.PI / segments;
        for (int i = 0; i < segments; i++)
        {
            Vector3 position = new Vector3(r * Mathf.Cos(i * angleStep), height, r * Mathf.Sin(i * angleStep));
            pts[i] = position;
            GameObject thisPointObject = Instantiate(pointObject);
            thisPointObject.transform.position = position;
            thisPointObject.transform.parent = parent.transform;
            circles[j].pointObjects[i] = thisPointObject;
        }
        return pts;
    }
    private void Update()
    {
        //Update points and update allPoints
        int counter = 0;
        for (int k = 0; k < numberOfCircles; k++)
        {
            for (int l = 0; l < segments; l++)
            {
                circles[k].points[l] = circles[k].pointObjects[l].transform.position;
                allPoints[counter] = circles[k].points[l];
                counter++;
            }
        }
        CalculateMesh();
    }

    public void CalculateMesh()
    {
        mesh.vertices = allPoints;
        mesh.triangles = CalculateTriangles();
        mesh.RecalculateNormals();
    }

    int[] CalculateTriangles()
    {
        int[] triangles = new int[(numberOfCircles - 1) * segments * 6];

        int pointsToBeCalc = segments * (numberOfCircles - 1);
        for (int i = 0, j = 0 ; i < pointsToBeCalc; i++, j+=6)
        {
            triangles[j] = i;
            triangles[j + 1] = i + segments + 1;
            triangles[j + 2] = i + segments;
            triangles[j + 3] = i;
            triangles[j + 4] = i + 1;
            triangles[j + 5] = i + segments + 1;
        }
        return triangles;
    }

}
