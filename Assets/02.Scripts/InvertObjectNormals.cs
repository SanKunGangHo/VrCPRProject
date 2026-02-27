using UnityEngine;
using System.Collections;

public class InvertObjectNormals : MonoBehaviour 
{
    public GameObject SferaPanoramica;

    void Awake()
    {
        InvertSphere();
    }

    void InvertSphere()
    {
        Vector3[] normals = SferaPanoramica.GetComponent<MeshFilter>().sharedMesh.normals;
        for(int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }
        SferaPanoramica.GetComponent<MeshFilter>().sharedMesh.normals = normals;

        int[] triangles = SferaPanoramica.GetComponent<MeshFilter>().sharedMesh.triangles;
        for (int i = 0; i < triangles.Length; i+=3)
        {
            (triangles[i], triangles[i + 2]) = (triangles[i + 2], triangles[i]);
        }           

        SferaPanoramica.GetComponent<MeshFilter>().sharedMesh.triangles= triangles;
    }
}