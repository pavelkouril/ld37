using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public float speed = 1.0f;
    public Transform core;
    public Transform lowerHand;
    public Transform upperHand;
    public Transform target;

    public enum State
    {
        STATE_DEFAULT,
        STATE_HORIZONTAL
    }

    public State state;
    private float handLength = 1.0f;
    public float coreAngle;
    public float coreTargetAngle;
    public float lowerAngle;
    public float lowerTargetAngle;
    public float upperAngle;
    public float upperTargetAngle;

    void Start()
    {
        state = State.STATE_DEFAULT;
        coreAngle = 0.0f;
        coreTargetAngle = 0.0f;
        lowerAngle = 0.0f;
        lowerTargetAngle = 0.0f;
        upperAngle = 0.0f;
        upperTargetAngle = 0.0f;
    }

    private bool AngleDirection(float targetAngle, float angle, out float direction)
    {
        float a1 = (targetAngle - angle + 360) % 360;
        float a2 = (angle - targetAngle + 360) % 360;
        if (a1 < a2)
        {
            direction = a1;
        }
        else
        {
            direction = -a2;
        }

        bool reached = false;
        if (direction < speed * 0.5f && direction > speed * -0.5f)
        {
            reached = true;
        }

        direction = Mathf.Max(Mathf.Min(direction, speed), -speed);

        return reached;
    }

    void FixedUpdate()
    {
        // Calculate direction to target and update Core (vertical axis) angle
        Vector3 direction = target.position - core.position;
        coreTargetAngle = (Mathf.Atan2(direction.z, direction.x) * -Mathf.Rad2Deg + 90.0f);

        float coreDir = 0.0f;
        AngleDirection(coreTargetAngle, coreAngle, out coreDir);
        coreAngle += coreDir;

        core.localRotation = Quaternion.AngleAxis(coreAngle, new Vector3(0.0f, 0.0f, 1.0f));

        // Calculate 2 angles using triangulation
        float distance = Vector3.Distance(target.position, core.position);
        upperTargetAngle = 90.0f - Mathf.Asin((distance * 0.5f) / handLength) * Mathf.Rad2Deg * 2.0f;
        lowerTargetAngle = 136.0f - Mathf.Acos((distance * 0.5f) / handLength) * Mathf.Rad2Deg;

        float upperDir = 0.0f;
        AngleDirection(upperTargetAngle, upperAngle, out upperDir);
        upperAngle += upperDir;

        float lowerDir = 0.0f;
        AngleDirection(lowerTargetAngle, lowerAngle, out lowerDir);
        lowerAngle += lowerDir;

        lowerHand.localRotation = Quaternion.AngleAxis(lowerAngle, new Vector3(1.0f, 0.0f, 0.0f));
        upperHand.localRotation = Quaternion.AngleAxis(upperAngle, new Vector3(1.0f, 0.0f, 0.0f));

        Debug.Log("Distance " + distance);
    }
}
