using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.UIElements;

public class CottonCandy : MonoBehaviour
{
    [SerializeField] private float growSpeed = 0.1f;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;

    private Dictionary<string, List<int>> vertexMap = new Dictionary<string, List<int>>();
    //int growingID;
    //int originID;
    //int directionID;
    //int radiusID;
    //int growSpeedID;

    void Start()
    {
        //growingID = Shader.PropertyToID("_Growing");
        //originID = Shader.PropertyToID("_Origin");
        //directionID = Shader.PropertyToID("_Direction");
        //radiusID = Shader.PropertyToID("_Radius");
        //growSpeedID = Shader.PropertyToID("_GrowSpeed");

        if (meshFilter != null)
        {
            Mesh mesh = meshFilter.mesh;
            var vertices = mesh.vertices;
            for (int i = 0; i < mesh.vertexCount; i++)
            {
                Vector3 pos = vertices[i];
                string key = Vector3ToString(pos);
                if (vertexMap.ContainsKey(key)){ 
                    vertexMap[key].Add(i);
                }
                else
                {
                    vertexMap[key] = new List<int> { i };
                }
            }
        }

    }

    private string Vector3ToString(Vector3 vec)
    {
        //We only consider 4-digit precision
        return vec.x.ToString("F4") + "," + vec.y.ToString("F4") + "," + vec.z.ToString("F4");
    }

    public void Grow(Vector3 origin, Vector3 up, float radius, float deltaTime)
    {
        //meshRenderer.material.SetFloat(growingID, 1f);
        //meshRenderer.material.SetVector(originID, origin);
        //meshRenderer.material.SetVector(directionID, up);
        //meshRenderer.material.SetFloat(radiusID, radius);
        //meshRenderer.material.SetFloat(growSpeedID, growSpeed);

        //  Transform to local
        origin = meshFilter.transform.InverseTransformPoint(origin);
        up = meshFilter.transform.InverseTransformDirection(up);
        radius = meshFilter.transform.InverseTransformVector(new Vector3(radius, 0, 0)).magnitude;
        if (meshFilter != null)
        {
            Mesh mesh = meshFilter.mesh;

            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = mesh.normals;

            foreach(var indices in vertexMap)
            {
                Vector3 vertex = vertices[indices.Value[0]];
                Vector3 averageNormal = new Vector3();
                foreach(var index in indices.Value)
                {
                    averageNormal += normals[index];
                }
                averageNormal /= indices.Value.Count;


                //Shortest distance from vertext to up normal
                var shortest = Vector3.Distance(Vector3.Project(vertex - origin, up) + origin, vertex);
                var k1 = 1f - Mathf.Clamp01(shortest / radius);

                //If the vertex is not directly facing the machine, then it grows slower
                var k2 = Vector3.Dot(averageNormal, -up);
                if (k1 * k2 < 0) continue;

                //  Update every vertext
                var newVertex = vertex + k1 * k2 * deltaTime * growSpeed * (-up);
                foreach (var index in indices.Value)
                {
                    vertices[index] = newVertex;
                }
            }
            //// Loop through all triangles
            //for (int i = 0; i < vertices.Length; ++i)
            //{
            //    //Shortest distance from vertext to up normal
            //    var shortest = Vector3.Distance(Vector3.Project(vertices[i] - origin, up) + origin, vertices[i]);
            //    var k1 = 1f - Mathf.Clamp01(shortest / radius);

            //    //If the vertex is not directly facing the machine, then it grows slower
            //    var k2 = Vector3.Dot(normals[i], -up);
            //    if (k2 < -0.6f) k2 = 0;
            //    else k2 = 1;

            //    if (k1 * k2 < 0) continue;

            //    vertices[i] += k1 * k2 * deltaTime * growSpeed * (-up);
            //}

            // Apply the modified vertices back to the mesh
            mesh.vertices = vertices;
            //mesh.RecalculateNormals();
            //mesh.RecalculateBounds();
        }


    }

}
