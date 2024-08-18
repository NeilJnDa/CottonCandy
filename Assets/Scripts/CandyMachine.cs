using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyMachine : MonoBehaviour
{
    [field:SerializeField]
    public bool Running { get; private set; }

    [SerializeField]
    private BoxCollider effectArea;


    private void Update()
    {
        if(Running)
        {
            var hits = Physics.OverlapBox(effectArea.transform.position + effectArea.center, effectArea.size / 2f, effectArea.transform.rotation);
            foreach(var hit in hits)
            {
                var cottonCandy = hit.GetComponent<CottonCandy>();
                if(cottonCandy != null)
                {
                    cottonCandy.Grow();

                }
            }
        }
    }
    public void StartMachine()
    {
        if(Running== false)
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
