using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.UIElements;

public class CottonCandy : MonoBehaviour
{
    [SerializeField] private float growSpeed = 0.1f;
    [SerializeField] private bool growing = false;
    [SerializeField] private float minGrowTime = 0.2f;
    private float timer = 0;
    MeshRenderer m_renderer;
    int growingID;
    int originID;
    int directionID;
    int radiusID;
    int growSpeedID;

    void Start()
    {
        m_renderer = GetComponent<MeshRenderer>();
        growingID = Shader.PropertyToID("_Growing");
        originID = Shader.PropertyToID("_Origin");
        directionID = Shader.PropertyToID("_Direction");
        radiusID = Shader.PropertyToID("_Radius");
        growSpeedID = Shader.PropertyToID("_GrowSpeed");

    }

    public void Grow(Vector3 origin, Vector3 up, float radius)
    {
        m_renderer.material.SetFloat(growingID, 1f);
        m_renderer.material.SetVector(originID, origin);
        m_renderer.material.SetVector(directionID, up);
        m_renderer.material.SetFloat(radiusID, radius);
        m_renderer.material.SetFloat(growSpeedID, growSpeed);
        growing = true;
        timer = minGrowTime;
    }

    private void Update()
    {
        if(growing)
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                growing = false;
                m_renderer.material.SetFloat(growingID, 0f);

            }
        }
    }
}
