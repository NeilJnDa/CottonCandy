using DG.Tweening;
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
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float maxVolumn = 0.3f;

    private void Start()
    {
        audioSource.volume = 0f;
    }
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
            particle.Play();
            audioSource.DOFade(maxVolumn, 1f);
        }
    }
    public void StopMachine()
    {
        if (Running == true)
        {
            Running = false;
            effectArea.enabled = false;
            particle.Stop();
            audioSource.DOFade(0f, 1f);

        }
    }


}
