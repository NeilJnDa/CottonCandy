using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyMachine : MonoBehaviour
{
    [field: SerializeField]
    public bool Running { get; private set; }

    [SerializeField]
    private BoxCollider effectArea;
    [SerializeField] private float radius = 0.02f;

    [SerializeField] private float interval = 0.2f;
    private float timer = 0;

    private void Update()
    {
        if (Running)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                timer = interval;
                var hits = Physics.OverlapBox(effectArea.transform.position + effectArea.center, effectArea.size / 2f, effectArea.transform.rotation);
                foreach (var hit in hits)
                {
                    var cottonCandy = hit.GetComponent<CottonCandy>();
                    if (cottonCandy != null)
                    {
                        cottonCandy.Grow(effectArea.transform.position, effectArea.transform.up, radius, interval);
                    }
                }
            }

        }
    }
    public void StartMachine()
    {
        if (Running == false)
        {
            Running = true;
            effectArea.enabled = true;
        }
    }
    public void StopMachine()
    {
        if (Running == true)
        {
            Running = false;
            effectArea.enabled = false;
        }
    }


}
