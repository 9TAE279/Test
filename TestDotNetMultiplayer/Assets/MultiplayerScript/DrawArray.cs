using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawArray : MonoBehaviour
{
    public int numberOfRays = 20;
    public float rayLength = 10f;
    public float raySpread = 10f;

    void Update()
    {
        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = i * (360f / numberOfRays);
            Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
            Ray ray = new Ray(transform.position, direction);
            Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);
        }
    }
}
