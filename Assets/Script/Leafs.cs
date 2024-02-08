using System;
using UnityEngine;

public class Leafs : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Mesh mesh;

    private float density = 1;
    private int densityPowerTen = 1;

    private float totalSurface;
    private float[] surfaceTriangle;

    private Vector3[] points;

    private Vector3[] vertices;
    int[] triangles;

    int nbPoints = 0;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            this.enabled = false;
            Debug.LogError("MeshFilter not found");
            return;
        }

        mesh = meshFilter.mesh;
        triangles = mesh.triangles;
        vertices = mesh.vertices;

        CalculateTotalAndTriangleSurface();
    }

    private void CalculatePointsOnSurface(int startingIndex)
    {
        nbPoints = CalculateTotalPoints();
        points = new Vector3[nbPoints];
        Vector3 newPoint;

        for (int index = 0; index < nbPoints; index++)
        {
            int indexTriangle = FindRandomTriangle();
            newPoint = GenerateRandomPointOnTriangle(indexTriangle);
            points[index] = newPoint;
        }
    }


    private int FindRandomTriangle()
    {

        if (surfaceTriangle.Length == 0)
        {
            throw new Exception("Triangle Array Empty");

        }
        float randomSurface = Random.Range(0f, totalSurface);
        int indexTriangle = -1;
        float area = 0;
        int maxIndexTriangle = surfaceTriangle.Length;
        while (area <= randomSurface && maxIndexTriangle != indexTriangle)
        {
            indexTriangle++;
            area += surfaceTriangle[indexTriangle];
        }
        return indexTriangle;
    }

    private Vector3 GenerateRandomPointOnTriangle(int indexTriangle)
    {
        Vector3 u = verticies[triangle[indexTriangle]];
        Vector3 v = verticies[triangle[indexTriangle + 1]];
        Vector3 w = verticies[triangle[indexTriangle + 2]];

        Vector3 scalaire = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)).normalized;

        return (u * scalaire.x) + (v * scalaire.y) + (w * scalaire.z);
    }

    private void CalculateTotalAndTriangleSurface()
    {

        int surfaceNumber = triangles.Length / 3;

        surfaceTriangle = new float[surfaceNumber];

        foreach (var triangleIndex in Enumerable.Range(0, surfaceNumber))
        {
            Vector3 triangleUn = vertices[triangles[triangleIndex * 3]];
            Vector3 triangleDeux = vertices[triangles[triangleIndex * 3 + 1]];
            Vector3 triangleTrois = vertices[triangles[triangleIndex * 3 + 2]];

            float surface = GetTriangleArea(triangleUn, triangleDeux, triangleTrois);
            totalSurface += surface;
            surfaceTriangle[triangleIndex] = surface;
        }
    }

    private int CalculateTotalPoints()
    {
        float result = (float)(density * totalSurface * Mathf.Pow(10, densityPowerTen));
        return Mathf.RoundToInt(result);
    }

    private float GetTriangleArea(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 ab = b - a;
        Vector3 ac = c - a;
        return 0.5f * Vector3.Cross(ab, ac).magnitude;
    }
}
