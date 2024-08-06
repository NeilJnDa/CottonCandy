using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandInput : MonoBehaviour
{
    [SerializeField] private PlayerInputRouter input;
    [SerializeField] private Transform playerTransform;
    private Quaternion initialLocalRotation;
    [SerializeField] private float rotateSpeed = 3f;

    private void Start()
    {
        initialLocalRotation = transform.rotation;
    }
    private void Update()
    {
        Debug.Log(playerTransform.forward);
        this.transform.rotation *= Quaternion.AngleAxis(input.look.x * rotateSpeed, playerTransform.forward);
    }
}
