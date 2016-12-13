using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3();

    void FixedUpdate()
    {
        transform.rotation = transform.rotation * Quaternion.AngleAxis(rotationSpeed.x * Time.fixedDeltaTime, new Vector3(1.0f, 0.0f, 0.0f)) *
             Quaternion.AngleAxis(rotationSpeed.y * Time.fixedDeltaTime, new Vector3(0.0f, 1.0f, 0.0f)) *
             Quaternion.AngleAxis(rotationSpeed.z * Time.fixedDeltaTime, new Vector3(0.0f, 0.0f, 1.0f));
    }
}
