using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox; 

public class HandInput : MonoBehaviour
{
    [SerializeField] private PlayerInputRouter input;
    [SerializeField] private float rotateSpeed = 3f;
    [Range(-89f, -1f)]
    [SerializeField] private float rotateRangeMin = -30f;
    [Range(1, 89f)]
    [SerializeField] private float rotateRangeMax = 30f;
    private void Start()
    {

    }
    private void Update()
    {
        Vector3 newLocalEuler = transform.localEulerAngles + new Vector3(-input.look.y, 0, -input.look.x);

        newLocalEuler.x = NormalizeAngle(newLocalEuler.x);
        newLocalEuler.z = NormalizeAngle(newLocalEuler.z);

        newLocalEuler.x = Mathf.Clamp(newLocalEuler.x, rotateRangeMin, rotateRangeMax);
        newLocalEuler.y = 0;
        newLocalEuler.z = Mathf.Clamp(newLocalEuler.z, rotateRangeMin, rotateRangeMax);

        transform.localEulerAngles = newLocalEuler;
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
