using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox; 

public class HandInput : MonoBehaviour
{
    [SerializeField] private PlayerInputRouter input;
    [SerializeField]
    [ReadOnly]
    private Vector3 initialLocalPosition;

    [SerializeField] private float moveSpeed = 0.02f;
    [SerializeField] private float moveRangeX = 0.2f;
    [SerializeField] private float moveRangeZ = 0.2f;

    [SerializeField] private float rotateSpeed = 1f;

    [SerializeField] private Transform toRotateTransform;


    private void Start()
    {
        initialLocalPosition = transform.localPosition;
    }
    private void Update()
    {
        var newLocalPosition = transform.localPosition + new Vector3(input.look.x, 0, -input.look.y) * moveSpeed;
        newLocalPosition.x = Mathf.Clamp(newLocalPosition.x, initialLocalPosition.x - moveRangeX, initialLocalPosition.x + moveRangeX);
        newLocalPosition.z = Mathf.Clamp(newLocalPosition.z, initialLocalPosition.z - moveRangeZ, initialLocalPosition.z + moveRangeZ);
        transform.localPosition = newLocalPosition;

        //  Rotate
        toRotateTransform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    // Function to normalize angles to the range -180 to 180 degrees
    private float NormalizeAngle(float angle)
    {
        while (angle > 180f)
        {
            angle -= 360f;
        }
        while (angle < -180f)
        {
            angle += 360f;
        }
        return angle;
    }
}
