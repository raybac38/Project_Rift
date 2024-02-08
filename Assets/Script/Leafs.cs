using System.Linq;
using UnityEngine;

public class Leafs : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Mesh mesh;

    private double density = 1;
    private int densityPowerTen = 1;

    private double totalSurface;
    private double[] surfaceTriangle;

    private void Awake() 
    {
    
        meshFilter = GetComponent<MeshFilter>();
        if(meshFilter == null)
        {
            this.enabled = false;
            Debug.LogError("MeshFilter not found");
            return;
        }
        mesh = meshFilter.mesh;
        CalculateTotalAndTriangleSurface();

    }

    private void CalculatePointsOnSurface(int startingIndex)
    {
        
    }


    private Vector3 GenerateRandomPointOnTriangle(int indexTriangle)
    {

    }

    private void CalculateTotalAndTriangleSurface()
    {
        var triangles = mesh.triangles;
        var vertices = mesh.vertices;
        int surfaceNumber = triangles.Length / 3;

        surfaceTriangle = new double[surfaceNumber];

        foreach (var triangleIndex in Enumerable.Range(0, surfaceNumber))
        {
            Vector3 triangleUn = vertices[triangles[triangleIndex * 3]]; 
            Vector3 triangleDeux = vertices[triangles[triangleIndex * 3 + 1]]; 
            Vector3 triangleTrois = vertices[triangles[triangleIndex * 3 + 2]]; 
            
            double surface = GetTriangleArea(triangleUn, triangleDeux, triangleTrois);
            totalSurface += surface;
            surfaceTriangle[triangleIndex] = surface;
        }

        Debug.Log("Total surface: " + totalSurface);
        Debug.Log("Total points: " + CalculateTotalPoints());
    }

    private int CalculateTotalPoints()
    {
        double result = density * totalSurface * Mathf.Pow(10, densityPowerTen);
        return Mathf.RoundToInt((float)result);
    }   

    private double GetTriangleArea(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 ab = b - a;
        Vector3 ac = c - a;
        return 0.5 * Vector3.Cross(ab, ac).magnitude;
    }
}
