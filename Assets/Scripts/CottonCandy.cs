using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CottonCandy : MonoBehaviour
{
    [SerializeField] private float growSpeed = 0.1f;
    MeshFilter meshFilter;
    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (meshFilter != null)
        {
            Mesh mesh = meshFilter.mesh;

            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;
            Vector3[] normals = mesh.normals;

            // Loop through all triangles
            for (int i = 0; i < triangles.Length; i += 3)
            {
                // Get the indices of the three vertices of the triangle
                int index1 = triangles[i];
                int index2 = triangles[i + 1];
                int index3 = triangles[i + 2];

                // Calculate the normal of the triangle
                Vector3 normal = (normals[index1] + normals[index2] + normals[index3]) / 3f;

                // Check if the normal is pointing downward
                if (Vector3.Dot(normal, Vector3.down) > 0.5f) // Adjust threshold as needed
                {
                    // Pull the vertices along their normal
                    vertices[index1] += normals[index1] * growSpeed;
                    vertices[index2] += normals[index2] * growSpeed;
                    vertices[index3] += normals[index3] * growSpeed;
                }
            }

            // Apply the modified vertices back to the mesh
            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

        }
    }
    public void Grow()
    {
        Debug.Log("Grow");
    }
}
