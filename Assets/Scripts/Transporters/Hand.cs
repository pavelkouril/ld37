using System;
using System.Collections;
using System.Collections.Generic;
using OneRoomFactory.Factory;
using UnityEngine;

namespace OneRoomFactory.Transporters
{
    public class Hand : MonoBehaviour, ITransporter
    {
        public float speed = 1.0f;
        public Transform core;
        public Transform lowerHand;
        public Transform upperHand;
        public Transform target;
        private Transform grabbedObject;

        public enum State
        {
            STATE_DEFAULT,
            STATE_GRABBED
        }

        private State state;
        private float handLength = 1.0f;
        private float coreAngle;
        private float coreTargetAngle;
        private float lowerAngle;
        private float lowerTargetAngle;
        private float upperAngle;
        private float upperTargetAngle;

        public Tile Output { get; private set; }

        public Tile Tile { get; set; }

        public BuildRotation Rotation { get; set; }

        public Movable ToMove { get; set; }

        public MovableType AcceptedType;

        private Movable lastMoved { get; set; }

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
            if (ToMove != null)
            {
                CalculateAnglesForTransform(ToMove.transform);
            }
            else
            {
                coreTargetAngle = 0.0f;
                upperTargetAngle = 0.0f;
                lowerTargetAngle = 0.0f;
            }

            float coreDir = 0.0f;
            bool hReached = AngleDirection(coreTargetAngle, coreAngle, out coreDir);
            coreAngle += coreDir;

            core.localRotation = Quaternion.AngleAxis(coreAngle, new Vector3(0.0f, 0.0f, 1.0f));

            float upperDir = 0.0f;
            bool uReached = AngleDirection(upperTargetAngle, upperAngle, out upperDir);
            upperAngle += upperDir;

            float lowerDir = 0.0f;
            bool lReached = AngleDirection(lowerTargetAngle, lowerAngle, out lowerDir);
            lowerAngle += lowerDir;

            lowerHand.localRotation = Quaternion.AngleAxis(lowerAngle, new Vector3(1.0f, 0.0f, 0.0f));
            upperHand.localRotation = Quaternion.AngleAxis(upperAngle, new Vector3(1.0f, 0.0f, 0.0f));

            if (hReached && uReached && lReached)
            {
                switch (state)
                {
                    case State.STATE_DEFAULT:
                        if (ToMove != null)
                        {
                            ToMove.transform.parent = upperHand.transform;
                            ToMove.GetComponent<Rigidbody>().isKinematic = true;
                            grabbedObject = ToMove.transform;
                            ToMove = null;
                            state = State.STATE_GRABBED;
                        }
                        break;

                    case State.STATE_GRABBED:
                        if (grabbedObject != null)
                        {
                            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                            grabbedObject.transform.parent = null;
                            grabbedObject = null;
                        }
                        state = State.STATE_DEFAULT;
                        break;
                }
            }
        }

        private void CalculateAnglesForTransform(Transform t)
        {
            float distance = Vector3.Distance(t.position, core.position);

            Vector3 direction = t.position - core.position;
            coreTargetAngle = (Mathf.Atan2(direction.z, direction.x) * -Mathf.Rad2Deg + 90.0f);

            upperTargetAngle = 90.0f - Mathf.Asin((distance * 0.5f) / handLength) * Mathf.Rad2Deg * 2.0f;
            lowerTargetAngle = 136.0f - Mathf.Acos((distance * 0.5f) / handLength) * Mathf.Rad2Deg;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Movable") && ToMove == null && state == State.STATE_DEFAULT)
            {
                var movable = other.GetComponent<Movable>();
                if (movable != lastMoved)
                {
                    ToMove = movable;
                    lastMoved = movable;
                }
            }
        }
    }
}